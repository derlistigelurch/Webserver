using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using Npgsql;

namespace Webserver.Database
{
    public class DatabaseConnection
    {
        public void testConnection()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine(connection.PostgreSqlVersion);
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

        public Dictionary<string, double> SelectTemperatureRange(DateTime from, DateTime until)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    NpgsqlCommand command = new NpgsqlCommand(
                        "SELECT time, temp FROM temperature WHERE TO_DATE(time, 'DD/MM/YY') BETWEEN TO_DATE(@from, 'DD/MM/YY') AND TO_DATE(@until, 'DD/MM/YY')",
                        connection);
                    command.Parameters.AddWithValue("@from", from.ToString());
                    command.Parameters.AddWithValue("@until", until.ToString());
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        Dictionary<string, double> dictionary = new Dictionary<string, double>();
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

        public Dictionary<string, double> SelectTemperatureExact(DateTime date)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    NpgsqlCommand command = new NpgsqlCommand(
                        "SELECT time, temp FROM temperature WHERE TO_DATE(time, 'DD/MM/YY')=TO_DATE(@date, 'DD/MM/YY')",
                        connection);
                    command.Parameters.AddWithValue("@date", date.ToString());
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        Dictionary<string, double> dictionary = new Dictionary<string, double>();
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

        /*public Dictionary<string, double> SelectTemperatureAll()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    NpgsqlCommand command = new NpgsqlCommand(
                        "SELECT time, temp FROM temperature",
                        connection);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        Dictionary<string, double> dictionary = new Dictionary<string, double>();
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
        }*/

        private void InsertTemperature(DateTime dateTime, double temperature)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SQL Statement vorbereiten
                    NpgsqlCommand command = new NpgsqlCommand(
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

        /*public void InsertTestData()
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

        public void ReadSensorData()
        {
            TimeSpan startTimeSpan = TimeSpan.Zero;
            TimeSpan periodTimeSpan = TimeSpan.FromMinutes(1);
            DatabaseConnection connection = new DatabaseConnection();
            Random random = new Random();

            Timer timer = new System.Threading.Timer(
                (e) =>
                {
                    connection.InsertTemperature(DateTime.Now, random.NextDouble() - 0.5 * 100);
                    Console.WriteLine("insert");
                },
                null, startTimeSpan, periodTimeSpan);
        }

        private string ConnectionString { get; } = "Server=127.0.0.1;" +
                                                   "Port=5432;" +
                                                   "Database=postgres;" +
                                                   "User Id=temp;" +
                                                   "Password=temp;";
    }
}