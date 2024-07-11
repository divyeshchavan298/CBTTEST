namespace DemoTask.DAL
{
    public class ClientMaster
    {
        public int nId { get; set; }
        public string sIcNumber { get; set; }
        public string sCustomerName { get; set; }
        public string sMobileNo { get; set; }    
        public string sEmail { get; set; }    
        public bool bPrivacyPolicy { get; set; }
        public string sPin { get; set; }
        public string? sBiometric { get; set; } = string.Empty;
        public DateTime dtCreated { get; set; }
        public DateTime dtLastUpdated { get; set; }
    }
}
