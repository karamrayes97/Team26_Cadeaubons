using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Services
{
    public static class VoucherNumberHelper
    {
        private static Random _random = new Random();

        // Generate a voucher number in the format YEAR-RANDOM-CHECK
        // Generated voucher number % 97 = 1
        public static string GenerateNumber(int randomDigits = 6)
        {
            if (randomDigits < 1)
                throw new ArgumentException("Random digits must be at least 1.", nameof(randomDigits));

            string year = DateTime.Now.Year.ToString();

            string randomNumber = "";
            for (int i = 0; i < randomDigits; i++)
                randomNumber += _random.Next(0, 10).ToString();

            string tempNumber = year + randomNumber + "00"; 
            int checkDigits = 98 - CalculateMod97Checksum(tempNumber);

            return $"{year}-{randomNumber}-{checkDigits:D2}";
        }

        public static bool IsValidNumber(string voucherNumber)
        {
            if (string.IsNullOrWhiteSpace(voucherNumber))
                return false;

            var parts = voucherNumber.Split('-');
            if (parts.Length != 3)
                return false;

            string year = parts[0];
            string randomNumber = parts[1];
            string checkDigitsStr = parts[2];

            if (!int.TryParse(year, out _) || 
                !long.TryParse(randomNumber, out _) || 
                !int.TryParse(checkDigitsStr, out int checkDigits))
                return false;

            string tempNumber = year + randomNumber + "00";
            int expectedCheck = 98 - CalculateMod97Checksum(tempNumber);

            return checkDigits == expectedCheck;
        }

        private static int CalculateMod97Checksum(string number)
        {
            int checksum = 0;
            foreach (char c in number)
            {
                if (!char.IsDigit(c))
                    throw new ArgumentException("Number must contain only digits.", nameof(number));

                int digit = c - '0';
                checksum = (checksum * 10 + digit) % 97;
            }
            return checksum;
        }
    }
}
