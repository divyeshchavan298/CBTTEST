using DemoTask.Interfaces;

namespace DemoTask.Services
{
    public class Env : IEnv
    {
        public string MSQL_CONNECTION_STRING { get; set; }
        public string OTP_Length { get; set; }
        public Env(IConfigurationSection settings)
        {
            MSQL_CONNECTION_STRING = Environment.GetEnvironmentVariable("MSQL_CONNECTION_STRING");
            if (MSQL_CONNECTION_STRING == null)
            {
                MSQL_CONNECTION_STRING = settings.GetValue<string>("MSQL_CONNECTION_STRING");
            }

            OTP_Length = Environment.GetEnvironmentVariable("OTP_Length");
            if (OTP_Length == null)
            {
                OTP_Length = settings.GetValue<string>("OTP_Length");
            }
        }
    }
}
