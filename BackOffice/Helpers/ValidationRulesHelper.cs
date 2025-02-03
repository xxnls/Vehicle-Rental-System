using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Other;

namespace BackOffice.Helpers
{
    public static class ValidationRulesHelper
    {
        #region Address
        public static void ValidateFirstLine(AddressDto address, Action<string, string> addError)
        {
            if (string.IsNullOrWhiteSpace(address.FirstLine))
                addError(nameof(address.FirstLine), LocalizationHelper.GetString("Addresses", "ErrorFirstLine1"));
            else if (address.FirstLine.Length > 100)
                addError(nameof(address.FirstLine), LocalizationHelper.GetString("Addresses", "ErrorFirstLine2"));
        }

        public static void ValidateSecondLine(AddressDto address, Action<string, string> addError)
        {
            if (!string.IsNullOrWhiteSpace(address.SecondLine) && address.SecondLine.Length > 100)
                addError(nameof(address.SecondLine), LocalizationHelper.GetString("Addresses", "ErrorSecondLine1"));
        }

        public static void ValidateZipCode(AddressDto address, Action<string, string> addError)
        {
            if (string.IsNullOrWhiteSpace(address.ZipCode))
                addError(nameof(address.ZipCode), LocalizationHelper.GetString("Addresses", "ErrorZipCode1"));
            else if (address.ZipCode.Length > 20)
                addError(nameof(address.ZipCode), LocalizationHelper.GetString("Addresses", "ErrorZipCode2"));
        }

        public static void ValidateCity(AddressDto address, Action<string, string> addError)
        {
            if (string.IsNullOrWhiteSpace(address.City))
                addError(nameof(address.City), LocalizationHelper.GetString("Addresses", "ErrorCity1"));
            else if (address.City.Length > 50)
                addError(nameof(address.City), LocalizationHelper.GetString("Addresses", "ErrorCity2"));
        }

        #endregion
    }
}
