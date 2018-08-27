using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNet.SignalR;


/// <summary>
/// Summary description for Sales
/// </summary>
public class SalesGroup
{

    private static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<SalesHub>();

    public SalesGroup()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public int? SaleID { get; set; }
    public int? InvoiceNumber { get; set; }

    public static void InsertSalesGroup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesGroup>(jsonString);

        param.Add(new SqlParameter("@SaleID", oneObject.SaleID));
        param.Add(new SqlParameter("@InvoiceNumber", oneObject.InvoiceNumber));
        
        con.ExecSpNone("SalesGroupInsert", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"}}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void InsertSalesGroupByID(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList paramMax = new System.Collections.ArrayList();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(jsonString);

        DataTable dtMaxInvoice = con.ExecSpSelect("SalesSelectMaxInvoice", paramMax);
        int iMaxInvoice = int.Parse(dtMaxInvoice.Rows[0]["InvoiceNumber"].ToString()) + 1;
        
        if (oneObject.Count() > 0)
        {
            foreach (var item in oneObject)
            {
                param.Clear();
                param.Add(new SqlParameter("@SaleID", item));
                param.Add(new SqlParameter("@InvoiceNumber", iMaxInvoice));
                con.ExecSpNone("SalesGroupInsert", param);
            }
        }

        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"InvoiceNumber\":" + iMaxInvoice + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void SelectSalesGroup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesGroup>(jsonString);
        
        param.Add(new SqlParameter("@InvoiceNumber", oneObject.InvoiceNumber));

        DataTable dt = con.ExecSpSelect("SalesGroupSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void SalesSelectMaxInvoice(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();

        DataTable dt = con.ExecSpSelect("SalesSelectMaxInvoice", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"InvoiceNumber\":" + dt.Rows[0]["InvoiceNumber"].ToString() + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void DeleteSalesGroup(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesGroup>(jsonString);

        param.Add(new SqlParameter("@SaleID", oneObject.SaleID));
        param.Add(new SqlParameter("@InvoiceNumber", oneObject.InvoiceNumber));

        con.ExecSpNone("SalesGroupDelete", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"}}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

}