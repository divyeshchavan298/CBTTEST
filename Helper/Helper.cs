using System.Text;
using System;

namespace DemoTask.Helper
{
    public static class Helper
    {
        private static readonly Random random = new Random();
        private const string Letters = "123456789";
        public const int OTPExceedTime = 30;
        public static string GenerateRandomInt(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(Letters.Length);
                sb.Append(Letters[index]);
            }
            return sb.ToString();
        }
    }
}
