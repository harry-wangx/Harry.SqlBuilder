using System;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var sqliteDbContext = new SqliteDbContext();
                var result= sqliteDbContext.Database.EnsureCreated();
                //new CrudOperations(sqliteDbContext.Database.GetDbConnection(),
                //    new Harry.SqlBuilder.Sqlite.SqlBuilder()).Test();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
