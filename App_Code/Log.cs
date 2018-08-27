using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Lookup
/// </summary>
public class Log
{
    public Log()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public string Details { get; set; }
    public int? Action { get; set; }
    public int? RelatedID { get; set; }
    public int? StaffID { get; set; }
    public int? Type { get; set; }
    public DateTime? CreationDate { get; set; }

    public DateTime? DateFrom { get; set; }
    public DateTime? @DateTo { get; set; } 
    public string TicketNumber { get; set; }
    public string ActionSearch { get; set; }
    public string TypeSearch { get; set; }

    public static void InsertLog(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Log>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@Details", oneObject.Details));
        param.Add(new System.Data.SqlClient.SqlParameter("@Action", oneObject.Action));
        if (oneObject.RelatedID != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@RelatedID", oneObject.RelatedID));
        param.Add(new System.Data.SqlClient.SqlParameter("@StaffID", oneObject.StaffID));
        param.Add(new System.Data.SqlClient.SqlParameter("@Type", oneObject.Type));

        con.ExecSpNone("LogInsert", param);

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectLog(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Log>(jsonString);

        if (oneObject != null)
        {
            if(oneObject.DateFrom != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@DateFrom", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@DateTo", oneObject.DateTo));
            if (oneObject.ActionSearch != null && oneObject.ActionSearch != "100")
                param.Add(new System.Data.SqlClient.SqlParameter("@Action", int.Parse(oneObject.ActionSearch)));
            if (oneObject.RelatedID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@RelatedID", oneObject.RelatedID));
            if (oneObject.StaffID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@StaffID", oneObject.StaffID));
            if (oneObject.TypeSearch != null && oneObject.TypeSearch != "100")
                param.Add(new System.Data.SqlClient.SqlParameter("@Type", int.Parse(oneObject.TypeSearch)));
            if (oneObject.TicketNumber != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@TicketNumber", oneObject.TicketNumber));
        }

        param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("LogSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }
}