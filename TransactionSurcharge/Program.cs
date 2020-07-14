using System;

namespace TransactionSurcharge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class AppSettings
    {
        public FeesConfig[] Fees { get; set; }
    }

    public class FeesConfig
    {
        public decimal minAmount { get; set; }
        public decimal maxAmount { get; set; }
        public decimal feeAmount { get; set; }
    }
}
