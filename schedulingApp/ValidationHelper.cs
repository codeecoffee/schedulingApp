using System;
using System.Text.RegularExpressions;

namespace schedulingApp
{
    public class ValidationHelper
    {
        // Username validation
        public static (bool isValid, string message) ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Username is required.");

            if (username.Length < 3)
                return (false, "Username must be at least 3 characters long.");

            if (username.Length > 50)
                return (false, "Username cannot exceed 50 characters.");

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
                return (false, "Username can only contain letters, numbers, and underscores.");

            return (true, string.Empty);
        }

        // Password validation
        public static (bool isValid, string message) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required.");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters long.");

            if (password.Length > 50)
                return (false, "Password cannot exceed 50 characters.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return (false, "Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                return (false, "Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                return (false, "Password must contain at least one number.");

            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
                return (false, "Password must contain at least one special character.");

            return (true, string.Empty);
        }

        // Password match validation
        public static (bool isValid, string message) ValidatePasswordMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return (false, "Passwords do not match.");

            return (true, string.Empty);
        }

        // Email validation
        public static (bool isValid, string message) ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email is required.");

            if (email.Length > 100)
                return (false, "Email cannot exceed 100 characters.");

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    return (false, "Invalid email format.");
            }
            catch
            {
                return (false, "Invalid email format.");
            }

            return (true, string.Empty);
        }

        // Name validation
        public static (bool isValid, string message) ValidateName(string name, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, $"{fieldName} is required.");

            if (name.Length > 50)
                return (false, $"{fieldName} cannot exceed 50 characters.");

            if (!Regex.IsMatch(name, @"^[a-zA-Z\s\-']+$"))
                return (false, $"{fieldName} can only contain letters, spaces, hyphens, and apostrophes.");

            return (true, string.Empty);
        }

        // Login form validation
        public static (bool isValid, string message) ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return (false, "Please enter both username and password.");

            return (true, string.Empty);
        }

        // Registration form validation
        public static (bool isValid, string message) ValidateRegistrationInput(
            string username,
            string password,
            string confirmPassword,
            string email,
            string firstName,
            string lastName)
        {
            // Validate username
            var (isUsernameValid, usernameMessage) = ValidateUsername(username);
            if (!isUsernameValid)
                return (false, usernameMessage);

            // Validate password
            var (isPasswordValid, passwordMessage) = ValidatePassword(password);
            if (!isPasswordValid)
                return (false, passwordMessage);

            // Validate password match
            var (isPasswordMatchValid, passwordMatchMessage) = ValidatePasswordMatch(password, confirmPassword);
            if (!isPasswordMatchValid)
                return (false, passwordMatchMessage);

            // Validate email
            var (isEmailValid, emailMessage) = ValidateEmail(email);
            if (!isEmailValid)
                return (false, emailMessage);

            // Validate first name
            var (isFirstNameValid, firstNameMessage) = ValidateName(firstName, "First name");
            if (!isFirstNameValid)
                return (false, firstNameMessage);

            // Validate last name
            var (isLastNameValid, lastNameMessage) = ValidateName(lastName, "Last name");
            if (!isLastNameValid)
                return (false, lastNameMessage);

            return (true, string.Empty);
        }
    }
}