using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Loockup
/// </summary>
public class Airline
{
    public Airline()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public string Name { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime CreationDate { get; set; }
    public string Code { get; set; }
    public string SabreCode { get; set; }
    public string IATACode { get; set; }


    public static void InsertAirline(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Airline>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
        param.Add(new System.Data.SqlClient.SqlParameter("@SabreCode", oneObject.SabreCode));
        param.Add(new System.Data.SqlClient.SqlParameter("@Code", oneObject.Code));
        param.Add(new System.Data.SqlClient.SqlParameter("@SabreCode", oneObject.SabreCode));


        con.ExecSpNone("AirlineInsert", param);

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void UpdateAirline(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Airline>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
        param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
        param.Add(new System.Data.SqlClient.SqlParameter("@SabreCode", oneObject.SabreCode));
        param.Add(new System.Data.SqlClient.SqlParameter("@Code", oneObject.Code));
        param.Add(new System.Data.SqlClient.SqlParameter("@SabreCode", oneObject.SabreCode));


        con.ExecSpNone("AirlineUpdate", param);

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectAirline(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Airline>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
            if (oneObject.Name != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
            if (oneObject.Code != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Code", oneObject.Code));
            if (oneObject.SabreCode != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@SabreCode", oneObject.SabreCode));
            if (oneObject.IATACode != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@IATACode", oneObject.IATACode));
        }

        param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("AirlineSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void DeleteAirline(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        param.Add(new System.Data.SqlClient.SqlParameter("@ID", int.Parse(context.Request.QueryString["ID"])));
        con.ExecSpNone("AirlineDelete", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }
}