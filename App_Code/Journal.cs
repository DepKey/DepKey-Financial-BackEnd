using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Journal
/// </summary>
public class Journal
{
    public Journal()
    {
        JournalMovements = new List<JournalMovement>();
        JournalMovementHistories = new List<JournalMovementHistory>();
    }

    public int? ID { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? JournalNumber { get; set; }
    public DateTime? JournalDate { get; set; }
    public int? JournalTypeID { get; set; }
    public int? BranchID { get; set; }
    public int? StaffID { get; set; }
    public int? SalesID { get; set; }
    public int? SalesStatusID { get; set; }

    public int? ApprovalStatusID { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }

    public List<JournalMovement> JournalMovements { get; set; }
    public List<JournalMovementHistory> JournalMovementHistories { get; set; }

    public static void InsertJournal(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Journal>(jsonString);

        param.Add(new SqlParameter("@JournalTypeID", oneObject.JournalTypeID));
        param.Add(new SqlParameter("@JournalDate", oneObject.JournalDate));
        param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
        param.Add(new SqlParameter("@StaffID", oneObject.StaffID));
        if (oneObject.SalesID != null)
            param.Add(new SqlParameter("@SalesID", oneObject.SalesID));
        if (oneObject.SalesStatusID != null)
            param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));

        DataTable dt = con.ExecSpSelect("JournalInsert", param);
        var journalID = int.Parse(dt.Rows[0]["Identity"].ToString());
        if (oneObject.JournalTypeID == 16)
        {
            param.Clear();
            //select sales
            param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.SalesID));
            param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", 1));
            param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", 1));
            DataTable dtSalteSelect = con.ExecSpSelect("SalesSelect", param);
            string jsonstring = con.DataTableToJson(dtSalteSelect);
            jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
            var SalesObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonstring);

            var SalesStatusID = int.Parse(dtSalteSelect.Rows[0]["SalesStatusID"].ToString());
            var RefAmountFromProvider = int.Parse(dtSalteSelect.Rows[0]["RefAmountFromProvider"].ToString());

            // get accpount for customer refund
            int RelatedID = 0;
            if (SalesObject.SalesTypeID == (int)SalesTypeEnum.BSP)
                RelatedID = (int)SalesTypeEnum.RefundBSPToCustomer;
            // XO or L.C same Account to Customer => refund
            else if (SalesObject.SalesTypeID == (int)SalesTypeEnum.XO || SalesObject.SalesTypeID == (int)SalesTypeEnum.LC)
                RelatedID = (int)SalesTypeEnum.RefundXOToCustomer;
            else if (SalesObject.SalesTypeID == (int)SalesTypeEnum.Voucher || SalesObject.SalesTypeID == (int)SalesTypeEnum.Cruise)
                RelatedID = (int)SalesTypeEnum.RefundVoucherToCustomer;
            else if (SalesObject.SalesTypeID == (int)SalesTypeEnum.Insurance)
                RelatedID = (int)SalesTypeEnum.RefundInsuranceToCustomer;
            else if (SalesObject.SalesTypeID == (int)SalesTypeEnum.Visa)
                RelatedID = (int)SalesTypeEnum.RefundVisaToCustomer;

            if (SalesStatusID == (int)SalesStatusEnum.Refunded)
            {
                //select account
                param.Clear();
                param.Add(new System.Data.SqlClient.SqlParameter("@SalesTypeID", RelatedID));
                DataTable dtMapping = con.ExecSpSelect("SalesMappingAccountSelect", param);
                // 1 mov
                foreach (DataRow row in dtMapping.Rows)
                {
                    // sales debit movment 4
                    if (int.Parse(row["DebitCredit"].ToString()) == 0 && int.Parse(row["SalesCost"].ToString()) == 0)
                    {
                        AddRefundMovement(param, journalID, oneObject, (float)oneObject.JournalMovements[0].Credit, 0,
                            int.Parse(row["AccountID"].ToString()));
                    }
                    // Cost debit movment 3
                    else if (int.Parse(row["DebitCredit"].ToString()) == 0 && int.Parse(row["SalesCost"].ToString()) == 1)
                    {
                        AddRefundMovement(param, journalID, oneObject, RefAmountFromProvider, 0,
                            int.Parse(row["AccountID"].ToString()));
                    }
                    //Cost credit movment 5
                    else if (int.Parse(row["DebitCredit"].ToString()) == 1 && int.Parse(row["SalesCost"].ToString()) == 1)
                    {
                        AddRefundMovement(param, journalID, oneObject, 0, RefAmountFromProvider,
                            int.Parse(row["AccountID"].ToString()));
                    }
                }

                // mov from screen
                foreach (var item in oneObject.JournalMovements)
                {
                    param.Clear();
                    param.Add(new SqlParameter("@JournalID", journalID));
                    param.Add(new SqlParameter("@Debit", item.Debit));
                    param.Add(new SqlParameter("@Credit", item.Credit));
                    param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                    param.Add(new SqlParameter("@AccountID", item.AccountID));
                    con.ExecSpNone("JournalMovementInsert", param);
                }
            }
            param.Clear();
            //Update sales 
            param.Add(new System.Data.SqlClient.SqlParameter("@ID", SalesObject.ID));
            param.Add(new System.Data.SqlClient.SqlParameter("@TicketNumber", SalesObject.TicketNumber));
            param.Add(new System.Data.SqlClient.SqlParameter("@CardNumber", SalesObject.CardNumber));
            param.Add(new System.Data.SqlClient.SqlParameter("@PaxName", SalesObject.PaxName));
            param.Add(new System.Data.SqlClient.SqlParameter("@Remarks", SalesObject.Remarks));
            param.Add(new System.Data.SqlClient.SqlParameter("@Destination", SalesObject.Destination));
            param.Add(new System.Data.SqlClient.SqlParameter("@PNR", SalesObject.PNR));
            param.Add(new System.Data.SqlClient.SqlParameter("@Fare", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@Tax", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@TotalCost", SalesObject.RefAmountFromProvider * -1));
            param.Add(new System.Data.SqlClient.SqlParameter("@SalesAmount", oneObject.JournalMovements[0].Credit * -1));
            param.Add(new System.Data.SqlClient.SqlParameter("@Profit",
                ((oneObject.JournalMovements[0].Credit * -1) - (SalesObject.RefAmountFromProvider * -1))));
            param.Add(new System.Data.SqlClient.SqlParameter("@Cash", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@Credit", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@Visa", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@Advance", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@Card", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@Complementary", 0));
            param.Add(new System.Data.SqlClient.SqlParameter("@DKNumber", SalesObject.DKNumber));
            param.Add(new System.Data.SqlClient.SqlParameter("@InvoiceNumberPNR", SalesObject.InvoiceNumberPNR));
            param.Add(new System.Data.SqlClient.SqlParameter("@SalesTypeID", SalesObject.SalesTypeID));
            param.Add(new System.Data.SqlClient.SqlParameter("@PaymentMethodID", SalesObject.PaymentMethodID));
            param.Add(new System.Data.SqlClient.SqlParameter("@AirlineID", SalesObject.AirlineID));
            param.Add(new System.Data.SqlClient.SqlParameter("@CreatedStaffID", SalesObject.CreatedStaffID));
            param.Add(new System.Data.SqlClient.SqlParameter("@AccountantStaffID", SalesObject.AccountantStaffID));
            param.Add(new System.Data.SqlClient.SqlParameter("@SalesStatusID", SalesStatusEnum.Refunded));
            param.Add(new System.Data.SqlClient.SqlParameter("@AccountID  ", SalesObject.AccountID));
            param.Add(new System.Data.SqlClient.SqlParameter("@SubAccountID  ", SalesObject.SubAccountID));
            param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", SalesObject.BranchID));
            param.Add(new System.Data.SqlClient.SqlParameter("@ParentID", SalesObject.ParentID));
            param.Add(new System.Data.SqlClient.SqlParameter("@RefAmountFromProvider", SalesObject.RefAmountFromProvider)); param.Add(new System.Data.SqlClient.SqlParameter("@RefAmountToCustomer", oneObject.JournalMovements[0].Credit));
            param.Add(new System.Data.SqlClient.SqlParameter("@RefDescription  ", SalesObject.RefDescription));
            param.Add(new System.Data.SqlClient.SqlParameter("@RefPaid", true));
            param.Add(new System.Data.SqlClient.SqlParameter("@CreationDate", SalesObject.CreationDate));
            con.ExecSpNone("SalesUpdate", param);
        }
        else {
            if (oneObject.JournalTypeID == 10)
            {
                param.Clear();
                param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.SalesID));
                param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", 1));
                param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", 1));
                DataTable dtSalteSelect = con.ExecSpSelect("SalesSelect", param);
                string jsonstring = con.DataTableToJson(dtSalteSelect);
                jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
                var SalesObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonstring);
                param.Clear();
                //Update sales 
                param.Add(new System.Data.SqlClient.SqlParameter("@ID", SalesObject.ID));
                param.Add(new System.Data.SqlClient.SqlParameter("@TicketNumber", SalesObject.TicketNumber));
                param.Add(new System.Data.SqlClient.SqlParameter("@CardNumber", SalesObject.CardNumber));
                param.Add(new System.Data.SqlClient.SqlParameter("@PaxName", SalesObject.PaxName));
                param.Add(new System.Data.SqlClient.SqlParameter("@Remarks", SalesObject.Remarks));
                param.Add(new System.Data.SqlClient.SqlParameter("@Destination", SalesObject.Destination));
                param.Add(new System.Data.SqlClient.SqlParameter("@PNR", SalesObject.PNR));
                param.Add(new System.Data.SqlClient.SqlParameter("@Fare", SalesObject.Fare));
                param.Add(new System.Data.SqlClient.SqlParameter("@Tax", SalesObject.Tax));
                param.Add(new System.Data.SqlClient.SqlParameter("@TotalCost", SalesObject.TotalCost));
                param.Add(new System.Data.SqlClient.SqlParameter("@SalesAmount", SalesObject.SalesAmount));
                param.Add(new System.Data.SqlClient.SqlParameter("@Profit", SalesObject.Profit));
                param.Add(new System.Data.SqlClient.SqlParameter("@Cash", SalesObject.Cash));
                param.Add(new System.Data.SqlClient.SqlParameter("@Credit", SalesObject.Credit));
                param.Add(new System.Data.SqlClient.SqlParameter("@Visa", SalesObject.Visa));
                param.Add(new System.Data.SqlClient.SqlParameter("@Advance", SalesObject.Advance));
                param.Add(new System.Data.SqlClient.SqlParameter("@Card", SalesObject.Card));
                param.Add(new System.Data.SqlClient.SqlParameter("@Complementary", SalesObject.Complementary));
                param.Add(new System.Data.SqlClient.SqlParameter("@DKNumber", SalesObject.DKNumber));
                param.Add(new System.Data.SqlClient.SqlParameter("@InvoiceNumberPNR", SalesObject.InvoiceNumberPNR));
                param.Add(new System.Data.SqlClient.SqlParameter("@SalesTypeID", SalesObject.SalesTypeID));
                param.Add(new System.Data.SqlClient.SqlParameter("@PaymentMethodID", SalesObject.PaymentMethodID));
                param.Add(new System.Data.SqlClient.SqlParameter("@AirlineID", SalesObject.AirlineID));
                param.Add(new System.Data.SqlClient.SqlParameter("@CreatedStaffID", SalesObject.CreatedStaffID));
                param.Add(new System.Data.SqlClient.SqlParameter("@AccountantStaffID", SalesObject.AccountantStaffID));
                param.Add(new System.Data.SqlClient.SqlParameter("@SalesStatusID", SalesStatusEnum.Void));
                param.Add(new System.Data.SqlClient.SqlParameter("@AccountID  ", SalesObject.AccountID));
                param.Add(new System.Data.SqlClient.SqlParameter("@SubAccountID  ", SalesObject.SubAccountID));
                param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", SalesObject.BranchID));
                param.Add(new System.Data.SqlClient.SqlParameter("@ParentID", SalesObject.ParentID));
                param.Add(new System.Data.SqlClient.SqlParameter("@RefAmountFromProvider", SalesObject.RefAmountFromProvider)); param.Add(new System.Data.SqlClient.SqlParameter("@RefAmountToCustomer", oneObject.JournalMovements[0].Credit));
                param.Add(new System.Data.SqlClient.SqlParameter("@RefPaid", true));
                param.Add(new System.Data.SqlClient.SqlParameter("@CreationDate", SalesObject.CreationDate));
                con.ExecSpNone("SalesUpdate", param);
            }
            if (oneObject.JournalMovements.Count != 0)
            {
                foreach (var item in oneObject.JournalMovements)
                {
                    param.Clear();
                    param.Add(new SqlParameter("@JournalID", journalID));
                    param.Add(new SqlParameter("@Debit", item.Debit));
                    param.Add(new SqlParameter("@Credit", item.Credit));
                    param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                    param.Add(new SqlParameter("@AccountID", item.AccountID));
                    con.ExecSpNone("JournalMovementInsert", param);
                }
            }
        }

        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void UpdateJournal(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Journal>(jsonString);

        param.Add(new SqlParameter("@ID", oneObject.ID));
        param.Add(new SqlParameter("@JournalDate", oneObject.JournalDate));

        con.ExecSpNone("JournalUpdate", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectJournal(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Journal>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
            {
                param.Add(new SqlParameter("@ID", oneObject.ID));
                DataTable Movmentdt = con.ExecSpSelect("JournalSelect", param);
                string Movmentresult = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(Movmentdt) + "}";
                context.Response.BinaryWrite(Encoding.UTF8.GetBytes(Movmentresult));
            }
            if (oneObject.JournalNumber != null)
                param.Add(new SqlParameter("@JournalNumber", oneObject.JournalNumber));
            if (oneObject.JournalTypeID != null)
                param.Add(new SqlParameter("@JournalTypeID", oneObject.JournalTypeID));
            if (oneObject.StaffID != null)
                param.Add(new SqlParameter("@StaffID", oneObject.StaffID));
            if (oneObject.BranchID != null)
                param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
            if (oneObject.SalesID != null)
                param.Add(new SqlParameter("@SalesID", oneObject.SalesID));
            if (oneObject.JournalDate != null)
                param.Add(new SqlParameter("@JournalDate", oneObject.JournalDate));
            if (oneObject.DateFrom != null)
                param.Add(new SqlParameter("@DateFrom", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new SqlParameter("@DateTo", oneObject.DateTo));
            if (oneObject.JournalDate != null)
                param.Add(new SqlParameter("@JournalDate", oneObject.JournalDate));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.ApprovalStatusID != null)
                param.Add(new SqlParameter("@ApprovalStatusID", oneObject.ApprovalStatusID));
        }
        param.Add(new SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));
        param.Add(new SqlParameter("@IsDeleted", false));

        DataTable dt = con.ExecSpSelect("JournalSelect", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void DeleteJournal(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        param.Add(new SqlParameter("@ID", int.Parse(context.Request.QueryString["ID"])));
        con.ExecSpNone("JournalDelete", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    private static void AddRefundMovement(ArrayList param, int JournalID, Journal oneObject,
      float? Debit, float? Credit, int? Account)
    {
        DatabaseConnection con = new DatabaseConnection();
        param.Clear();
        param.Add(new System.Data.SqlClient.SqlParameter("@JournalID", JournalID));
        param.Add(new System.Data.SqlClient.SqlParameter("@AccountID", Account));
        param.Add(new System.Data.SqlClient.SqlParameter("@Debit", Debit));
        param.Add(new System.Data.SqlClient.SqlParameter("@Credit", Credit));
        param.Add(new System.Data.SqlClient.SqlParameter("@MovementDescription", oneObject.JournalMovements.FirstOrDefault().MovementDescription));

        con.ExecSpNone("JournalMovementInsert", param);
    }
}