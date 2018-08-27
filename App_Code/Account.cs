using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Account
/// </summary>
public class Account
{
    public Account()
    {
        Account1 = new List<Account>();
        JournalMovements = new List<JournalMovement>();
        JournalMovementHistories = new List<JournalMovementHistory>();
    }

    public int? ID { get; set; }
    public string Name { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreationDate { get; set; }
    public string AccountNumber { get; set; }
    public double? Debit { get; set; }
    public double? Credit { get; set; }
    public double? Limit { get; set; }
    public string Description { get; set; }
    public int? StaffID { get; set; }
    public int? AccountTypeID { get; set; }
    public int? ParentID { get; set; }

    public Account Account2 { get; set; }
    public Lookup Lookup { get; set; }
    public List<Account> Account1 { get; set; }
    public List<JournalMovement> JournalMovements { get; set; }
    public List<JournalMovementHistory> JournalMovementHistories { get; set; }
    public List<Sales> Sales { get; set; }
    public int? BranchID { get; set; }

    //search Fields
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? AccountID { get; set; }

    public static void InsertAccount(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
        param.Add(new System.Data.SqlClient.SqlParameter("@AccountNumber", oneObject.AccountNumber));
        if (oneObject.Debit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Debit", oneObject.Debit));
        if (oneObject.Credit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Credit", oneObject.Credit));
        if (oneObject.Limit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Limit", oneObject.Limit));
        if (oneObject.Description != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Description", oneObject.Description));
        if (oneObject.ParentID != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@ParentID", oneObject.ParentID));
        param.Add(new System.Data.SqlClient.SqlParameter("@StaffID", oneObject.StaffID));
        param.Add(new System.Data.SqlClient.SqlParameter("@AccountTypeID", oneObject.AccountTypeID));
        param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", oneObject.BranchID)); 

        DataTable dt = con.ExecSpSelect("AccountInsert", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void UpdateAccount(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(jsonString);

        param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
        param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
        param.Add(new System.Data.SqlClient.SqlParameter("@AccountNumber", oneObject.AccountNumber));
        if (oneObject.Debit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Debit", oneObject.Debit));
        if (oneObject.Credit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Credit", oneObject.Credit));
        if (oneObject.Limit != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Limit", oneObject.Limit));
        if (oneObject.Description != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@Description", oneObject.Description));
        if (oneObject.ParentID != null)
            param.Add(new System.Data.SqlClient.SqlParameter("@ParentID", oneObject.ParentID));
        param.Add(new System.Data.SqlClient.SqlParameter("@StaffID", oneObject.StaffID));
        param.Add(new System.Data.SqlClient.SqlParameter("@AccountTypeID", oneObject.AccountTypeID));
        param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", oneObject.BranchID));

        con.ExecSpNone("AccountUpdate", param);

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectAccount(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new SqlParameter("@ID", oneObject.ID));
            if (oneObject.Name != null)
                param.Add(new SqlParameter("@Name", oneObject.Name));
            if (oneObject.AccountNumber != null)
                param.Add(new SqlParameter("@AccountNumber", oneObject.AccountNumber));
            if (oneObject.StaffID != null)
                param.Add(new SqlParameter("@StaffID", oneObject.StaffID));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.AccountTypeID != null)
                param.Add(new SqlParameter("@AccountTypeID", oneObject.AccountTypeID));
            if (oneObject.ParentID != null)
                param.Add(new SqlParameter("@ParentID", oneObject.ParentID));
            if (oneObject.Description != null)
                param.Add(new SqlParameter("@Description", oneObject.Description));
            if (oneObject.BranchID != null)
                param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
        }
        param.Add(new SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("AccountSelect", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectAccountDC(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.AccountID != null)
                param.Add(new SqlParameter("@ID", oneObject.AccountID));
            if (oneObject.DateFrom != null)
                param.Add(new SqlParameter("@Date", oneObject.DateFrom));
        }

        DataTable dt = con.ExecSpSelect("AccountSelectDC", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectAccountWithSub(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new SqlParameter("@ID", oneObject.ID));
            if (oneObject.Name != null)
                param.Add(new SqlParameter("@Name", oneObject.Name));
            if (oneObject.AccountNumber != null)
                param.Add(new SqlParameter("@AccountNumber", oneObject.AccountNumber));
            if (oneObject.StaffID != null)
                param.Add(new SqlParameter("@StaffID", oneObject.StaffID));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.AccountTypeID != null)
                param.Add(new SqlParameter("@AccountTypeID", oneObject.AccountTypeID));
            if (oneObject.ParentID != null)
                param.Add(new SqlParameter("@ParentID", oneObject.ParentID));
        }
        param.Add(new SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("AccountSelectWithSub", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void DeleteAccount(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        param.Add(new System.Data.SqlClient.SqlParameter("@ID", int.Parse(context.Request.QueryString["ID"])));
        con.ExecSpNone("AccountDelete", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectAccountsForTrialBalanceReport(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.DateFrom != null)
                param.Add(new SqlParameter("@DateFrom", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new SqlParameter("@DateTo", oneObject.DateTo));

        }
        DataTable dt = con.ExecSpSelect("RptTrialBalanceReport", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

}