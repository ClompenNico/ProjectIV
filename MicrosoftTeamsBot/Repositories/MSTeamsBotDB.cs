using Microsoft.Azure.WebJobs;
using Microsoft.Bot.Builder.Dialogs;
using MicrosoftTeamsBot.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MicrosoftTeamsBot.Repositories
{
    public static class MSTeamsBotDB
    {
        private static readonly string CONNECTIONSTRING = "Server=tcp:[SERVERNAME].database.windows.net,1433;Initial Catalog=[DATABASE];Persist Security Info=False;User ID=[ID/USERNAME];Password=[PASSWORD];MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [FunctionName("GetUserById")]
        public static Guid GetUserById(string id)
        {
            try
            {
                Users users = new Users();
                using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = $"SELECT * FROM Users WHERE Id = @userId";
                        command.Parameters.AddWithValue("@userId", id);
                        command.CommandText = sql;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            users.VouchersId = new Guid(reader["VouchersId"].ToString());
                        }
                    }
                }
                return users.VouchersId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FunctionName("GetReceivedById")]
        public static bool GetReceivedById(Guid id)
        {
            try
            {
                Vouchers vouchers = new Vouchers();
                using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = $"SELECT * FROM Vouchers WHERE Id = @vouchersId";
                        command.Parameters.AddWithValue("@vouchersId", id);
                        command.CommandText = sql;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            vouchers.Received = Convert.ToBoolean(reader["Received"].ToString());
                        }
                    }
                }
                return vouchers.Received;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FunctionName("CreateUserData")]
        public static void CreateUserData(string id, string name)
        {
            try
            {
                Guid VouchersId = GetUserById(id);

                if (VouchersId == Guid.Empty)
                {
                    Users users = new Users();
                    Vouchers vouchers = new Vouchers();
                    using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand())
                        {
                            Guid vouchersId = Guid.NewGuid();
                            Random r = new Random();
                            int Received = r.Next(-1, 1);

                            command.Connection = connection;
                            string sql = "INSERT INTO Users VALUES(@Id, @Name, @Role, @VouchersId); INSERT INTO Vouchers VALUES(@VouchersId, @Received)";
                            command.Parameters.AddWithValue("@Id", id);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Role", false);
                            command.Parameters.AddWithValue("@VouchersId", vouchersId);
                            command.Parameters.AddWithValue("@Received", Received);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FunctionName("SaveSandwich")]
        public static void SaveSandwich(string UsersId, Sandwich sandwich, IDialogContext context)
        {
            try
            {
                //==========================================================================
                string greeting = $"New order in from {context.Activity.From.Name}:\r\n\r\n";
                string body = "";
                string ending = "\r\n\r\nThanks in advance!";

                string tomato = "";
                string salad = "";

                string mToppings = "";
                string mSauces = "";

                if (sandwich.tomatoes.ToString() == "Yes")
                {
                    tomato = ", tomato";
                }

                if (sandwich.salad.ToString() == "Yes")
                {
                    salad = "salad";
                }

                foreach(var topping in sandwich.topping)
                {
                    mToppings += (topping.ToString()).ToLower() + ", ";
                }

                foreach (var sauce in sandwich.sauce)
                {
                    mSauces += (sauce.ToString()).ToLower();
                }

                body = $"{sandwich.Amount} {sandwich.typeOfBun} sandwich(es) with {mToppings}{salad}{tomato} and {mSauces} as sauce.";

                string message = greeting + body + ending;

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("SEND_EMAIL_ADRES", "PASSWORD"),
                    EnableSsl = true
                };
                client.Send("SEND_EMAIL_ADRES", "RECEIVE_EMAIL_ADRES", "order of the day.", message);

                using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        Guid OrdersId = Guid.NewGuid();
                        DateTime DateNow = DateTime.Now;
                        string toppings = "";
                        string sauces = "";
                        //sandwich.salad = sandwich.salad - 1 + 1;
                        //sandwich.tomatoes = sandwich.tomatoes - 1 + 1;
                        command.Connection = connection;
                        string sql = "INSERT INTO Users_Orders VALUES(@UsersId, @OrdersId, @Amount, @Date); INSERT INTO FoodOrder VALUES(@OrdersId, @TypeOfBun, @Toppings, @Salad, @Tomatoes, @Sauce, @Date)";
                        command.Parameters.AddWithValue("@UsersId", UsersId);
                        command.Parameters.AddWithValue("@OrdersId", OrdersId);
                        command.Parameters.AddWithValue("@Amount", sandwich.Amount);
                        command.Parameters.AddWithValue("@Date", DateNow);
                        command.Parameters.AddWithValue("@TypeOfBun", sandwich.typeOfBun.ToString());
                        foreach(var topping in sandwich.topping)
                        {
                            toppings += topping.ToString() + ", ";
                        }
                        toppings = toppings.Remove(toppings.Length - 2);
                        command.Parameters.AddWithValue("@Toppings", toppings);
                        command.Parameters.AddWithValue("@Salad", sandwich.salad.ToString());
                        command.Parameters.AddWithValue("@Tomatoes", sandwich.tomatoes.ToString());
                        foreach (var sauce in sandwich.sauce)
                        {
                            sauces += sauce.ToString() + ", ";
                        }
                        sauces = sauces.Remove(sauces.Length - 2);
                        command.Parameters.AddWithValue("@Sauce", sauces);

                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FunctionName("GetOrdersByUserId")]
        public static List<Users_Orders> GetOrdersByUserId(string UsersId)
        {
            try
            {
                List<Users_Orders> users_Orders = new List<Users_Orders>();
                using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = $"SELECT * FROM Users_Orders WHERE UsersId = @userId";
                        command.Parameters.AddWithValue("@userId", UsersId);
                        command.CommandText = sql;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Users_Orders users_Order = new Users_Orders();
                            users_Order.Id = Convert.ToInt32(reader["Id"]);
                            users_Order.UsersId = reader["UsersId"].ToString();
                            users_Order.OrdersId = new Guid(reader["OrdersId"].ToString());
                            users_Order.Amount = Convert.ToInt32(reader["Amount"]);
                            users_Order.Date = Convert.ToDateTime(reader["Date"]);
                            users_Orders.Add(users_Order);
                        }
                    }
                }
                return users_Orders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FunctionName("GetOrdersByUserIdDate")]
        public static List<Users_Orders> GetOrdersByUserIdDate(string UsersId)
        {
            try
            {
                DateTime TodayDate = DateTime.Today;

                List<Users_Orders> users_Orders = new List<Users_Orders>();
                using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        string sql = $"SELECT * FROM Users_Orders WHERE UsersId = @userId";
                        command.Parameters.AddWithValue("@userId", UsersId);
                        command.Parameters.AddWithValue("@todayDate", TodayDate);
                        command.CommandText = sql;
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Users_Orders users_Order = new Users_Orders();
                            users_Order.Id = Convert.ToInt32(reader["Id"]);
                            users_Order.UsersId = reader["UsersId"].ToString();
                            users_Order.OrdersId = new Guid(reader["OrdersId"].ToString());
                            users_Order.Amount = Convert.ToInt32(reader["Amount"]);
                            users_Order.Date = Convert.ToDateTime(reader["Date"]);
                            
                            if (users_Order.Date.Date == DateTime.Today)
                            {
                                users_Orders.Add(users_Order);
                            }    
                        }
                    }
                }
                return users_Orders;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [FunctionName("GetSandwichByUserId")]
        public static List<FoodOrder> GetSandwichByUserId(string UserId)
        {
            List<Users_Orders> users_Orders = GetOrdersByUserIdDate(UserId);

            List<FoodOrder> foodOrders = new List<FoodOrder>();

            foreach (var user_Orders in users_Orders)
            {
                try
                {
                    FoodOrder foodOrder = new FoodOrder();
                    using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            string sql = $"SELECT * FROM FoodOrder WHERE Id = @FoodId";
                            command.Parameters.AddWithValue("@FoodId", user_Orders.OrdersId);
                            command.CommandText = sql;
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                foodOrder.Id = new Guid(reader["Id"].ToString());
                                foodOrder.TypeOfBun = reader["TypeOfBun"].ToString();
                                foodOrder.Toppings = reader["Toppings"].ToString();
                                foodOrder.Salad = reader["Salad"].ToString();
                                foodOrder.Tomato = reader["Tomato"].ToString();
                                foodOrder.Sauce = reader["Sauce"].ToString();
                                foodOrder.Date = Convert.ToDateTime(reader["Date"]);
                                foodOrder.Amount = user_Orders.Amount;
                                foodOrders.Add(foodOrder);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return foodOrders;
        }
    }
}