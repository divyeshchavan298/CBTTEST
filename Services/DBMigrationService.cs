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
            Console.WriteLine("Success");
            _cbtDbContext = cbtDbContext;
        }
        public string ApplyMigration()
        {
            Console.WriteLine("In ApplyMigration: Step 1");
            try
            {
                if (_cbtDbContext == null)
                {
                    Console.WriteLine("Error");
                    return "Error";
                }

                _cbtDbContext.Database.Migrate();
                Console.WriteLine("Success");
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("In ApplyMigration: Step 6");
                Console.WriteLine("Exception " + ex);

                //_logger.LogError(ex.ToString() + ex.Message);
                return ex.ToString() + ex.Message;
            }
        }
    }
}
