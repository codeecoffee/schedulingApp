using MySqlConnector;
using System;
using System.Data;
using System.Configuration;


namespace schedulingApp
{

    public class DatabaseHelper
    {
        private readonly string connectionString;
        private static string _currentUser ="system";

        public static string CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        public static void SetCurrentUser(string username)
        {
            _currentUser = username;
        }

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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

        //[OK] Method to validate user credentials - used in the Login Screen
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

        //!!TODO: FIX THIS 
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
                command.Parameters.AddWithValue("@createdBy", _currentUser);
                command.Parameters.AddWithValue("@lastUpdate", now);
                command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);

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
                command.Parameters.AddWithValue("@createdBy", _currentUser);
                command.Parameters.AddWithValue("@lastUpdate", now);
                command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);

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
                command.Parameters.AddWithValue("@createdBy", _currentUser);
                command.Parameters.AddWithValue("@lastUpdate", now);
                command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        //OK
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
                            //New stuff
                            //var countryId = InsertCountry(connection,country);
                            //var cityId = InsertCity(connection, city, countryId);
                            //var addressId = InsertAddress(connection, address1, address2, cityId, postalCode, phone);

                            //WORKING
                            // Insert country first and get the ID
                            string countryQuery = @"INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) 
                                          VALUES (@country, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                                          SELECT LAST_INSERT_ID();";

                            int countryId;
                            using (MySqlCommand cmd = new MySqlCommand(countryQuery, connection, transaction))
                            {
                                DateTime now = DateTime.Now;
                                cmd.Parameters.AddWithValue("@country", country);
                                cmd.Parameters.AddWithValue("@createDate", now);
                                cmd.Parameters.AddWithValue("@createdBy", _currentUser);
                                cmd.Parameters.AddWithValue("@lastUpdate", now);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                                countryId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            //MessageBox.Show("Country added");

                            //WORKING
                            // Insert city and get the ID
                            string cityQuery = @"INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) 
                                       VALUES (@city, @countryId, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                                       SELECT LAST_INSERT_ID();";

                            int cityId;


                            using (MySqlCommand cmd = new MySqlCommand(cityQuery, connection, transaction))
                            {
                                DateTime now = DateTime.Now;
                                cmd.Parameters.AddWithValue("@city", city);
                                cmd.Parameters.AddWithValue("@countryId", countryId);
                                cmd.Parameters.AddWithValue("@createDate", now);
                                cmd.Parameters.AddWithValue("@createdBy", _currentUser);
                                cmd.Parameters.AddWithValue("@lastUpdate", now);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                                cityId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            //MessageBox.Show("City added");

                            // Insert address and get the ID
                            string addressQuery = @"INSERT INTO address 
                            (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)
                            VALUES 
                            (@address, @address2, @cityId, @postalCode, @phone, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                            SELECT LAST_INSERT_ID();";

                            int addressId;
                            using (MySqlCommand cmd = new MySqlCommand(addressQuery, connection, transaction))
                            {
                                DateTime now = DateTime.Now;
                                cmd.Parameters.AddWithValue("@address", address1);
                                cmd.Parameters.AddWithValue("@address2", string.IsNullOrEmpty(address2) ? (object)DBNull.Value : address2);
                                cmd.Parameters.AddWithValue("@cityId", cityId);
                                cmd.Parameters.AddWithValue("@postalCode", string.IsNullOrEmpty(postalCode) ? (object)DBNull.Value : postalCode);
                                cmd.Parameters.AddWithValue("@phone", phone);
                                cmd.Parameters.AddWithValue("@createDate", now);
                                cmd.Parameters.AddWithValue("@createdBy", _currentUser);
                                cmd.Parameters.AddWithValue("@lastUpdate", now);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                                addressId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            MessageBox.Show("Address added");

                            // Finally insert customer
                            string customerQuery = @"INSERT INTO customer 
                        (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES 
                        (@customerName, @addressId, 1, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                        SELECT LAST_INSERT_ID();";

                            using (MySqlCommand cmd = new MySqlCommand(customerQuery, connection, transaction))
                            {
                                DateTime now = DateTime.Now;
                                cmd.Parameters.AddWithValue("@customerName", customerName);
                                cmd.Parameters.AddWithValue("@addressId", addressId);
                                cmd.Parameters.AddWithValue("@createDate", now);
                                cmd.Parameters.AddWithValue("@createdBy", _currentUser);
                                cmd.Parameters.AddWithValue("@lastUpdate", now);
                                cmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            //MessageBox.Show("Customer added");

                            return (true, "Customer added successfully!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return (false, $"Transaction error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Database connection error: {ex.Message}");
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
        public (bool success, string message) DeleteCustomer(int customerId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = "DELETE FROM customer WHERE customerId = @customerId";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Transaction = transaction;
                            command.Parameters.AddWithValue("@customerId", customerId);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                return (true, "Customer deleted successfully.");
                            }
                            else
                            {
                                transaction.Rollback();
                                return (false, "Customer not found.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return (false, $"Error deleting customer: {ex.Message}");
                    }
                }
            }
        }
        //Ok
        public (bool success, string message) UpdateCustomer(
    int customerId,
    string customerName,
    string address,
    string address2,
    string city,
    string country,
    string postalCode,
    string phone)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, get the addressId for this customer
                        string getAddressIdQuery = "SELECT addressId FROM customer WHERE customerId = @customerId";
                        int addressId;

                        using (MySqlCommand cmd = new MySqlCommand(getAddressIdQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@customerId", customerId);
                            var result = cmd.ExecuteScalar();
                            addressId = result != null ? Convert.ToInt32(result) : -1;
                        }

                        if (addressId == -1)
                        {
                            transaction.Rollback();
                            return (false, "Customer not found.");
                        }

                        // Handle country first
                        int countryId;
                        string checkCountryQuery = "SELECT countryId FROM country WHERE country = @country";
                        using (MySqlCommand cmd = new MySqlCommand(checkCountryQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@country", country);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                countryId = Convert.ToInt32(result);
                            }
                            else
                            {
                                // Insert new country if it doesn't exist
                                string insertCountryQuery = @"INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) 
                                                    VALUES (@country, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                                                    SELECT LAST_INSERT_ID();";
                                using (MySqlCommand insertCmd = new MySqlCommand(insertCountryQuery, connection, transaction))
                                {
                                    DateTime now = DateTime.Now;
                                    insertCmd.Parameters.AddWithValue("@country", country);
                                    insertCmd.Parameters.AddWithValue("@createDate", now);
                                    insertCmd.Parameters.AddWithValue("@createdBy", _currentUser);
                                    insertCmd.Parameters.AddWithValue("@lastUpdate", now);
                                    insertCmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                                    countryId = Convert.ToInt32(insertCmd.ExecuteScalar());
                                }
                            }
                        }

                        // Handle city
                        int cityId;
                        string checkCityQuery = "SELECT cityId FROM city WHERE city = @city AND countryId = @countryId";
                        using (MySqlCommand cmd = new MySqlCommand(checkCityQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@city", city);
                            cmd.Parameters.AddWithValue("@countryId", countryId);
                            var result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                cityId = Convert.ToInt32(result);
                            }
                            else
                            {
                                // Insert new city if it doesn't exist
                                string insertCityQuery = @"INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) 
                                                 VALUES (@city, @countryId, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);
                                                 SELECT LAST_INSERT_ID();";
                                using (MySqlCommand insertCmd = new MySqlCommand(insertCityQuery, connection, transaction))
                                {
                                    DateTime now = DateTime.Now;
                                    insertCmd.Parameters.AddWithValue("@city", city);
                                    insertCmd.Parameters.AddWithValue("@countryId", countryId);
                                    insertCmd.Parameters.AddWithValue("@createDate", now);
                                    insertCmd.Parameters.AddWithValue("@createdBy", _currentUser);
                                    insertCmd.Parameters.AddWithValue("@lastUpdate", now);
                                    insertCmd.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                                    cityId = Convert.ToInt32(insertCmd.ExecuteScalar());
                                }
                            }
                        }

                        // Update address table with new cityId
                        string updateAddressQuery = @"UPDATE address 
                    SET address = @address,
                        address2 = @address2,
                        cityId = @cityId,
                        postalCode = @postalCode,
                        phone = @phone,
                        lastUpdate = @lastUpdate,
                        lastUpdateBy = @lastUpdateBy
                    WHERE addressId = @addressId";

                        using (MySqlCommand command = new MySqlCommand(updateAddressQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@addressId", addressId);
                            command.Parameters.AddWithValue("@address", address);
                            command.Parameters.AddWithValue("@address2", string.IsNullOrEmpty(address2) ? (object)DBNull.Value : address2);
                            command.Parameters.AddWithValue("@cityId", cityId);
                            command.Parameters.AddWithValue("@postalCode", string.IsNullOrEmpty(postalCode) ? (object)DBNull.Value : postalCode);
                            command.Parameters.AddWithValue("@phone", phone);
                            command.Parameters.AddWithValue("@lastUpdate", DateTime.Now);
                            command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                            command.ExecuteNonQuery();
                        }

                        // Update customer table
                        string updateCustomerQuery = @"UPDATE customer 
                    SET customerName = @customerName,
                        lastUpdate = @lastUpdate,
                        lastUpdateBy = @lastUpdateBy
                    WHERE customerId = @customerId";

                        using (MySqlCommand command = new MySqlCommand(updateCustomerQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@customerId", customerId);
                            command.Parameters.AddWithValue("@customerName", customerName);
                            command.Parameters.AddWithValue("@lastUpdate", DateTime.Now);
                            command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                return (true, "Customer updated successfully.");
                            }
                            else
                            {
                                transaction.Rollback();
                                return (false, "Customer not found.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return (false, $"Error updating customer: {ex.Message}");
                    }
                }
            }
        }
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
                        command.Parameters.AddWithValue("@createdBy", _currentUser);
                        command.Parameters.AddWithValue("@lastUpdate", now);
                        command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);

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
                        command.Parameters.AddWithValue("@lastUpdateBy", _currentUser);

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