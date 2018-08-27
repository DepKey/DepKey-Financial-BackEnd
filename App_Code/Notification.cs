using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Loockup
/// </summary>
public class Notification
{
    public Notification()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int? ID { get; set; }
    public string Description { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsSeen { get; set; }
    public DateTime CreationDate { get; set; }
    public int? RelatedID { get; set; }

    public static void InsertNotification(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Notification>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@Description", oneObject.Description));
        param.Add(new System.Data.SqlClient.SqlParameter("@RelatedID", oneObject.RelatedID));

        con.ExecSpNone("NotificationInsert", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void UpdateNotification(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Notification>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
        param.Add(new System.Data.SqlClient.SqlParameter("@IsSeen", oneObject.IsSeen));

        con.ExecSpNone("NotificationUpdate", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectNotification(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Notification>(jsonString);
        if (oneObject != null)
        {
            if (oneObject.RelatedID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@RelatedID", oneObject.RelatedID));
            if (oneObject.IsSeen != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@IsSeen", oneObject.IsSeen));
        }

        DataTable dt = con.ExecSpSelect("NotificationSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void CountNotification(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        DataTable dt = con.ExecSpSelect("NotificationNotSeenCount", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }
}