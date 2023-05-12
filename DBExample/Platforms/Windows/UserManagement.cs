using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.System;

namespace DBExample.Services;

public partial class UserManagement
{    
    internal OleDbDataAdapter Users_Adapter = new();

    internal OleDbConnection Connexion = new();
    
    internal void ConfigTools()
    {
        Connexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.16.0;"
                                    + @"Data Source=" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QualityServer", "UserAccounts.accdb")
                                    + ";Persist Security Info=False";

        string Insert_CommandText = "INSERT INTO DB_Users(UserName,UserPassword,UserAccessType) VALUES (@UserName,@UserPassword,@UserAccessType);";
        string Delete_CommandText = "DELETE FROM DB_Users WHERE UserName = @UserName;";
        string Select_CommandText = "SELECT * FROM DB_Users ORDER BY User_ID;";
        string Update_CommandText = "UPDATE DB_Users SET UserPassword = @UserPassword, UserAccessType = @UserAccessType WHERE UserName = @UserName;";

        OleDbCommand Insert_Command = new OleDbCommand(Insert_CommandText, Connexion);
        OleDbCommand Delete_Command = new OleDbCommand(Delete_CommandText, Connexion);
        OleDbCommand Select_Command = new OleDbCommand(Select_CommandText, Connexion);
        OleDbCommand Update_Command = new OleDbCommand(Update_CommandText, Connexion);

        Users_Adapter.SelectCommand = Select_Command;
        Users_Adapter.InsertCommand = Insert_Command;
        Users_Adapter.DeleteCommand = Delete_Command;
        Users_Adapter.UpdateCommand = Update_Command;

        Users_Adapter.TableMappings.Add("DB_Users", "Users");
        Users_Adapter.TableMappings.Add("DB_Access", "Access");

        Users_Adapter.InsertCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        Users_Adapter.InsertCommand.Parameters.Add("@UserPassword", OleDbType.VarChar, 40, "UserPassword");
        Users_Adapter.InsertCommand.Parameters.Add("@UserAccessType", OleDbType.Numeric, 100, "UserAccessType");
        Users_Adapter.DeleteCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        Users_Adapter.UpdateCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        Users_Adapter.UpdateCommand.Parameters.Add("@UserPassword", OleDbType.VarChar, 40, "UserPassword");
        Users_Adapter.UpdateCommand.Parameters.Add("@UserAccessType", OleDbType.Numeric, 100, "UserAccessType");
    }
    internal async Task InsertUsers(string name,string password, Int32 access)
    {
        try
        {
            Connexion.Open();

            Users_Adapter.InsertCommand.Parameters[0].Value = name;
            Users_Adapter.InsertCommand.Parameters[1].Value = password;
            Users_Adapter.InsertCommand.Parameters[2].Value = 2;

            int buffer = Users_Adapter.InsertCommand.ExecuteNonQuery();

            if (buffer != 0) await Shell.Current.DisplayAlert("Database", "User successfully inserted", "OK");
            else await Shell.Current.DisplayAlert("Database", "User not inserted", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }
        finally
        {
            Connexion.Close();
        }
    }
    internal async Task DeleteUsers(String Name)
    {
       Users_Adapter.DeleteCommand.Parameters[0].Value = Name;

        try
        {
            Connexion.Open();

            int buffer = Users_Adapter.DeleteCommand.ExecuteNonQuery();

            if (buffer != 0) await Shell.Current.DisplayAlert("Database", "User successfully deleted", "OK");
            else await Shell.Current.DisplayAlert("Database", "User not found", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }
        finally
        {
            Connexion.Close();
        }
    }
    internal async Task FillUsers()
    {
        Globals.UserSets.Tables["Users"].Clear();

        try
        {
            Connexion.Open();

            Users_Adapter.Fill(Globals.UserSets.Tables["Users"]);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }
        finally
        {
            Connexion.Close();
        }
    }
    internal async Task UpdateUsers( )
    {
        try
        {
            Connexion.Open();

            Users_Adapter.Update(Globals.UserSets.Tables["Users"]);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }
        finally
        {
            Connexion.Close();
        }
    }
    internal async Task ReadAccessTable()
    {
        OleDbCommand SelectCommand = new OleDbCommand("SELECT * from DB_Access;", Connexion);

        try
        {
            Connexion.Open();

            OleDbDataReader DBReader = SelectCommand.ExecuteReader();

            if (DBReader.HasRows)
            {
                while (DBReader.Read())
                {
                    Globals.UserSets.Tables["Access"].Rows.Add(new object[] { DBReader[0], DBReader[1], DBReader[2], DBReader[3], DBReader[4], DBReader[5]});
                }
            }

            DBReader.Close();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }
        finally
        {
            Connexion.Close();
        }
    }
}
