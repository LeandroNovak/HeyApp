using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyApp
{
    public static class Common
    {
        // Inserir strings de conexão aqui

        public static bool IsValidEmail(string email)
        {
            const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"+ @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

            if (email == null)
                return false;

            email = email.TrimEnd(' ');
            return Regex.IsMatch(email, MatchEmailPattern);
        }

        public static void SendConfirmationEmail(string name, string email, string code)
        {
            var body = "<p>Hello <strong>" + name + "</strong>,</p><p>Your verification code is:</p><h1><strong>" + code + "</strong></h1>";
            //var body = "<p>Olá <strong>" + name + "</strong>,</p><p>Seu código de confirmação é:</p><h1><strong>" + code + "</strong></h1>";

            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 100000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(YOUR_EMAIL, YOUR_PASSWORD)
            };

            //MailMessage mm = new MailMessage(YOUR_EMAIL, email, "Código de confirmação", body)
            MailMessage mm = new MailMessage(YOUR_EMAIL, email, "Verification Code", body)
            {
                BodyEncoding = UTF8Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                IsBodyHtml = true
            };

            client.Send(mm);
        }

        public static string GenerateCode()
        {
            Random rand = new Random();
            // Generate random number with 4 digits
            var code = rand.Next(1000, 9999).ToString();
            Console.WriteLine("Random: {0}", code);
            return code;
        }

        public static bool UserExists(string email)
        {
            NpgsqlConnection connection;
            // Connect to database
            try
            {
                connection = new NpgsqlConnection(Common.YOUR_CONNECTION_STRING);
                connection.Open();
                Console.WriteLine("Opened database connection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            // Executes a command
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM public.users WHERE email='" + email + "'";

            try
            {
                if (command.ExecuteScalar() != null)
                {
                    connection.Close();
                    return true;
                }
                else
                {
                    throw new Exception("user not found");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            connection.Close();
            return false;
        }

        public static bool GetUserInformations(string email)
        {
            NpgsqlConnection connection;
            // Connect to database
            try
            {
                connection = new NpgsqlConnection(Common.YOUR_CONNECTION_STRING);
                connection.Open();
                Console.WriteLine("Opened database connection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            // Executes a command
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.users WHERE email='" + email + "'";

            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Application.Current.Properties["id"] = reader["id"].ToString();
                    Application.Current.Properties["name"] = reader["name"].ToString();
                    Application.Current.Properties["email"] = reader["email"].ToString();

                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            connection.Close();
            return false;
        }

        public static bool InsertUserOnDatabase(string name, string email)
        {
            NpgsqlConnection connection;
            // Connect to database
            try
            {
                connection = new NpgsqlConnection(Common.YOUR_CONNECTION_STRING);
                connection.Open();
                Console.WriteLine("Opened database connection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            // Executes a command
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO PUBLIC.USERS (name, email) VALUES ('" + name + "', '" + email + "');";
            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                connection.Close();
                return false;
            }
        }

        public static bool InsertPostOnDatabase(string title, string description, string image)
        {
            string userId = Application.Current.Properties["id"].ToString();

            NpgsqlConnection connection;

            // Connect to database
            try
            {
                connection = new NpgsqlConnection(Common.YOUR_CONNECTION_STRING);
                connection.Open();
                Console.WriteLine("Opened database connection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            // Executes a command
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO PUBLIC.POSTS (title, description, image, status, location, userid) VALUES ('" 
                + title + "', '" + description + "', '" + image + "', '" + true + "', '" + "" + "', '" + userId + "');";
            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                connection.Close();
                return false;
            }
        }

        public static Stack<Post> GetPostsFromDatabase()
        {
            NpgsqlConnection connection;
            Stack<Post> postList = new Stack<Post>();

            // Connect to database
            try
            {
                connection = new NpgsqlConnection(Common.YOUR_CONNECTION_STRING);
                connection.Open();
                Console.WriteLine("Opened database connection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            // Executes a command
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM POSTS";
            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Post post = new Post
                    {
                        Title = reader["title"].ToString(),
                        Description = reader["description"].ToString(),
                        ImageBase64 = reader["image"].ToString()
                    };

                    postList.Push(post);
                }

                connection.Close();
                return postList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            connection.Close();
            return null;
        }
    }

    public class ExpandableEditor : Editor
    {
        public ExpandableEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~ExpandableEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
