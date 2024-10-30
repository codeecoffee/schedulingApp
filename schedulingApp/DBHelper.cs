using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Configuration;

namespace schedulingApp
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            // You should store this in app.config or similar in production
            connectionString = "Server=localhost;Database=schedulingdb;Uid=your_username;Pwd=your_password;";
        }

        // Method to validate user credentials
        public bool ValidateUser(string username, string password)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Use parameterized query to prevent SQL injection
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@username", username);
                        // In production, you should hash the password
                        command.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception in production
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        // New method to check if username already exists
        public bool UsernameExists(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return true; // Return true on error to prevent registration
            }
        }

        // New method to register a new user
        public (bool success, string message) RegisterUser(string username, string password, string email, string firstName, string lastName)
        {
            try
            {
                // Check if username already exists
                if (UsernameExists(username))
                {
                    return (false, "Username already exists.");
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO users (username, password, email, first_name, last_name) 
                                   VALUES (@username, @password, @email, @firstName, @lastName)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        // In production, you should hash the password
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@lastName", lastName);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0
                            ? (true, "User registered successfully!")
                            : (false, "Failed to register user.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Registration error: {ex.Message}");
            }
        }

        // Existing methods remain the same...
        public DataRow GetUserDetails(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM users WHERE username = @username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                return dataTable.Rows[0];
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
        }

        public void LogLoginAttempt(string username, bool success)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO login_logs (username, login_timestamp, success) 
                                   VALUES (@username, @timestamp, @success)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@timestamp", DateTime.Now);
                        command.Parameters.AddWithValue("@success", success);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log login attempt: {ex.Message}");
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // New method to add a customer
        public (bool success, string message) AddCustomer(string customerName, string address, string phoneNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // First check if customer with same phone number exists
                    string checkQuery = "SELECT COUNT(*) FROM customers WHERE phone_number = @phone";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@phone", phoneNumber);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                        if (count > 0)
                        {
                            return (false, "A customer with this phone number already exists.");
                        }
                    }

                    // If no duplicate, proceed with insertion
                    string insertQuery = @"INSERT INTO customers (customer_name, address, phone_number, created_date) 
                                        VALUES (@name, @address, @phone, @created)";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", customerName.Trim());
                        command.Parameters.AddWithValue("@address", address.Trim());
                        command.Parameters.AddWithValue("@phone", phoneNumber.Trim());
                        command.Parameters.AddWithValue("@created", DateTime.Now);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0
                            ? (true, "Customer added successfully!")
                            : (false, "Failed to add customer.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error adding customer: {ex.Message}");
            }
        }

        // Method to get all customers
        public DataTable GetAllCustomers()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customers ORDER BY customer_name";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable customersTable = new DataTable();
                            adapter.Fill(customersTable);
                            return customersTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customers: {ex.Message}");
                return null;
            }
        }

        public DataRow GetCustomerById(int customerId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customers WHERE customer_id = @customerId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving customer: {ex.Message}");
                return null;
            }
        }

        // Method to update existing customer
        public (bool success, string message) UpdateCustomer(int customerId, string customerName, string address, string phoneNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if phone number exists for other customers
                    string checkQuery = "SELECT COUNT(*) FROM customers WHERE phone_number = @phone AND customer_id != @customerId";
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@phone", phoneNumber);
                        checkCommand.Parameters.AddWithValue("@customerId", customerId);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                        if (count > 0)
                        {
                            return (false, "This phone number is already assigned to another customer.");
                        }
                    }

                    string updateQuery = @"UPDATE customers 
                                        SET customer_name = @name, 
                                            address = @address, 
                                            phone_number = @phone 
                                        WHERE customer_id = @customerId";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@name", customerName.Trim());
                        command.Parameters.AddWithValue("@address", address.Trim());
                        command.Parameters.AddWithValue("@phone", phoneNumber.Trim());

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0
                            ? (true, "Customer updated successfully!")
                            : (false, "No changes were made to the customer.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error updating customer: {ex.Message}");
            }
        }

        // Method to delete customer
        public (bool success, string message) DeleteCustomer(int customerId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM customers WHERE customer_id = @customerId";

                    using (MySqlCommand command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0
                            ? (true, "Customer deleted successfully!")
                            : (false, "Customer not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting customer: {ex.Message}");
            }
        }
    }
}