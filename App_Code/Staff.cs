using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Loockup
/// </summary>
public class Staff
{
    public Staff()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public string Name { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime CreationDate { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int? BranchID { get; set; }
    public int? RoleID { get; set; }
    public string Email { get; set; }
    public string Limit { get; set; }
    public string UserPages { get; set; }
    public string UserPermissions { get; set; }
    public int[] UserPagesList { get; set; }
    public int[] UserPermissionsList { get; set; }

    public static void InsertStaff(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Staff>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
        if (oneObject.Username != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Username", oneObject.Username));
        if (oneObject.Password != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Password", oneObject.Password));
        if (oneObject.Email != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Email", oneObject.Email));
        if (oneObject.Limit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Limit", oneObject.Limit));
        param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", oneObject.BranchID));
        param.Add(new System.Data.SqlClient.SqlParameter("@RoleID", oneObject.RoleID));
        if (oneObject.UserPagesList.Length != 0)
        {
            string strUserPages = "";
            foreach (int item in oneObject.UserPagesList)
                strUserPages += item.ToString() + ',';
            strUserPages = strUserPages.Remove(strUserPages.Length - 1);
            param.Add(new System.Data.SqlClient.SqlParameter("@UserPages", strUserPages));
        }
        if (oneObject.UserPagesList.Length != 0)
        {
            string strPermissions = "";
            foreach (int item in oneObject.UserPermissionsList)
                strPermissions += item.ToString() + ',';
            strPermissions = strPermissions.Remove(strPermissions.Length - 1);
            param.Add(new System.Data.SqlClient.SqlParameter("@UserPermissions", strPermissions));
        }
        
        DataTable dt = con.ExecSpSelect("StaffInsert", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void UpdateStaff(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Staff>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
        param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
        if (oneObject.Username != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Username", oneObject.Username));
        if (oneObject.Password != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Password", oneObject.Password));
        if (oneObject.Email != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Email", oneObject.Email));
        if (oneObject.Limit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Limit", oneObject.Limit));
        param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", oneObject.BranchID));
        param.Add(new System.Data.SqlClient.SqlParameter("@RoleID", oneObject.RoleID));
        if (oneObject.UserPagesList != null)
        {
            string strUserPages = "";
            foreach (int item in oneObject.UserPagesList)
                strUserPages += item.ToString() + ',';
            strUserPages = strUserPages.Remove(strUserPages.Length - 1);
            param.Add(new System.Data.SqlClient.SqlParameter("@UserPages", strUserPages));
        }
        if (oneObject.UserPermissionsList != null)
        {
            string strPermissions = "";
            foreach (int item in oneObject.UserPermissionsList)
                strPermissions += item.ToString() + ',';
            strPermissions = strPermissions.Remove(strPermissions.Length - 1);
            param.Add(new System.Data.SqlClient.SqlParameter("@UserPermissions", strPermissions));
        }

        con.ExecSpNone("StaffUpdate", param);

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }
    public static void SelectStaff(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Staff>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
            if (oneObject.Name != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
            if (oneObject.IsDeleted != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.Username != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@UserName", oneObject.Username));
            if (oneObject.Password != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Password", oneObject.Password));
            if (oneObject.BranchID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", oneObject.BranchID));
            if (oneObject.RoleID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@RoleID", oneObject.RoleID));
        }

        param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("StaffSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }
    public static void DeleteStaff(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        param.Add(new System.Data.SqlClient.SqlParameter("@ID", int.Parse(context.Request.QueryString["ID"])));
        con.ExecSpNone("StaffDelete", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

}