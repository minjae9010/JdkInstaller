using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace JavaInstaller.Model
{
    internal class DbDAO
    {
        private string connectionString = "Data Source=DB/JdkVersion.db;Version=3;";

        public DbDAO() { }

        public List<jdkInfo> SelectDataFromTable()
        {
            List<jdkInfo> results = new List<jdkInfo>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM mainTable;";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string product = reader.GetString(0);
                        string jdkVer = reader.GetString(1);
                        string version = reader.GetString(2);
                        string path = reader.GetString(3);

                        jdkInfo info = new jdkInfo
                        {
                            product = product,
                            jdkver = jdkVer,
                            version = version,
                            path = path
                        };

                        results.Add(info);
                    }
                }
            }

            return results;
        }

        public void InsertDataIntoTable(jdkInfo info)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO mainTable (Product, JDKVer, Version, Path) VALUES (@product, @jdkver, @version, @path);";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@product", info.product);
                command.Parameters.AddWithValue("@jdkver", info.jdkver);
                command.Parameters.AddWithValue("@version", info.version);
                command.Parameters.AddWithValue("@path", info.path);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteDataFromTable(jdkInfo info)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM mainTable WHERE Product = @product AND JDKVer = @jdkver;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@product", info.product);
                command.Parameters.AddWithValue("@jdkver", info.jdkver);

                command.ExecuteNonQuery();
            }
        }
    }
}
