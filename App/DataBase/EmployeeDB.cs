using App.Model;

using Microsoft.Data.SqlClient;

namespace App.DataBase
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    class EmployeeDB
    {
        private const string DatabaseName = "EmployeeDB";
        private const string TableName = "Employees";

        private const string CreateDatabaseQuery = "CREATE DATABASE EmployeeDB";
        private const string CreateTableQuery = @"
            CREATE TABLE EmployeeDB.dbo.Employees (
                EmployeeID INT PRIMARY KEY IDENTITY(1,1),
                FirstName NVARCHAR(50),
                LastName NVARCHAR(50),
                Email NVARCHAR(100) UNIQUE,
                DateOfBirth DATE,
                Salary DECIMAL(18, 2)
            )";

        public static void Make()
        {
            if (!CheckDatabaseAndTableExistence())
            {
                CreateDatabaseAndTable();
            }
        }

        public static async Task AddEmployeeAsync(Employee employee)
        {
            const string insertQuery = @"
            INSERT INTO EmployeeDB.dbo.Employees (FirstName, LastName, Email, DateOfBirth, Salary) 
            VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Salary)";

            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_DB))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                        command.Parameters.AddWithValue("@Salary", employee.Salary);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при добавлении сотрудника: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при добавлении сотрудника в базу данных.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
        }

        public static async Task<List<Employee>> GetAllEmployeesAsync()
        {
            List<Employee> employees = new List<Employee>();
            const string selectQuery = "SELECT * FROM EmployeeDB.dbo.Employees";

            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_DB))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Employee employee = new Employee
                                {
                                    EmployeeID = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    DateOfBirth = reader.GetDateTime(4),
                                    Salary = reader.GetDecimal(5)
                                };
                                employees.Add(employee);
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при получении списка сотрудников: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при получении списка сотрудников из базы данных.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
            return employees;
        }

        public static async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            Employee employee = null;
            const string selectQuery = "SELECT * FROM EmployeeDB.dbo.Employees WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_DB))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync()) 
                            {
                                employee = new Employee
                                {
                                    EmployeeID = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    DateOfBirth = reader.GetDateTime(4),
                                    Salary = reader.GetDecimal(5)
                                };
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при получении списка сотрудников: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при получении списка сотрудников из базы данных.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
            return employee;
        }

        public static async Task UpdateEmployeeAsync(Employee employee)
        {
            const string updateQuery = @"
            UPDATE EmployeeDB.dbo.Employees 
            SET FirstName = @FirstName, LastName = @LastName, Email = @Email, DateOfBirth = @DateOfBirth, Salary = @Salary 
            WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_DB))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                        command.Parameters.AddWithValue("@Salary", employee.Salary);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при обновлении сотрудника: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при обновлении сотрудника в базе данных.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
        }

        public static async Task DeleteEmployeeAsync(int employeeId)
        {
            const string deleteQuery = "DELETE FROM EmployeeDB.dbo.Employees WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_DB))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при удалении сотрудника: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при удалении сотрудника из базы данных.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
        }

        private static bool CheckDatabaseAndTableExistence()
        {
            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_SERVER))
            {
                try
                {
                    connection.Open();

                    string checkDatabaseQuery = $"SELECT db_id('{DatabaseName}')";
                    using (SqlCommand command = new SqlCommand(checkDatabaseQuery, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            Console.WriteLine($"База данных '{DatabaseName}' существует.");

                            string checkTableQuery = $"SELECT OBJECT_ID('{DatabaseName}.dbo.{TableName}')";
                            using (SqlCommand commandTable = new SqlCommand(checkTableQuery, connection))
                            {
                                object tableResult = commandTable.ExecuteScalar();
                                if (tableResult != DBNull.Value)
                                {
                                    Console.WriteLine($"Таблица '{TableName}' существует в базе данных '{DatabaseName}'.");
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine($"Таблица '{TableName}' не существует в базе данных '{DatabaseName}'.");
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"База данных '{DatabaseName}' не существует.");
                            return false;
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при проверке существования базы данных и таблицы: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при проверке существования базы данных и таблицы.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
        }

        private static void CreateDatabaseAndTable()
        {
            using (SqlConnection connection = new SqlConnection(Settings.CONNECTION_SERVER))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(CreateDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("База данных 'EmployeeDB' успешно создана.");
                    }

                    string newConnectionString = Settings.CONNECTION_DB;

                    using (SqlCommand command = new SqlCommand(CreateTableQuery, new SqlConnection(newConnectionString)))
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Таблица 'Employees' успешно создана.");
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Ошибка при создании базы данных или таблицы: " + sqlEx.Message);
                    throw new DatabaseException("Ошибка при создании базы данных или таблицы.", sqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    throw;
                }
            }
        }
    }
}