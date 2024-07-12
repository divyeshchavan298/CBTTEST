using DemoTask.Interfaces;

namespace DemoTask.Services
{
    public class Env
    {
        public string MSQL_CONNECTION_STRING { get; set; }
        public string OTP_Length { get; set; }
        public string OTPRESEND_LIMIT { get; set; }
        public string OTPEXPIRE_TIME { get; set; }
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

            OTPRESEND_LIMIT = Environment.GetEnvironmentVariable("OTPRESEND_LIMIT");
            if (OTPRESEND_LIMIT == null)
            {
                OTPRESEND_LIMIT = settings.GetValue<string>("OTPRESEND_LIMIT");
            }

            OTPEXPIRE_TIME = Environment.GetEnvironmentVariable("OTPEXPIRE_TIME");
            if (OTPEXPIRE_TIME == null)
            {
                OTPEXPIRE_TIME = settings.GetValue<string>("OTPEXPIRE_TIME");
            }
        }
    }
}
