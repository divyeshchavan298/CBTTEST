using System.ComponentModel.DataAnnotations;

namespace DemoTask.Models
{
    public class RegisterUserReq
    {

        public string sIcNumber { get; set; }
        public string sCustomerName { get; set; }
        public string sMobileNo { get; set; }
        public string sEmail { get; set; }

    }
}
