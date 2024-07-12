using DemoTask.DAL;
using DemoTask.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoTask.Services
{
    public class DBMigrationService : IDBMigrationService
    {
        private readonly CbtDbContext _cbtDbContext;

        public DBMigrationService(CbtDbContext cbtDbContext)
        {
            _cbtDbContext = cbtDbContext;
        }
        public string ApplyMigration()
        {
            Console.WriteLine("Migration Started");
            try
            {
                if (_cbtDbContext == null)
                {
                    Console.WriteLine("Error");
                    return "Error";
                }

                _cbtDbContext.Database.Migrate();
                Console.WriteLine("Success");
                Console.WriteLine("Migration End");
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex);
                return ex.ToString() + ex.Message;
            }
        }
    }
}
