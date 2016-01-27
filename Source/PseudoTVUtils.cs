using System;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using NLog;
using PseudoTV_Manager.Enum;
using PseudoTV_Manager.Forms;
using PseudoTV_Manager.Properties;

namespace PseudoTV_Manager
{
    public static class PseudoTvUtils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static KodiVersion GetKodiVersion(string videoDbFileName)
        {
            Regex regex = new Regex("(MyVideos)(\\d+).db");
            Match match = regex.Match(videoDbFileName);
            if (match.Success)
            {
                int dbNumber = Convert.ToInt32(match.Groups[2].Value);
                if ((dbNumber < 90))
                {
                    return KodiVersion.Gotham;
                }
                if ((dbNumber < 91))
                {
                    return KodiVersion.Helix;
                }
                if ((dbNumber < 94))
                {
                    return KodiVersion.Isengard;
                }
                if ((dbNumber > 93))
                {
                    return KodiVersion.Jarvis;
                }
            }
            return KodiVersion.Gotham;
        }

        public static string ReadFile(string filePath)
        {
            //Reads the file and returns it back as a string variable.
            string fileText = File.ReadAllText(filePath);
            return fileText;
        }

        public static void SaveFile(string filePath, string Data)
        {
            if (File.Exists(filePath))
            {
                StreamWriter objWriter2 = new StreamWriter(filePath);
                objWriter2.Write(Data);
                objWriter2.Dispose();
                objWriter2.Close();
            }

        }

        public static string[] ReadPluginRecord(string dbLocation, string sqlStatement, int[] columnArray)
        {
            //Connect to the data-base

            string PluginDatabaseData = "Data Source=" + Settings.Default.AddonDatabaseLocation;

            string[] ArrayResponse = { "" };
            //This is a standard, SQLite database.
            var sqlConnect = new SQLiteConnection();
            SQLiteCommand sqlCommand = null;
            sqlConnect.ConnectionString = "Data Source=" + Settings.Default.AddonDatabaseLocation;

            try
            {
                sqlConnect.Open();
                sqlCommand = sqlConnect.CreateCommand();
                sqlCommand.CommandText = sqlStatement;
                var sqlReader = sqlCommand.ExecuteReader();

                int x = 0;

                while (sqlReader.Read())
                {
                    Array.Resize(ref ArrayResponse, x + 1);
                    var stringResponse = "";


                    for (var y = 0; y < columnArray.Length; y++)
                    {
                        if (y > 0)
                        {
                            stringResponse = stringResponse + "~" + sqlReader[columnArray[y]];
                        }
                        else
                        {
                            stringResponse = sqlReader[columnArray[y]].ToString();
                        }
                    }

                    ArrayResponse[x] = stringResponse;

                    x = x + 1;

                }

            }
            catch (SQLiteException myerror)
            {
                Logger.Error(myerror.Message);
                MessageBox.Show("Error Connecting to Database: " + myerror.Message);

            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnect.Close();

            }

            return ArrayResponse;
        }

        public static void PluginExecute(string SQLQuery)
        {
            string PluginDatabaseData = "Data Source=" + Settings.Default.AddonDatabaseLocation;

            //These are standard SQLite databases.
            //Open the connection.
            SQLiteConnection sqlConnect = new SQLiteConnection();
            SQLiteCommand sqlCommand = null;

            sqlConnect.ConnectionString = "Data Source=" + Settings.Default.AddonDatabaseLocation;

            try
            {
                sqlConnect.Open();

                //Set the command.
                sqlCommand = sqlConnect.CreateCommand();

                //Execute the command.
                sqlCommand.CommandText = SQLQuery;
                sqlCommand.ExecuteNonQuery();

            }
            catch (SQLiteException myerror)
            {
                Logger.Error(myerror.Message);
                MessageBox.Show("Error Connecting to Database: " + myerror.Message);
            }
            finally
            {
                //Dispose of and close the connection.
                sqlCommand.Dispose();
                sqlConnect.Close();
            }

        }

        public static string[] DbReadRecord(string dbLocation, string sqlStatement, int[] ColumnArray)
        {
            //Connect to the data-base

            //Dim VideoDatabaseData As String = "Data Source=" & MainWindow.VideoDatabaseLocation
            //Dim PluginDatabaseData As String = "Data Source=" & MainWindow.TxtAddonDatabaseLocation

            string[] arrayResponse = { "" };


            if (Settings.Default.DatabaseType == 0)
            {
                //This is a standard, SQLite database.
                var sqlConnect = new SQLiteConnection();
                SQLiteCommand sqlCommand = null;
                sqlConnect.ConnectionString = "Data Source=" + Settings.Default.VideoDatabaseLocation;

                try
                {
                    sqlConnect.Open();
                    sqlCommand = sqlConnect.CreateCommand();
                    sqlCommand.CommandText = sqlStatement;
                    var sqlReader = sqlCommand.ExecuteReader();

                    var x = 0;

                    while (sqlReader.Read())
                    {
                        Array.Resize(ref arrayResponse, x + 1);
                        var response = "";

                        for (var y = 0; y <= ColumnArray.Length - 1; y++)
                        {
                            response = y > 0
                                ? $"{response}~{sqlReader[ColumnArray[y]]}"
                                : sqlReader[ColumnArray[y]].ToString();
                        }

                        arrayResponse[x] = response;

                        x = x + 1;

                    }

                }
                catch (MySqlException myerror)
                {
                    Logger.Error(myerror.Message);
                    MessageBox.Show("Error Connecting to Database: " + myerror.Message);

                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnect.Close();
                }

            }
            else
            {
                //This is for MySQL Databases, just slight syntax differences.
                var sqlConnect = new MySqlConnection();
                MySqlCommand sqlCommand = null;
                sqlConnect.ConnectionString = Settings.Default.MySqlConnectionString;
                try
                {
                    sqlConnect.Open();
                    sqlCommand = sqlConnect.CreateCommand();
                    sqlCommand.CommandText = sqlStatement;
                    var sqlReader = sqlCommand.ExecuteReader();

                    var x = 0;

                    while (sqlReader.Read())
                    {
                        Array.Resize(ref arrayResponse, x + 1);
                        string StringResponse = "";


                        for (var y = 0; y <= ColumnArray.Length; y++)
                        {
                            StringResponse = y > 0
                                ? StringResponse + "~" + sqlReader[ColumnArray[y]]
                                : sqlReader[ColumnArray[y]].ToString();
                        }

                        arrayResponse[x] = StringResponse;
                        x++;
                    }

                }
                catch (MySqlException myerror)
                {
                    Logger.Error(myerror.Message);
                    MessageBox.Show("Error Connecting to Database: " + myerror.Message);
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnect.Close();

                }

            }


            return arrayResponse;

        }

        public static void DbExecute(string SQLQuery)
        {
            if (Settings.Default.DatabaseType == 0)
            {
                //These are standard SQLite databases.
                //Open the connection.
                SQLiteConnection sqlConnect = new SQLiteConnection();
                SQLiteCommand sqlCommand = null;
                sqlConnect.ConnectionString = "Data Source=" + Settings.Default.VideoDatabaseLocation;

                try
                {
                    sqlConnect.Open();

                    //Set the command.
                    sqlCommand = sqlConnect.CreateCommand();

                    //Execute the command.
                    sqlCommand.CommandText = SQLQuery;
                    sqlCommand.ExecuteNonQuery();

                }
                catch (MySqlException myerror)
                {
                    Logger.Error(myerror.Message);
                    MessageBox.Show("Error Connecting to Database: " + myerror.Message);
                }
                finally
                {
                    //Dispose of and close the connection.
                    sqlCommand.Dispose();
                    sqlConnect.Close();
                }

            }
            else
            {
                //These are for MYSQL connections
                //Just anything that says SQLite is changed to MySQL .. no biggy.

                //Open the connection.
                MySqlConnection sqlConnect = new MySqlConnection();
                MySqlCommand sqlCommand = null;
                sqlConnect.ConnectionString = Settings.Default.MySqlConnectionString;
                try
                {
                    sqlConnect.Open();

                    //Set the command.
                    sqlCommand = sqlConnect.CreateCommand();

                    //Execute the command.
                    sqlCommand.CommandText = SQLQuery;
                    sqlCommand.ExecuteNonQuery();
                }
                catch (MySqlException myerror)
                {
                    Logger.Error(myerror.Message);
                    MessageBox.Show("Error Connecting to Database: " + myerror.Message);
                    //Finally
                    //Dispose of and close the connection.
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnect.Close();

                }

            }
        }

        public static bool TestMySql(string connectionstring)
        {

            bool ConnectSuccessful = false;

            dynamic conn = new MySqlConnection();
            //Dim sqlCommand As MySqlCommand
            conn.ConnectionString = connectionstring;
            try
            {
                conn.Open();
                ConnectSuccessful = true;
                MessageBox.Show("Successfully connected to database.");
                conn.Close();
            }
            catch (MySqlException myerror)
            {
                Logger.Error(myerror.Message);
                MessageBox.Show("Error Connecting to Database: " + myerror.Message);
            }
            finally
            {
                conn.Dispose();
            }

            return ConnectSuccessful;
        }

        public static bool TestMySqlLite(string connectionstring)
        {
            var ConnectSuccessful = false;

            var conn = new SQLiteConnection();
            //Dim sqlCommand As MySqlCommand
            conn.ConnectionString = "Data Source=" + connectionstring;
            try
            {
                conn.Open();
                ConnectSuccessful = true;
                MessageBox.Show("Successfully connected to database.");
                conn.Close();
            }
            catch (MySqlException myerror)
            {
                Logger.Error(myerror.Message);
                MessageBox.Show("Error Connecting to Database: " + myerror.Message);
            }
            finally
            {
                conn.Dispose();
            }

            return ConnectSuccessful;
        }

        public static object TestMysql2(string DBLocation, string sqlStatement, string[] ColumnArray)
        {
            //Connect to the data-base
            var sqlConnect = new MySqlConnection
            {
                ConnectionString = "server=" + "127.0.0.1" + ";" + "user id=" + "xbmc" + ";" + "password=" +
                                   "xbmc" + ";" + "database=xbmcvideo60"
            };
            sqlConnect.Open();
            var sqlCommand = sqlConnect.CreateCommand();
            sqlCommand.CommandText = sqlStatement;
            var sqlReader = sqlCommand.ExecuteReader();

            int x = 0;

            string[] arrayResponse = null;
            while (sqlReader.Read())
            {
                //TODO: Artifact of VB Port, use list
                Array.Resize(ref arrayResponse, x + 1);
                var response = "";


                for (var y = 0; y <= ColumnArray.Length; y++)
                    response = y > 0 ? 
                        response + "~" + sqlReader[ColumnArray[y]] : 
                        sqlReader[ColumnArray[y]].ToString();

                arrayResponse[x] = response;

                x = x + 1;
            }
            sqlCommand.Dispose();
            sqlConnect.Close();

            return arrayResponse;
        }

        public static int LookUpGenre(string genreName)
        {
            //This looks up the Genre based on the name and returns the proper Genre ID
            var genreId = 0;

            var selectArray = new[] { 0 };

            var genrePar = "name";
            if ((Settings.Default.KodiVersion < (int)KodiVersion.Isengard))
            {
                genrePar = "strGenre";
            }

            //Shoot it over to the ReadRecord sub
            var returnArray = DbReadRecord(Settings.Default.VideoDatabaseLocation,
                "SELECT * FROM genre where " + genrePar + "='" + genreName + "'", selectArray);

            //The ID # is all we need.
            //Just make sure it's not a null reference.
            if (returnArray == null) MessageBox.Show("nothing!");
            else genreId = Convert.ToInt32(returnArray[0]);

            return genreId;
        }

        public static string LookUpNetwork(string network)
        {
            //This looks up the Network name and returns the proper Network ID

            string networkId = null;

            var selectArray = new[] { 0 };

            var studioPar = "name";
            if ((Settings.Default.KodiVersion < (int)KodiVersion.Isengard))
            {
                studioPar = "strStudio";
            }

            //Shoot it over to the ReadRecord sub
            var returnArray = DbReadRecord(Settings.Default.VideoDatabaseLocation,
                "SELECT * FROM studio where " + studioPar + "='" + network + "'", selectArray);

            //The ID # is all we need.
            //Just make sure it's not a null reference.
            if (returnArray == null)
            {
            }
            else
            {
                networkId = returnArray[0];
            }
            return networkId;

        }

        public static string ConvertGenres(ListBox genrelist)
        {
            //Converts the existing ListTVGenre's contents to the proper format.

            var tvGenresString = "";
            for (var x = 0; x <= genrelist.Items.Count - 1; x++)
            {
                if (x == 0)
                {
                    tvGenresString = genrelist.Items[x].ToString();
                }
                else
                {
                    tvGenresString = tvGenresString + " / " + genrelist.Items[x].ToString();
                }
            }

            return tvGenresString;
        }

        public static InputDialogResponse ShowInputDialog(string question, string inputTxt = null)
        {
            var size = new System.Drawing.Size(200, 70);
            var inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = size,
                Text = inputTxt ?? question
            };

            var textBox = new TextBox
            {
                Size = new System.Drawing.Size(size.Width - 10, 23),
                Location = new System.Drawing.Point(5, 5),
                Text = question
            };
            inputBox.Controls.Add(textBox);

            var okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new System.Drawing.Size(75, 23),
                Text = "&OK",
                Location = new System.Drawing.Point(size.Width - 80 - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            var cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                Size = new System.Drawing.Size(75, 23),
                Text = "&Cancel",
                Location = new System.Drawing.Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            var result = inputBox.ShowDialog();

            return new InputDialogResponse
            {
                DialogResult = result,
                Input = textBox.Text
            };
        }

        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }
    }


}