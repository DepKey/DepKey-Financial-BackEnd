using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using Newtonsoft.Json.Linq;

/// <summary>
/// Summary description for Loockup
/// </summary>
public class Lookup
{
    public Lookup()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public string Title { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime CreationDate { get; set; }
    public int? LookupOrder { get; set; }
    public string Note { get; set; }
    public int? LookupTypeID { get; set; }

    public static void InsertLookup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Lookup>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@Title", oneObject.Title));
        param.Add(new System.Data.SqlClient.SqlParameter("@LookupTypeID", oneObject.LookupTypeID));
        if (oneObject.LookupOrder != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@LookupOrder", oneObject.LookupOrder));
        if (oneObject.Note != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Note", oneObject.Note));
        
        DataTable dt = con.ExecSpSelect("LookupInsert", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void UpdateLookup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Lookup>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
        param.Add(new System.Data.SqlClient.SqlParameter("@Title", oneObject.Title));
        param.Add(new System.Data.SqlClient.SqlParameter("@LookupTypeID", oneObject.LookupTypeID));
        if (oneObject.LookupOrder != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@LookupOrder", oneObject.LookupOrder));
        if (oneObject.Note != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Note", oneObject.Note));

        con.ExecSpNone("LookupUpdate", param);

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectLookup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Lookup>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
            if (oneObject.Title != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Title", oneObject.Title));
            if (oneObject.IsDeleted != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.LookupOrder != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@LookupOrder", oneObject.LookupOrder));
            if (oneObject.LookupTypeID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@LookupTypeID", oneObject.LookupTypeID));
            if (oneObject.Note != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Note", oneObject.Note));
        }

        param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("LookupSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void DeleteLookup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        param.Add(new System.Data.SqlClient.SqlParameter("@ID", int.Parse(context.Request.QueryString["ID"])));
        con.ExecSpNone("LookupDelete", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }
}