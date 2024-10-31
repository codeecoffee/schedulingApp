using System;
using System.Text.RegularExpressions;

namespace schedulingApp
{
    public class ValidationHelper
    {
        // User validation
        public static (bool isValid, string message) ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, "Please enter both username and password.");

            if (username.Length > 50)
                return (false, "Username cannot exceed 50 characters.");

            if (password.Length > 50)
                return (false, "Password cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        //// Password validation
        //public static (bool isValid, string message) ValidatePassword(string password)
        //{
        //    if (string.IsNullOrWhiteSpace(password))
        //        return (false, "Password is required.");

        //    if (password.Length < 8)
        //        return (false, "Password must be at least 8 characters long.");

        //    if (password.Length > 50)
        //        return (false, "Password cannot exceed 50 characters.");

        //    if (!Regex.IsMatch(password, @"[A-Z]"))
        //        return (false, "Password must contain at least one uppercase letter.");

        //    if (!Regex.IsMatch(password, @"[a-z]"))
        //        return (false, "Password must contain at least one lowercase letter.");

        //    if (!Regex.IsMatch(password, @"[0-9]"))
        //        return (false, "Password must contain at least one number.");

        //    if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
        //        return (false, "Password must contain at least one special character.");

        //    return (true, string.Empty);
        //}

        // Password match validation
        public static (bool isValid, string message) ValidatePasswordMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return (false, "Passwords do not match.");

            return (true, string.Empty);
        }

        // Customer validation
        public static (bool isValid, string message) ValidateCustomerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Customer name is required.");

            if (name.Length > 45)
                return (false, "Customer name cannot exceed 45 characters.");

            return (true, string.Empty);
        }

        // Address validation
        public static (bool isValid, string message) ValidateAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return (false, "Address is required.");

            if (address.Length > 50)
                return (false, "Address cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        public static (bool isValid, string message) ValidateAddress2(string address2)
        {
            if (!string.IsNullOrWhiteSpace(address2) && address2.Length > 50)
                return (false, "Address2 cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        public static (bool isValid, string message) ValidatePostalCode(string postalCode)
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return (false, "Postal code is required.");

            if (postalCode.Length > 10)
                return (false, "Postal code cannot exceed 10 characters.");

            return (true, string.Empty);
        }

        public static (bool isValid, string message) ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return (false, "Phone number is required.");

            if (phone.Length > 20)
                return (false, "Phone number cannot exceed 20 characters.");

            return (true, string.Empty);
        }

        // City validation
        public static (bool isValid, string message) ValidateCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return (false, "City is required.");

            if (city.Length > 50)
                return (false, "City cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        // Country validation
        public static (bool isValid, string message) ValidateCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                return (false, "Country is required.");

            if (country.Length > 50)
                return (false, "Country cannot exceed 50 characters.");

            return (true, string.Empty);
        }

        // Validate complete customer input including address
        public static (bool isValid, string message) ValidateCustomerInput(
            string customerName,
            string address,
            string address2,
            string postalCode,
            string phone,
            string city,
            string country)
        {
            var (isNameValid, nameMessage) = ValidateCustomerName(customerName);
            if (!isNameValid)
                return (false, nameMessage);

            var (isAddressValid, addressMessage) = ValidateAddress(address);
            if (!isAddressValid)
                return (false, addressMessage);

            var (isAddress2Valid, address2Message) = ValidateAddress2(address2);
            if (!isAddress2Valid)
                return (false, address2Message);

            var (isPostalValid, postalMessage) = ValidatePostalCode(postalCode);
            if (!isPostalValid)
                return (false, postalMessage);

            var (isPhoneValid, phoneMessage) = ValidatePhone(phone);
            if (!isPhoneValid)
                return (false, phoneMessage);

            var (isCityValid, cityMessage) = ValidateCity(city);
            if (!isCityValid)
                return (false, cityMessage);

            var (isCountryValid, countryMessage) = ValidateCountry(country);
            if (!isCountryValid)
                return (false, countryMessage);

            return (true, string.Empty);
        }
    }
}