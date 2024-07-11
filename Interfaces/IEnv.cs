namespace DemoTask.Interfaces
{
    public interface IEnv
    {
        string MSQL_CONNECTION_STRING { get; set; }
        string OTP_Length { get; set; }
    }
}
