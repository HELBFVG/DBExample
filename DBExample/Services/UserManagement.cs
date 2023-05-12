using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBExample.Services;

public partial class UserManagement
{  
    public UserManagement()
    {
        DataTable UserTable = new DataTable();
        DataTable AccessTable = new DataTable();

        DataColumn User_ID = new DataColumn("User_ID", System.Type.GetType("System.Int16"));
        DataColumn UserName = new DataColumn("UserName", System.Type.GetType("System.String"));
        DataColumn UserPassword = new DataColumn("UserPassword", System.Type.GetType("System.String"));
        DataColumn AccessType = new DataColumn("UserAccessType", System.Type.GetType("System.Int16"));

        DataColumn Access_ID = new DataColumn("Access_ID", System.Type.GetType("System.Int16"));
        DataColumn AccessName = new DataColumn("AccessName", System.Type.GetType("System.String"));
        DataColumn CreateObject = new DataColumn("CreateObject", System.Type.GetType("System.Boolean"));
        DataColumn DestroyObject = new DataColumn("DestroyObject", System.Type.GetType("System.Boolean"));
        DataColumn ModifyObject = new DataColumn("ModifyObject", System.Type.GetType("System.Boolean"));
        DataColumn ChangeUserRights = new DataColumn("ChangeUserRights", System.Type.GetType("System.Boolean"));

        /* UserTable*/
        UserTable.TableName = "Users";
        User_ID.Unique = true;
        User_ID.AutoIncrement = true;
        UserTable.Columns.Add(User_ID);

        UserName.Unique = true;
        UserTable.Columns.Add(UserName);

        UserPassword.Unique = false;
        UserTable.Columns.Add(UserPassword);

        AccessType.Unique = false;
        UserTable.Columns.Add(AccessType);

        /* AccessTable*/
        AccessTable.TableName = "Access";
        Access_ID.Unique = true;
        AccessTable.Columns.Add(Access_ID);

        AccessName.Unique = true;
        AccessTable.Columns.Add(AccessName);

        CreateObject.Unique = false;
        AccessTable.Columns.Add(CreateObject);

        DestroyObject.Unique = false;
        AccessTable.Columns.Add(DestroyObject);

        ModifyObject.Unique = false;
        AccessTable.Columns.Add(ModifyObject);

        ChangeUserRights.Unique = false;
        AccessTable.Columns.Add(ChangeUserRights);

        /* Dataset & Datarelation */
        Globals.UserSets.Tables.Add(UserTable);
        Globals.UserSets.Tables.Add(AccessTable);

        DataColumn parentColumn = Globals.UserSets.Tables["Access"].Columns["Access_ID"];
        DataColumn childColumn = Globals.UserSets.Tables["Users"].Columns["UserAccessType"];

        DataRelation relation = new DataRelation("Access2User", parentColumn, childColumn);

        Globals.UserSets.Tables["Users"].ParentRelations.Add(relation);
    }
}



