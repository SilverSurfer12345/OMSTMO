using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace OrderManagement
{
    public static class DatabaseManager
    {
        private static string _connectionString;
        private static string _databasePath;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = GetConnectionString();
                }
                return _connectionString;
            }
        }

        public static string DatabasePath => _databasePath;

        static DatabaseManager()
        {
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            try
            {
                // Set the database path relative to the application's directory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                _databasePath = Path.Combine(baseDir, @"..\..\RM.mdf");
                _databasePath = Path.GetFullPath(_databasePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing database: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetConnectionString()
        {
            try
            {
                return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + _databasePath +
                       ";Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating connection string: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Fallback (should not be used)
                return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + _databasePath + ";Integrated Security=True;Connect Timeout=30";
            }
        }


        public static void ForceCheckpoint()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("CHECKPOINT", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error forcing checkpoint: " + ex.Message);
            }
        }

        public static void CloseConnections()
        {
            try
            {
                // Force garbage collection to release any lingering connections
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Force a second garbage collection to ensure all resources are released
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing database connections: " + ex.Message);
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT 1", con))
                    {
                        cmd.ExecuteScalar();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing query: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing non-query: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing scalar: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}