using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CoreTwitter.Classes;
using Microsoft.AspNetCore.Mvc;

namespace CoreTwitter.Controllers
{
    public class AzureSaveController : Controller
    {
        //conection to sqlserver and id list
        public List<long> IdsList = new List<long>();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public void SaveToAzure(long id, string url, int retweet)
        {
            //check for id if already exists
            if(!IdsList.Contains(id))
            {
                try
                {
                    builder.DataSource = "";
                    builder.UserID = "";
                    builder.Password = "";
                    builder.InitialCatalog = "";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        //query for inserting data
                        String query = "INSERT INTO twit (id,url,retweet) VALUES (@id,@url,@retweet)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@url", url);
                            command.Parameters.AddWithValue("@retweet", retweet);

                            connection.Open();
                            int result = command.ExecuteNonQuery();

                            // Check Error
                            if (result < 0)
                                Console.WriteLine("Error inserting data into Database!");
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            
        }
        //getting data from azure database
        public void GetFromAzure()
        {
            try
            {
                builder.DataSource = "";
                builder.UserID = "";
                builder.Password = "";
                builder.InitialCatalog = "";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select id from twit", connection);
                    // int result = command.ExecuteNonQuery();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            while (reader.Read())
                            {
                                long s = reader.GetInt64(0);
                                IdsList.Add(s);
                                //Console.WriteLine("READER OUTPUT: " + String.Format("{0}", reader["id"]));
                                //Console.WriteLine("READER OUTPUT: " + s);
                            }
                            
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}