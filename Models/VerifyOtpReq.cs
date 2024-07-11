namespace DemoTask.Models
{
    public class VerifyOtpReq
    {
        public string sIcNumber { get; set; }
        public string sOtp { get; set; }
        public string sOtpFor { get; set; } //In Case Of Mobile = {Mobile Number}, Email = {Email}
        public short nOtpType { get; set; } //In Case Of Mobile = 0, Email = 1
    }
}
