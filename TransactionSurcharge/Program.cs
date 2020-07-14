using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TransactionSurcharge
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false);
            var configuration = builder.Build();

            var appSettings = new AppSettings();
            var section = configuration.GetSection("AppSettings");
            appSettings = section.Get<AppSettings>();

            StartProgram(appSettings);
        }

        private static void StartProgram(AppSettings appSettings)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Welcome to the Transaction Surcharge Calculator");

            Console.WriteLine("Please enter the amount you want to transfer. Amount must be greater than 0!");
            decimal amountToBeTransferred = VerifyInput(Console.ReadLine());

            if (amountToBeTransferred == 0 || amountToBeTransferred > 999999999)
            {
                Console.WriteLine("Input is invalid. Please try again");
                return;
            }
            decimal surchargeFee = ComputeSurchargeFee(amountToBeTransferred, appSettings);

            decimal advisedAmount = amountToBeTransferred - surchargeFee;

            decimal debitChargeFee = ComputeSurchargeFee(advisedAmount, appSettings);

            decimal debitAmount = advisedAmount + debitChargeFee;

            Console.WriteLine($"The surcharge fee is: {surchargeFee}");
            
            Console.WriteLine($"You are advised to transfer {advisedAmount} because we will take care of the surcharge fee. ;)");

            Console.WriteLine($"You will be debitted {debitAmount}.");

            Console.ReadLine();
        }

        private static decimal VerifyInput(string input)
        {
            decimal decimalValueOfInput = 0;
            decimal.TryParse(input, out decimalValueOfInput);
            return decimalValueOfInput;
        }

        private static decimal ComputeSurchargeFee(decimal amountToBeTransferred, AppSettings appSettings)
        {
            decimal feeAmount = 0;
            for (int i = 0; i < appSettings.Fees.Length; i++)
            {
                if (amountToBeTransferred >= appSettings.Fees[i].minAmount && amountToBeTransferred <= appSettings.Fees[i].maxAmount)
                {
                    feeAmount = appSettings.Fees[i].feeAmount;
                }
            }
            return feeAmount;
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
