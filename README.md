# EntityNotWrittenToDB
Sample repo to demo unexpected behaviour of EF Core 2.0.3.

The [database](EntityNotWrittenToDB/EntityIsNotInStateAdded/dbSchema.sql) contains a One-To-One relationship from Attributes to Persons.
Meaning, there is exactly one attribute entity for each person.

### Entities:
```csharp
public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }
}

public class Attributes
{
    public Person Person { get; set; }

    public string Attribs { get; set; }
}
```

### DbContext Config:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Attributes>()
                .Property<int>("PersonId");

    modelBuilder.Entity<Attributes>()
                .HasKey("PersonId");

    modelBuilder.Entity<Attributes>()
                .HasOne(_ => _.Person)
                .WithOne()
                .IsRequired();

    modelBuilder.Entity<Attributes>()
                .Property(_ => _.Attribs)
                .HasColumnName("Attributes");
}

```

### What works very well for Attributes?
* Read
* Update
* Delete

### What does not work?
Even though a new _Attributes_ instance is added to the _DbSet_ it's not marked as _Added_ and thus not commited to the db.
Marking the entity state explicitly as _Added_ commits the instance correctly to the db. 
This explicitness is only necessary for _Attributes_. All other entities in my real project are correctly added through DbSet<T>.Add().

What's wrong with my expectations?
See sample...

```csharp
static void Main(string[] args)
{
    using (var dbContext = new Context())
    {
        var readEntity = dbContext.Set<Attributes>()
                                  .First(_ => _.Person.Id == 1);

        Console.WriteLine($"Read entity has attributes value: {readEntity.Attribs}");
    }

    using (var dbContext = new Context())
    {
        var person = dbContext.Set<Person>().First(_ => _.Id == 2);

        var attributes = new Attributes
        {
            Person  = person,
            Attribs = "some"
        };

        var state = dbContext.Set<Attributes>().Add(attributes);
        Console.WriteLine($"State: {state.State}");

        var count = dbContext.SaveChanges();
        Console.WriteLine($"Changes written: {count}");

        if (count == 0)
        {
            Console.WriteLine("Trying again with explicitly setting entity state to Added.");

            state.State = Microsoft.EntityFrameworkCore.EntityState.Added;
            count = dbContext.SaveChanges();

            Console.WriteLine($"Changes written in second try: {count}");
        }
    }

    Console.WriteLine("done");
    Console.ReadLine();
}
```
