using MySqlConnector;
using System;
using System.Data;
using System.Configuration;


namespace schedulingApp
{
    public class DatabaseHelper
    {
        private readonly string connectionString;
        private readonly string currentUser;

        public DatabaseHelper(string currentUser = "system")
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            this.currentUser = currentUser;
        }
        //[OK] Opens the connection and Setup the DB if no costumer is found on the DB. 
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    SetupDatabase();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        
        //Calls the does the comparison to the DB Customers and calls the initialization
        public void SetupDatabase()
        {
            var customers = GetAllCustomers();
            if (customers != null && customers.Rows.Count > 0)
            {
                Console.WriteLine($"Found {customers.Rows.Count} customers in database.");
                
            }
            else 
            {
                var dbSetup= new DatabaseSetup();
                dbSetup.InitializeDatabase(connectionString);
            }
        }

        //[?] Method to validate user credentials - used in the Login Screen
        public bool ValidateUser(string username, string password)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM user WHERE userName = @username AND password = @password AND active = 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        //[Ok] method to check if username already exists
        public (bool exists, string message) UsernameExists(string username)
        {  
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM user WHERE username = @username";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    //MessageBox.Show($"You hit [Func UsernameExists:DBHelper] and this is the user count: {count}");

                    if (count > 0) { return (true, "Username already taken"); }
                    else { return (false,""); }
                   
                }
            }
        }

        // [Ok] method to register a new user
        public (bool success, string message) RegisterUser(string username, string password)
        {
            try
            {
                bool userIsInDB = UsernameExists(username).exists;

                //MessageBox.Show($"[Func RegisterUser] this is the value on userIsInDB: {userIsInDB }");
                
                //First check if username already exists
                if (userIsInDB)
                {
                    return (false, "Username already exists.");
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO user 
                (userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy) 
                VALUES 
                (@username, @password, 1, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        DateTime now = DateTime.Now;
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@createDate", now);
                        command.Parameters.AddWithValue("@createdBy", "system");
                        command.Parameters.AddWithValue("@lastUpdate", now);
                        command.Parameters.AddWithValue("@lastUpdateBy", "system");

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
               
        private int InsertCountry(MySqlConnection connection, string countryName)
        {
            string query = @"INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) 
                           VALUES (@country, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                           SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                DateTime now = DateTime.Now;
                command.Parameters.AddWithValue("@country", countryName);
                command.Parameters.AddWithValue("@createDate", now);
                command.Parameters.AddWithValue("@createdBy", currentUser);
                command.Parameters.AddWithValue("@lastUpdate", now);
                command.Parameters.AddWithValue("@lastUpdateBy", currentUser);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private int InsertCity(MySqlConnection connection, string cityName, int countryId)
        {
            string query = @"INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) 
                           VALUES (@city, @countryId, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                           SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                DateTime now = DateTime.Now;
                command.Parameters.AddWithValue("@city", cityName);
                command.Parameters.AddWithValue("@countryId", countryId);
                command.Parameters.AddWithValue("@createDate", now);
                command.Parameters.AddWithValue("@createdBy", currentUser);
                command.Parameters.AddWithValue("@lastUpdate", now);
                command.Parameters.AddWithValue("@lastUpdateBy", currentUser);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private int InsertAddress(MySqlConnection connection, string address1, string address2, int cityId, string postalCode, string phone)
        {
            string query = @"INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) 
                           VALUES (@address, @address2, @cityId, @postalCode, @phone, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                           SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                DateTime now = DateTime.Now;
                command.Parameters.AddWithValue("@address", address1);
                command.Parameters.AddWithValue("@address2", address2 ?? "");
                command.Parameters.AddWithValue("@cityId", cityId);
                command.Parameters.AddWithValue("@postalCode", postalCode);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@createDate", now);
                command.Parameters.AddWithValue("@createdBy", currentUser);
                command.Parameters.AddWithValue("@lastUpdate", now);
                command.Parameters.AddWithValue("@lastUpdateBy", currentUser);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public (bool success, string message) AddCustomer(string customerName, string address1, string address2,
                   string city, string country, string postalCode, string phone)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Insert country
                            int countryId = InsertCountry(connection, country);

                            // Insert city
                            int cityId = InsertCity(connection, city, countryId);

                            // Insert address
                            int addressId = InsertAddress(connection, address1, address2, cityId, postalCode, phone);

                            // Insert customer
                            string customerQuery = @"INSERT INTO customer 
                                (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                                VALUES 
                                (@customerName, @addressId, 1, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";

                            using (MySqlCommand command = new MySqlCommand(customerQuery, connection))
                            {
                                DateTime now = DateTime.Now;
                                command.Parameters.AddWithValue("@customerName", customerName);
                                command.Parameters.AddWithValue("@addressId", addressId);
                                command.Parameters.AddWithValue("@createDate", now);
                                command.Parameters.AddWithValue("@createdBy", currentUser);
                                command.Parameters.AddWithValue("@lastUpdate", now);
                                command.Parameters.AddWithValue("@lastUpdateBy", currentUser);

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return (true, "Customer added successfully!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error in transaction: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error adding customer: {ex.Message}");
            }
        }

        // Get them all!!
        public DataTable GetAllCustomers()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            c.customerId,
                            c.customerName,
                            a.address,
                            a.address2,
                            a.postalCode,
                            a.phone,
                            ci.city,
                            co.country,
                            c.active
                        FROM customer c
                        JOIN address a ON c.addressId = a.addressId
                        JOIN city ci ON a.cityId = ci.cityId
                        JOIN country co ON ci.countryId = co.countryId
                        ORDER BY c.customerName";

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

        // Customer update
        public (bool success, string message) UpdateCustomer(
        int customerId,
        string customerName,
        string address1,
        string address2,
        string city,
        string country,
        string postalCode,
        string phone)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Get current customer data for comparison
                            string getCurrentDataQuery = @"
                            SELECT 
                                c.addressId,
                                a.cityId,
                                ci.countryId,
                                a.address,
                                ci.city,
                                co.country
                            FROM customer c
                            JOIN address a ON c.addressId = a.addressId
                            JOIN city ci ON a.cityId = ci.cityId
                            JOIN country co ON ci.countryId = co.countryId
                            WHERE c.customerId = @customerId";

                            int currentAddressId, currentCityId, currentCountryId;
                            using (MySqlCommand command = new MySqlCommand(getCurrentDataQuery, connection))
                            {
                                command.Parameters.AddWithValue("@customerId", customerId);
                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        return (false, "Customer not found.");
                                    }
                                    currentAddressId = reader.GetInt32("addressId");
                                    currentCityId = reader.GetInt32("cityId");
                                    currentCountryId = reader.GetInt32("countryId");
                                }
                            }

                            // Update country or create new if changed
                            int countryId = currentCountryId;
                            string checkCountryQuery = "SELECT countryId FROM country WHERE country = @country";
                            using (MySqlCommand command = new MySqlCommand(checkCountryQuery, connection))
                            {
                                command.Parameters.AddWithValue("@country", country);
                                object result = command.ExecuteScalar();
                                if (result == null)
                                {
                                    countryId = InsertCountry(connection, country);
                                }
                                else
                                {
                                    countryId = Convert.ToInt32(result);
                                }
                            }

                            // Update city or create new if changed
                            int cityId = currentCityId;
                            string checkCityQuery = "SELECT cityId FROM city WHERE city = @city AND countryId = @countryId";
                            using (MySqlCommand command = new MySqlCommand(checkCityQuery, connection))
                            {
                                command.Parameters.AddWithValue("@city", city);
                                command.Parameters.AddWithValue("@countryId", countryId);
                                object result = command.ExecuteScalar();
                                if (result == null)
                                {
                                    cityId = InsertCity(connection, city, countryId);
                                }
                                else
                                {
                                    cityId = Convert.ToInt32(result);
                                }
                            }

                            // Update address
                            string updateAddressQuery = @"
                            UPDATE address 
                            SET 
                                address = @address,
                                address2 = @address2,
                                cityId = @cityId,
                                postalCode = @postalCode,
                                phone = @phone,
                                lastUpdate = @lastUpdate,
                                lastUpdateBy = @lastUpdateBy
                            WHERE addressId = @addressId";

                            using (MySqlCommand command = new MySqlCommand(updateAddressQuery, connection))
                            {
                                DateTime now = DateTime.Now;
                                command.Parameters.AddWithValue("@addressId", currentAddressId);
                                command.Parameters.AddWithValue("@address", address1);
                                command.Parameters.AddWithValue("@address2", address2 ?? "");
                                command.Parameters.AddWithValue("@cityId", cityId);
                                command.Parameters.AddWithValue("@postalCode", postalCode);
                                command.Parameters.AddWithValue("@phone", phone);
                                command.Parameters.AddWithValue("@lastUpdate", now);
                                command.Parameters.AddWithValue("@lastUpdateBy", currentUser);
                                command.ExecuteNonQuery();
                            }

                            // Update customer
                            string updateCustomerQuery = @"
                            UPDATE customer 
                            SET 
                                customerName = @customerName,
                                lastUpdate = @lastUpdate,
                                lastUpdateBy = @lastUpdateBy
                            WHERE customerId = @customerId";

                            using (MySqlCommand command = new MySqlCommand(updateCustomerQuery, connection))
                            {
                                DateTime now = DateTime.Now;
                                command.Parameters.AddWithValue("@customerId", customerId);
                                command.Parameters.AddWithValue("@customerName", customerName);
                                command.Parameters.AddWithValue("@lastUpdate", now);
                                command.Parameters.AddWithValue("@lastUpdateBy", currentUser);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return (true, "Customer updated successfully!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error in transaction: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error updating customer: {ex.Message}");
            }
        }

        public (bool success, string message) DeleteCustomer(int customerId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            //check if customer has any appointments
                            string checkAppointmentsQuery = "SELECT COUNT(*) FROM appointment WHERE customerId = @customerId";
                            using (MySqlCommand command = new MySqlCommand(checkAppointmentsQuery, connection))
                            {
                                command.Parameters.AddWithValue("@customerId", customerId);
                                int appointmentCount = Convert.ToInt32(command.ExecuteScalar());
                                if (appointmentCount > 0)
                                {
                                    return (false, "Cannot delete customer with existing appointments. Please delete appointments first.");
                                }
                            }

                            // Get the addressId for the customer
                            string getAddressIdQuery = "SELECT addressId FROM customer WHERE customerId = @customerId";
                            int addressId;
                            using (MySqlCommand command = new MySqlCommand(getAddressIdQuery, connection))
                            {
                                command.Parameters.AddWithValue("@customerId", customerId);
                                object result = command.ExecuteScalar();
                                if (result == null)
                                {
                                    return (false, "Customer not found.");
                                }
                                addressId = Convert.ToInt32(result);
                            }

                            // Delete the customer
                            string deleteCustomerQuery = "DELETE FROM customer WHERE customerId = @customerId";
                            using (MySqlCommand command = new MySqlCommand(deleteCustomerQuery, connection))
                            {
                                command.Parameters.AddWithValue("@customerId", customerId);
                                command.ExecuteNonQuery();
                            }

                            // Get cityId from address
                            string getCityIdQuery = "SELECT cityId FROM address WHERE addressId = @addressId";
                            int cityId;
                            using (MySqlCommand command = new MySqlCommand(getCityIdQuery, connection))
                            {
                                command.Parameters.AddWithValue("@addressId", addressId);
                                cityId = Convert.ToInt32(command.ExecuteScalar());
                            }

                            // Delete the address
                            string deleteAddressQuery = "DELETE FROM address WHERE addressId = @addressId";
                            using (MySqlCommand command = new MySqlCommand(deleteAddressQuery, connection))
                            {
                                command.Parameters.AddWithValue("@addressId", addressId);
                                command.ExecuteNonQuery();
                            }

                            // Check if city has other addresses
                            string checkCityQuery = "SELECT COUNT(*) FROM address WHERE cityId = @cityId";
                            using (MySqlCommand command = new MySqlCommand(checkCityQuery, connection))
                            {
                                command.Parameters.AddWithValue("@cityId", cityId);
                                int addressCount = Convert.ToInt32(command.ExecuteScalar());
                                if (addressCount == 0)
                                {
                                    // Get countryId from city
                                    string getCountryIdQuery = "SELECT countryId FROM city WHERE cityId = @cityId";
                                    int countryId;
                                    using (MySqlCommand cmd = new MySqlCommand(getCountryIdQuery, connection))
                                    {
                                        cmd.Parameters.AddWithValue("@cityId", cityId);
                                        countryId = Convert.ToInt32(cmd.ExecuteScalar());
                                    }

                                    // Delete the city
                                    string deleteCityQuery = "DELETE FROM city WHERE cityId = @cityId";
                                    using (MySqlCommand cmd = new MySqlCommand(deleteCityQuery, connection))
                                    {
                                        cmd.Parameters.AddWithValue("@cityId", cityId);
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Check if country has other cities
                                    string checkCountryQuery = "SELECT COUNT(*) FROM city WHERE countryId = @countryId";
                                    using (MySqlCommand cmd = new MySqlCommand(checkCountryQuery, connection))
                                    {
                                        cmd.Parameters.AddWithValue("@countryId", countryId);
                                        int cityCount = Convert.ToInt32(cmd.ExecuteScalar());
                                        if (cityCount == 0)
                                        {
                                            // Delete the country
                                            string deleteCountryQuery = "DELETE FROM country WHERE countryId = @countryId";
                                            using (MySqlCommand cmdDelCountry = new MySqlCommand(deleteCountryQuery, connection))
                                            {
                                                cmdDelCountry.Parameters.AddWithValue("@countryId", countryId);
                                                cmdDelCountry.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            return (true, "Customer and related records deleted successfully!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error in transaction: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting customer: {ex.Message}");
            }
        }

        //Main screen stuff

        public DataTable GetUpcomingAppointments(string username, int minutesThreshold)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    a.appointmentId,
                    a.customerId,
                    c.customerName,
                    a.userId,
                    a.title,
                    a.description,
                    a.location,
                    a.contact,
                    a.type,
                    a.url,
                    a.start,
                    a.end
                FROM appointment a
                JOIN customer c ON a.customerId = c.customerId
                JOIN user u ON a.userId = u.userId
                WHERE u.userName = @username
                AND a.start BETWEEN UTC_TIMESTAMP() AND UTC_TIMESTAMP() + INTERVAL @minutes MINUTE
                ORDER BY a.start";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@minutes", minutesThreshold);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable appointmentsTable = new DataTable();
                            adapter.Fill(appointmentsTable);
                            return appointmentsTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving upcoming appointments: {ex.Message}");
                return null;
            }
        }
        public DataTable GetAllAppointments()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    a.appointmentId,
                    a.customerId,
                    c.customerName,
                    a.userId,
                    a.title,
                    a.description,
                    a.location,
                    a.contact,
                    a.type,
                    a.url,
                    a.start,
                    a.end
                FROM appointment a
                JOIN customer c ON a.customerId = c.customerId
                ORDER BY a.start";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable appointmentsTable = new DataTable();
                            adapter.Fill(appointmentsTable);
                            return appointmentsTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving appointments: {ex.Message}");
                return null;
            }
        }

        public DataTable GetAppointmentsByDate(DateTime date)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    a.appointmentId,
                    a.customerId,
                    c.customerName,
                    a.userId,
                    a.title,
                    a.description,
                    a.location,
                    a.contact,
                    a.type,
                    a.url,
                    a.start,
                    a.end
                FROM appointment a
                JOIN customer c ON a.customerId = c.customerId
                WHERE DATE(a.start) = DATE(@date)
                ORDER BY a.start";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@date", date.Date);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable appointmentsTable = new DataTable();
                            adapter.Fill(appointmentsTable);
                            return appointmentsTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving appointments: {ex.Message}");
                return null;
            }
        }

        public Dictionary<DateTime, int> GetMonthlyAppointmentCounts(DateTime month)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT DATE(start) as date, COUNT(*) as count
                FROM appointment
                WHERE YEAR(start) = YEAR(@date) AND MONTH(start) = MONTH(@date)
                GROUP BY DATE(start)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@date", month);
                        Dictionary<DateTime, int> counts = new Dictionary<DateTime, int>();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                counts.Add(
                                    reader.GetDateTime("date"),
                                    reader.GetInt32("count")
                                );
                            }
                        }
                        return counts;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving appointment counts: {ex.Message}");
                return new Dictionary<DateTime, int>();
            }
        }

        public DataTable GetCustomerAppointments(int customerId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    a.appointmentId,
                    a.customerId,
                    c.customerName,
                    a.userId,
                    a.title,
                    a.description,
                    a.location,
                    a.contact,
                    a.type,
                    a.url,
                    a.start,
                    a.end
                FROM appointment a
                JOIN customer c ON a.customerId = c.customerId
                WHERE a.customerId = @customerId
                ORDER BY a.start";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable appointmentsTable = new DataTable();
                            adapter.Fill(appointmentsTable);
                            return appointmentsTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving appointments: {ex.Message}");
                return null;
            }
        }

        public bool HasOverlappingAppointments(int customerId, DateTime startTime, DateTime endTime, int? excludeAppointmentId = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT COUNT(*)
                FROM appointment
                WHERE customerId = @customerId
                AND ((start BETWEEN @start AND @end)
                     OR (end BETWEEN @start AND @end)
                     OR (start <= @start AND end >= @end))";

                    if (excludeAppointmentId.HasValue)
                        query += " AND appointmentId != @excludeAppointmentId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@start", startTime);
                        command.Parameters.AddWithValue("@end", endTime);
                        if (excludeAppointmentId.HasValue)
                            command.Parameters.AddWithValue("@excludeAppointmentId", excludeAppointmentId.Value);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking overlapping appointments: {ex.Message}");
                return true; // Return true on error to prevent scheduling
            }
        }

        public (bool success, string message) DeleteAppointment(int appointmentId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM appointment WHERE appointmentId = @appointmentId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@appointmentId", appointmentId);
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0
                            ? (true, "Appointment cancelled successfully!")
                            : (false, "Appointment not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error cancelling appointment: {ex.Message}");
            }
        }

        public (bool success, string message) AddAppointment(
            int customerId,
            int userId,
            string title,
            string description,
            string location,
            string contact,
            string type,
            string url,
            DateTime startLocal,
            DateTime endLocal)
        {
            try
            {
                // Convert to UTC for storage
                DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal);
                DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal);

                // Check for overlapping appointments
                if (HasOverlappingAppointments(customerId, startUtc, endUtc))
                {
                    return (false, "This customer already has an appointment scheduled during this time.");
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                INSERT INTO appointment 
                (customerId, userId, title, description, location, contact, type, url,
                 start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                VALUES 
                (@customerId, @userId, @title, @description, @location, @contact, @type,
                 @url, @start, @end, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        DateTime now = DateTime.UtcNow;
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@location", location);
                        command.Parameters.AddWithValue("@contact", contact);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@url", url);
                        command.Parameters.AddWithValue("@start", startUtc);
                        command.Parameters.AddWithValue("@end", endUtc);
                        command.Parameters.AddWithValue("@createDate", now);
                        command.Parameters.AddWithValue("@createdBy", currentUser);
                        command.Parameters.AddWithValue("@lastUpdate", now);
                        command.Parameters.AddWithValue("@lastUpdateBy", currentUser);

                        command.ExecuteNonQuery();
                        return (true, "Appointment added successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error adding appointment: {ex.Message}");
            }
        }

        public (bool success, string message) UpdateAppointment(
            int appointmentId,
            int customerId,
            int userId,
            string title,
            string description,
            string location,
            string contact,
            string type,
            string url,
            DateTime startLocal,
            DateTime endLocal)
        {
            try
            {
                // Convert to UTC for storage
                DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal);
                DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                UPDATE appointment 
                SET customerId = @customerId,
                    userId = @userId,
                    title = @title,
                    description = @description,
                    location = @location,
                    contact = @contact,
                    type = @type,
                    url = @url,
                    start = @start,
                    end = @end,
                    lastUpdate = @lastUpdate,
                    lastUpdateBy = @lastUpdateBy
                WHERE appointmentId = @appointmentId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        DateTime now = DateTime.UtcNow;
                        command.Parameters.AddWithValue("@appointmentId", appointmentId);
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@location", location);
                        command.Parameters.AddWithValue("@contact", contact);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@url", url);
                        command.Parameters.AddWithValue("@start", startUtc);
                        command.Parameters.AddWithValue("@end", endUtc);
                        command.Parameters.AddWithValue("@lastUpdate", now);
                        command.Parameters.AddWithValue("@lastUpdateBy", currentUser);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0
                            ? (true, "Appointment updated successfully!")
                            : (false, "No appointment was updated.");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error updating appointment: {ex.Message}");
            }
        }


    }
}