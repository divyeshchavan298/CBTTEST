namespace DemoTask.DAL
{
    public class OtpMaster
    {
        public int nId { get; set; }
        public string sIcNumber { get; set; }
        public string sOtpFor { get; set; } //In Case Of Mobile = {Mobile Number}, Email = {Email}
        public short nOtpType { get; set; }//In Case Of Mobile = 0, Email = 1
        public string sOtp { get; set; }
        public bool bOtpVerify { get; set; }
        public short nResendOTPCount { get; set; }
        public DateTime dtOtpGentnTime { get; set; }
    }
}
