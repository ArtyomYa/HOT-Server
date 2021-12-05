using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; //
using System.Net.Sockets;//
using System.Data.SqlClient;

namespace Project_HOT_Server
{
    class Program
    {
        public static string CheckRequest(string str) //func to check what the client requested 
        {
            string sign = str.Substring(0,2);
            str = str.Substring(2);
            if (sign == "00")
                str = UpdateUser(str);
            else if(sign == "01")
                str = CreateUser(str);
            else if(sign == "02")
                str = ShowAllUsers(str);
            else if(sign == "03")
                str = DeleteUser(str);
            else if(sign == "04")
                str = UpdateEmploye(str);
            else if(sign == "05")
                str = CreateEmploye(str);
            else if(sign == "06")
                str = ShowAllEmployees();
            else if(sign == "07")
                str = DeleteEmploye(str);
            else if (sign == "08") // check user name and password and type 
                str = CheckLogin(str);
            return str;
        }
        public static string DeleteEmploye(string str)
        {
            try
            {
                string Id = str;
                int n;
                SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = "delete from Employees where Id='" + Id + "';";
                n = mySqlCommand.ExecuteNonQuery();
                if (n == 0)
                {
                    mySqlConnection.Close();
                    return "Employee not found";
                }
                else
                {
                    mySqlConnection.Close();
                    return "Employee ID " + Id + " deleted";
                }
                
            }
            catch 
            {
                return "First need to delete User";
            }
        }
        public static string ShowAllEmployees()//func to show all employees
        {
            List<string> UsersList = new List<string>();
            SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");//kesher to database
            SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = "Select * from Employees order by Fname;"; //command sql for select
            mySqlConnection.Open(); //open connection
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();// open mySqlDataReader
            string str = "";
            // read row from table
            while (mySqlDataReader.Read())
            {
                str += mySqlDataReader["Id"] + "~";
                str += mySqlDataReader["Fname"] + "~";
                str += mySqlDataReader["Lname"] + "~";
                str += mySqlDataReader["Phone"] + "~";
                str += mySqlDataReader["Address"] + "~";
                str += mySqlDataReader["Employee_type"] + "~";
                UsersList.Add(str);
                str = "";
            }
            mySqlDataReader.Close();
            mySqlConnection.Close();
            str = String.Join("$", UsersList.ToArray()); //list convert to string
            Console.WriteLine(str);
            return str;
        }
        public static string CreateEmploye(string str)//func to create(add) new employee
        {
            string[] temp = str.Split('$');
            try
            {
                string Id = temp[0];
                string Fname = temp[1];
                string Lname = temp[2];
                string Phone = temp[3];
                string Address = temp[4];
                string Employee_type = temp[5];
                SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = "INSERT INTO Employees(Id,Fname,Lname,Phone,Address,Employee_type) VALUES('" + Id + "','" + Fname + "','" + Lname + "','" + Phone + "','" + Address + "','" + Employee_type + "');";
                mySqlCommand.ExecuteNonQuery();
                Console.WriteLine("Employee " + Fname + " " + Lname + " created");
                mySqlConnection.Close();
                return "Employee " + Fname + " " + Lname + " created";
            }
            catch
            {
                return "This ID already exist";
            }
        }
        public static string UpdateEmploye(string str)//func to update selected employee
        {
            string[] temp = str.Split('$');
            try
            {
                string Id = temp[0];
                string Fname = temp[1];
                string Lname = temp[2];
                string Phone = temp[3];
                string Address = temp[4];
                string Employee_type = temp[5];
                int n;
                SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = "update Employees set Fname='" + Fname + "',Lname='" + Lname + "',Phone='" + Phone + "',Address='" + Address + "',Employee_type='" + Employee_type + "' where Id='" + Id + "';";
                n = mySqlCommand.ExecuteNonQuery();
                if (n == 0)
                {
                    Console.WriteLine("Employee not found");
                    str = "Employee not found";
                }
                else
                {
                    Console.WriteLine("Update  " + n.ToString() + " Employee");
                    str = "Update  " + n.ToString() + " Employee";
                }
                mySqlConnection.Close();
            }
            catch (Exception err)
            {
                str = err.ToString();
            }
            return str;
        }
        public static string ShowAllUsers(string str)//func to show all users
        {
            List<string> UsersList = new List<string>();
            SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");//kesher to database
            SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = "SELECT * FROM Login ORDER BY Username;"; //command sql for select
            mySqlConnection.Open(); //open connection
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();// open mySqlDataReader
            str = "";
            // read row from table
            while (mySqlDataReader.Read())
            {
                str += mySqlDataReader["Id"] + "~";
                str += mySqlDataReader["Username"] + "~";
                str += mySqlDataReader["Password"] + "~";
                str += mySqlDataReader["Status"] + "~";
                UsersList.Add(str);
                str = "";
            }
            mySqlDataReader.Close();
            mySqlConnection.Close();
            str = String.Join("$", UsersList.ToArray()); //list convert to string
            return str;
        }
        public static string DeleteUser(string str)// func to delete selected user
        {
            try
            {
                string username = str;
                int n;
                SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = "DELETE FROM Login WHERE Username ='" + username + "';";
                n = mySqlCommand.ExecuteNonQuery();
                if (n == 0)
                {
                    mySqlConnection.Close();
                    return "Username not found";
                }
                else
                {
                    mySqlConnection.Close();
                    return "Username " + username + " deleted";
                }
            }
            catch (Exception err)
            {
                return err.ToString();
            }
        }
        public static string CreateUser(string str) //func to create(add) new user
        {
            string[] temp = str.Split('$');
            try
            {
                string Id = temp[0];
                string Username = temp[1];
                string Password = temp[2];
                string Status = temp[3];
                
                SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = "INSERT INTO Login (Id,Username,Password,Status) VALUES ('" + Id + "','" + Username + "','" + Password + "','" + Status + "');";
                mySqlCommand.ExecuteNonQuery();
                Console.WriteLine("User " + Username + " created");
                mySqlConnection.Close();
                return "User " + Username + " created";
            }
            catch 
            {
                return "This Username already exist/Wrong ID";
            }
        }
        public static string UpdateUser(string str) //func to update selected user
        {
            string[] temp = str.Split('$');
            try
            {
                string Id = temp[0];
                string Username = temp[1];
                string Password = temp[2];
                string Status = temp[3];

                int n;
                SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");
                SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = "UPDATE Login SET Username='" + Username + "',Password='" + Password + "',Status='" + Status + "' WHERE Id='" + Id + "';";
                n = mySqlCommand.ExecuteNonQuery();
                if (n == 0)
                {
                    mySqlConnection.Close();
                    return "Username not found";
                }
                else
                {
                    mySqlConnection.Close();
                    return "Username " + Username + " updated";
                }
                
            }
            catch (Exception err)
            {
                return err.ToString();
            }
        }

        public static string CheckLogin(string str) //func to check if username and password correct and status (active/not active)
        {
            string[] temp = str.Split('$');
            string username = temp[0];
            string password = temp[1];

            SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");//kesher to database
            SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = "SELECT Status FROM Login WHERE Username ='" + username + "';"; //command sql for select
            mySqlConnection.Open(); //open connection
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();// open mySqlDataReader
            string status = "";
            while (mySqlDataReader.Read())
            {
                status = mySqlDataReader["status"].ToString();  // save the status string
            }
            mySqlDataReader.Close();
            mySqlConnection.Close();
            
            if (status == "") //checking if username excist
            {
                Console.WriteLine("Username not found");
                str = "Username not found";
            }
            else if (status.Trim() == "Inactive")
            {
                Console.WriteLine("This username is inactive");
                return "This username is inactive";
            }
            else
            {
                mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");//kesher to database
                mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = "Select Password from Login where Username='" + username + "';"; //command sql for select
                mySqlConnection.Open(); //open connection
                mySqlDataReader = mySqlCommand.ExecuteReader();// open mySqlDataReader
                string tmp = "";
                while (mySqlDataReader.Read())
                {
                    tmp = mySqlDataReader["Password"].ToString(); //save the password string
                }
                mySqlDataReader.Close();
                mySqlConnection.Close();
                if (tmp.Trim().Equals(password)) //checking if the password is correct
                {
                    str = CheckLoginType(username);
                }
                else
                {
                    str = "incorrect password";
                }
            }
            Console.WriteLine(str);
            return str;
        }

        public static string CheckLoginType(string username) //func to return the employee\login type (admin\sales\service)
        {
            SqlConnection mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");//kesher to database
            SqlCommand mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = "Select Id from Login where Username='" + username + "';"; //command sql for select
            mySqlConnection.Open(); //open connection
            SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();// open mySqlDataReader
            string id = "";
            while (mySqlDataReader.Read())
            {
                id = mySqlDataReader["Id"].ToString(); //save the id string
            }
            mySqlDataReader.Close();
            mySqlConnection.Close();
            string login_type = "";
            mySqlConnection = new SqlConnection("server=(local)\\SQLEXPRESS;database=HOT;Integrated Security=SSPI;");//kesher to database
            mySqlCommand = mySqlConnection.CreateCommand();
            mySqlCommand.CommandText = "Select Employee_type from Employees where Id='" + id + "';"; //command sql for select
            mySqlConnection.Open(); //open connection
            mySqlDataReader = mySqlCommand.ExecuteReader();// open mySqlDataReader
            while (mySqlDataReader.Read())
            {
                login_type = mySqlDataReader["Employee_type"].ToString(); //save the employee type string
            }
            mySqlDataReader.Close();
            mySqlConnection.Close();

            return login_type.Trim();
        }

        static void Main(string[] args)
        {
            try
            {
                Int32 port = 13000; // number port
                IPAddress localAddr = IPAddress.Parse("127.0.0.1"); //ip address
                TcpListener server = new TcpListener(localAddr, port);
                server.Start();

                byte[] bytes = new byte[256];
                string data = null;
                while (true)
                {
                    Console.WriteLine("waiting to connection...");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("connected ....");
                    Console.WriteLine(client.Client.RemoteEndPoint.ToString());
                    Console.WriteLine(DateTime.Now.ToString());

                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;
                    i = stream.Read(bytes, 0, bytes.Length);
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine(data);
                    string data1 = CheckRequest(data);
                    byte[] msg = Encoding.ASCII.GetBytes(data1);
                    stream.Write(msg, 0, msg.Length);
                    client.Close();
                }

            }
            catch (Exception arr)
            {
                Console.WriteLine(arr.Message);
            }
        }

        
    }
}
