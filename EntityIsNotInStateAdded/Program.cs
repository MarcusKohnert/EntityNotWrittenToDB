using System;
using System.Linq;

namespace EntityIsNotInStateAdded
{
    class Program
    {
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
    }
}