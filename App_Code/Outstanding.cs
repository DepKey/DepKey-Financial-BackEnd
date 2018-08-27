using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Outstanding
/// </summary>
public class Outstanding
{
    public Outstanding()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public int? OsNumber { get; set; }
    public DateTime? OsDate { get; set; }
    public float? Amount { get; set; }
    public int? StatusID { get; set; }
    public int? JournalMovementID { get; set; }

    public int? AccountID { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public static void InsertOutstanding(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Outstanding>(jsonString);
        
        param.Add(new System.Data.SqlClient.SqlParameter("@OsNumber", oneObject.OsNumber));
        param.Add(new System.Data.SqlClient.SqlParameter("@Amount", oneObject.Amount));
        param.Add(new System.Data.SqlClient.SqlParameter("@StatusID", oneObject.StatusID));
        param.Add(new System.Data.SqlClient.SqlParameter("@JournalMovementID", oneObject.JournalMovementID));
        
        DataTable dt = con.ExecSpSelect("OutStandingInsert", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
        
    }

    public static void SelectOutstanding(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Outstanding>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@AccountID", oneObject.AccountID));
        param.Add(new System.Data.SqlClient.SqlParameter("@DateFrom", oneObject.DateFrom));
        param.Add(new System.Data.SqlClient.SqlParameter("@DateTo", oneObject.DateTo));

        DataTable dt = con.ExecSpSelect("OutStandingSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void SelectOutstandingPartial(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Outstanding>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@JournalMovementID", oneObject.JournalMovementID));

        DataTable dt = con.ExecSpSelect("OutStandingSelectPartial", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void OutStandingSelectMaxOsNumber(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        DataTable dt = con.ExecSpSelect("OutStandingSelectMaxOsNumber", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }
}