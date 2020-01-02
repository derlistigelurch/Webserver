using System;
using System.Collections.Generic;
using Npgsql;

namespace Webserver.Database
{
    /// <summary>
    /// Class to access the PostgresSQL Database.
    /// </summary>
    public class DatabaseConnection
    {
        /// <summary>
        /// Test Connection to Database, only needed for testing
        /// </summary>
        /// <returns></returns>
        public string TestConnection()
        {
            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    return connection.ServerVersion;
                }
                catch (NpgsqlException npgsqlException)
                {
                    Console.WriteLine(npgsqlException.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Select temperature data from a given range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="until"></param>
        /// <returns>A Dictionary with Date and Temperature data.</returns>
        public Dictionary<string, double> SelectTemperatureRange(DateTime from, DateTime until)
        {
            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    var command = new NpgsqlCommand(
                        "SELECT time, temp FROM temperature WHERE TO_DATE(time, 'DD/MM/YY') BETWEEN TO_DATE(@from, 'DD/MM/YY') AND TO_DATE(@until, 'DD/MM/YY')",
                        connection);
                    command.Parameters.AddWithValue("@from", from.ToString());
                    command.Parameters.AddWithValue("@until", until.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        var dictionary = new Dictionary<string, double>();
                        while (reader.Read())
                        {
                            dictionary.Add(reader.GetString(0), reader.GetDouble(1));
                        }

                        return dictionary;
                    }
                }
                catch (NpgsqlException npgsqlException)
                {
                    Console.WriteLine(npgsqlException.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Select temperature data from an exact date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>A Dictionary with Date and Temperature data.</returns>
        public Dictionary<string, double> SelectTemperatureExact(DateTime date)
        {
            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    var command = new NpgsqlCommand(
                        "SELECT time, temp FROM temperature WHERE TO_DATE(time, 'DD/MM/YY')=TO_DATE(@date, 'DD/MM/YY')",
                        connection);
                    command.Parameters.AddWithValue("@date", date.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        var dictionary = new Dictionary<string, double>();
                        while (reader.Read())
                        {
                            dictionary.Add(reader.GetString(0), reader.GetDouble(1));
                        }

                        return dictionary;
                    }
                }
                catch (NpgsqlException npgsqlException)
                {
                    Console.WriteLine(npgsqlException.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Insert new temperature data.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="temperature"></param>
        public void InsertTemperature(DateTime dateTime, double temperature)
        {
            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    var command = new NpgsqlCommand(
                        "INSERT INTO temperature (time, temp) VALUES (@time, @temp)", connection);
                    // Parameter setzen
                    command.Parameters.AddWithValue("@time", dateTime.ToString());
                    command.Parameters.AddWithValue("@temp", Math.Round(temperature, 1));
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                catch (NpgsqlException npgsqlException)
                {
                    Console.WriteLine(npgsqlException.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /*
        /// <summary>
        /// Insert 10000 rows for testing
        /// </summary>
        public void InsertTestData()
        {
            Random random = new Random();
            for (int j = -10; j < 0; j++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    DateTime dateTime = DateTime.Now
                        .AddYears(j)
                        .AddDays(i / 5)
                        .AddHours(i % 11);
                    double temp = Math.Sin(Math.PI * i) + (random.NextDouble() - 0.5) * 100;
                    this.InsertTemperature(dateTime, temp);
                }
            }

            Console.WriteLine("fertig");
        }*/

        /// <summary>
        /// Reads data from Temperaturesensor every minute
        /// </summary>
        public void ReadSensorData()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);
            var connection = new DatabaseConnection();
            var random = new Random();

            var timer = new System.Threading.Timer(
                (e) =>
                {
                    connection.InsertTemperature(DateTime.Now, random.NextDouble() - 0.5 * 100);
                    // Console.WriteLine("insert");
                },
                null, startTimeSpan, periodTimeSpan);
        }

        /// <summary>
        /// Returns connection string to connect to database
        /// </summary>
        public string ConnectionString { get; } = "Server=127.0.0.1;" +
                                                  "Port=5432;" +
                                                  "Database=postgres;" +
                                                  "User Id=temp;" +
                                                  "Password=temp;";
    }
}