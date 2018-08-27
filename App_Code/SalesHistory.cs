using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.AspNet.SignalR;

public class SalesHistory
{

    private static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<SalesHub>();
    public SalesHistory()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public string Title { get; set; }
    public DateTime CreationDate { get; set; }
    public string CardNumber { get; set; }
    public string PaxName { get; set; }
    public string Remarks { get; set; }
    public string Destination { get; set; }
    public string PNR { get; set; }
    public float? Fare { get; set; }
    public float? Tax { get; set; }
    public float? TotalCost { get; set; }
    public float? SalesAmount { get; set; }
    public float? Profit { get; set; }
    public float? Cash { get; set; }
    public float? Credit { get; set; }
    public float? Visa { get; set; }
    public float? Advance { get; set; }
    public float? Card { get; set; }
    public float? Complementary { get; set; }
    public float? Commision { get; set; }
    public int? DKNumber { get; set; }
    public int? InvoiceNumber { get; set; }
    public int? InvoiceNumberPNR { get; set; }
    public string TicketNumber { get; set; }
    public int? SalesTypeID { get; set; }
    public int? PaymentMethodID { get; set; }
    public int? AirlineID { get; set; }
    public int? CreatedStaffID { get; set; }
    public int? AccountantStaffID { get; set; }
    public int? AccountID { get; set; }
    public int? BankAccountID { get; set; }
    public int? SubAccountID { get; set; }
    public int? CreditAccountID { get; set; }
    public int? RecievableMainAccountID { get; set; }
    public int? BranchID { get; set; }
    public int? SalesStatusID { get; set; }

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public bool? IsDeleted { get; set; }

    public int? ApproveStatusID { get; set; }
    public string RejectionReason { get; set; }
    public int? SaleID { get; set; }
    public int? HistoryCreatedStaffID { get; set; }

    public float? RefAmountFromProvider { get; set; }
    public float? RefAmountToCustomer { get; set; }
    public string RefRemarks { get; set; }
    public string RefDescription { get; set; }

    public static void InsertSalesHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesHistory>(jsonString);
        if (oneObject != null)
        {
            if (oneObject.TicketNumber != null)
                param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
            if (oneObject.CardNumber != null)
                param.Add(new SqlParameter("@CardNumber", oneObject.CardNumber));
            if (oneObject.PaxName != null)
                param.Add(new SqlParameter("@PaxName", oneObject.PaxName));
            if (oneObject.Remarks != null)
                param.Add(new SqlParameter("@Remarks", oneObject.Remarks));
            if (oneObject.Destination != null)
                param.Add(new SqlParameter("@Destination", oneObject.Destination));
            if (oneObject.PNR != null)
                param.Add(new SqlParameter("@PNR", oneObject.PNR));
            if (oneObject.Fare != null)
                param.Add(new SqlParameter("@Fare", oneObject.Fare));
            if (oneObject.Tax != null)
                param.Add(new SqlParameter("@Tax", oneObject.Tax));
            if (oneObject.TotalCost != null)
                param.Add(new SqlParameter("@TotalCost", oneObject.TotalCost));
            if (oneObject.SalesAmount != null)
                param.Add(new SqlParameter("@SalesAmount", oneObject.SalesAmount));
            if (oneObject.Profit != null)
                param.Add(new SqlParameter("@Profit", oneObject.Profit));
            if (oneObject.Cash != null)
                param.Add(new SqlParameter("@Cash", oneObject.Cash));
            if (oneObject.Credit != null)
                param.Add(new SqlParameter("@Credit", oneObject.Credit));
            if (oneObject.Visa != null)
                param.Add(new SqlParameter("@Visa", oneObject.Visa));
            if (oneObject.Advance != null)
                param.Add(new SqlParameter("@Advance", oneObject.Advance));
            if (oneObject.Commision != null)
                param.Add(new SqlParameter("@Commision", oneObject.Commision));
            if (oneObject.Card != null)
                param.Add(new SqlParameter("@Card", oneObject.Card));
            if (oneObject.Complementary != null)
                param.Add(new SqlParameter("@Complementary", oneObject.Complementary));
            if (oneObject.DKNumber != null)
                param.Add(new SqlParameter("@DKNumber", oneObject.DKNumber));
            if (oneObject.InvoiceNumber != null)
                param.Add(new SqlParameter("@InvoiceNumber", oneObject.InvoiceNumber));
            if (oneObject.InvoiceNumberPNR != null)
                param.Add(new SqlParameter("@InvoiceNumberPNR", oneObject.InvoiceNumberPNR));
            if (oneObject.SalesTypeID != null)
                param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
            if (oneObject.PaymentMethodID != null)
                param.Add(new SqlParameter("@PaymentMethodID", oneObject.PaymentMethodID));
            if (oneObject.AirlineID != null)
                param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
            if (oneObject.CreatedStaffID != null)
                param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
            if (oneObject.AccountantStaffID != null)
                param.Add(new SqlParameter("@AccountantStaffID", oneObject.AccountantStaffID));
            if (oneObject.AccountID != null)
                param.Add(new SqlParameter("@AccountID", oneObject.AccountID));
            if (oneObject.BranchID != null)
                param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
            if (oneObject.SubAccountID != null)
                param.Add(new SqlParameter("@SubAccountID", oneObject.SubAccountID));
            if (oneObject.HistoryCreatedStaffID != null)
                param.Add(new SqlParameter("@HistoryCreatedStaffID", oneObject.HistoryCreatedStaffID));
            if (oneObject.RecievableMainAccountID != null)
                param.Add(new SqlParameter("@RecievableMainAccountID", oneObject.RecievableMainAccountID));
            if (oneObject.CreditAccountID != null)
                param.Add(new SqlParameter("@CreditAccountID", oneObject.CreditAccountID));
            if (oneObject.BankAccountID != null)
                param.Add(new SqlParameter("@BankAccountID  ", oneObject.BankAccountID));
            if (oneObject.CreationDate != null)
                param.Add(new SqlParameter("@CreationDate  ", oneObject.CreationDate));
            param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));
            param.Add(new SqlParameter("@SaleID", oneObject.ID));

            con.ExecSpNone("SalesHistoryInsert", param);

            #region Notification
            string Msg = "";
            if (oneObject.SalesStatusID == (int)SalesStatusEnum.PendingVoid)
                Msg = "Sale No : " + oneObject.TicketNumber + " has been requested to be void ";
            else if (oneObject.SalesStatusID == (int)SalesStatusEnum.PendingEdit)
                Msg = "Sale No : " + oneObject.TicketNumber + " has been requested to be edit";
            else if (oneObject.SalesStatusID == (int)SalesStatusEnum.PendingDelete)
                Msg = "Sale No : " + oneObject.TicketNumber + " has been requested to be deleted ";

            param.Clear();
            param.Add(new SqlParameter("@Description", Msg));
            param.Add(new SqlParameter("@RelatedID", oneObject.ID));
            con.ExecSpNone("NotificationInsert", param);

            HubContext.Clients.All.NotificationFired(Msg);
            #endregion
            context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
        }
    }

    public static void SelectSalesHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesHistory>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new SqlParameter("@ID", oneObject.ID));
            if (oneObject.PaxName != null)
                param.Add(new SqlParameter("@PaxName", oneObject.PaxName));
            if (oneObject.PNR != null)
                param.Add(new SqlParameter("@PNR", oneObject.PNR));
            if (oneObject.DKNumber != null)
                param.Add(new SqlParameter("@DKNumber", oneObject.DKNumber));
            if (oneObject.AirlineID != null)
                param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
            if (oneObject.CreatedStaffID != null)
                param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
            if (oneObject.SalesTypeID != null)
                param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
            if (oneObject.DateFrom != null)
                param.Add(new SqlParameter("@FromDate", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new SqlParameter("@ToDate", oneObject.DateTo));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.SalesStatusID != null)
                param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));
            if (oneObject.TicketNumber != null)
                param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
            if (oneObject.HistoryCreatedStaffID != null)
                param.Add(new SqlParameter("@HistoryCreatedStaffID", oneObject.HistoryCreatedStaffID));
            if (oneObject.ApproveStatusID != null)
                param.Add(new SqlParameter("@ApproveStatusID", oneObject.ApproveStatusID));
        }
        param.Add(new SqlParameter("@BranchID", int.Parse(context.Request.QueryString["BranchID"])));
        param.Add(new SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("SalesHistorySelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void UpdateSalesHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesHistory>(jsonString);

        param.Clear();
        param.Add(new SqlParameter("@ID", oneObject.ID));
        param.Add(new SqlParameter("@SaleID", oneObject.SaleID));
        param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
        param.Add(new SqlParameter("@Destination", oneObject.Destination));
        param.Add(new SqlParameter("@PNR", oneObject.PNR));
        param.Add(new SqlParameter("@Fare", oneObject.Fare));
        param.Add(new SqlParameter("@Tax", oneObject.Tax));
        param.Add(new SqlParameter("@TotalCost", oneObject.TotalCost));
        param.Add(new SqlParameter("@SalesAmount", oneObject.SalesAmount));
        param.Add(new SqlParameter("@Profit", oneObject.Profit));
        param.Add(new SqlParameter("@DKNumber", oneObject.DKNumber));
        param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
        param.Add(new SqlParameter("@PaymentMethodID", oneObject.PaymentMethodID));
        param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
        param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
        param.Add(new SqlParameter("@PaxName", oneObject.PaxName));
        param.Add(new SqlParameter("@AccountID", oneObject.AccountID));

        if (oneObject.CardNumber != null)
            param.Add(new SqlParameter("@CardNumber", oneObject.CardNumber));
        if (oneObject.Remarks != null)
            param.Add(new SqlParameter("@Remarks", oneObject.Remarks));
        if (oneObject.Cash != null)
            param.Add(new SqlParameter("@Cash", oneObject.Cash));
        if (oneObject.Credit != null)
            param.Add(new SqlParameter("@Credit", oneObject.Credit));
        if (oneObject.Visa != null)
            param.Add(new SqlParameter("@Visa", oneObject.Visa));
        if (oneObject.Advance != null)
            param.Add(new SqlParameter("@Advance", oneObject.Advance));
        if (oneObject.Card != null)
            param.Add(new SqlParameter("@Card", oneObject.Card));
        if (oneObject.Complementary != null)
            param.Add(new SqlParameter("@Complementary", oneObject.Complementary));
        if (oneObject.Commision != null)
            param.Add(new SqlParameter("@Commision", oneObject.Commision));
        if (oneObject.InvoiceNumberPNR != null)
            param.Add(new SqlParameter("@InvoiceNumberPNR", oneObject.InvoiceNumberPNR));
        if (oneObject.SubAccountID != null)
            param.Add(new SqlParameter("@SubAccountID", oneObject.SubAccountID));
        if (oneObject.AccountantStaffID != null)
            param.Add(new SqlParameter("@AccountantStaffID", oneObject.AccountantStaffID));
        if (oneObject.BranchID != null)
            param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
        if (oneObject.RecievableMainAccountID != null)
            param.Add(new SqlParameter("@RecievableMainAccountID", oneObject.RecievableMainAccountID));
        if (oneObject.BankAccountID != null)
            param.Add(new SqlParameter("@BankAccountID", oneObject.BankAccountID));
        if (oneObject.CreditAccountID != null)
            param.Add(new SqlParameter("@CreditAccountID", oneObject.CreditAccountID));
        if (oneObject.SalesStatusID != null)
            param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));
        if (oneObject.ApproveStatusID != null)
            param.Add(new SqlParameter("@ApproveStatusID", oneObject.ApproveStatusID));
        if (oneObject.RejectionReason != null)
            param.Add(new SqlParameter("@RejectionReason", oneObject.RejectionReason));
        if (oneObject.RefDescription != null)
            param.Add(new SqlParameter("@RefDescription", oneObject.RefDescription));
        if (oneObject.RefRemarks != null)
            param.Add(new SqlParameter("@RefRemarks", oneObject.RefRemarks));
        if (oneObject.RefAmountFromProvider != null)
            param.Add(new SqlParameter("@RefAmountFromProvider", oneObject.RefAmountFromProvider));
        if (oneObject.RefAmountToCustomer != null)
            param.Add(new SqlParameter("@RefAmountToCustomer", oneObject.RefAmountToCustomer));
        if (oneObject.CreationDate != null)
            param.Add(new SqlParameter("@CreationDate", oneObject.CreationDate));

        con.ExecSpNone("SalesHistoryUpdate", param);

        if (oneObject.ApproveStatusID == 33 && oneObject.SalesStatusID == (int)SalesStatusEnum.PendingEdit)
        {
            //remove all movment and jouranl related sales
            var SaleID = oneObject.SaleID;

            param.Clear();
            param.Add(new SqlParameter("@SalesID", SaleID));
            param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
            param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));
            param.Add(new SqlParameter("@JournalTypeID", JournalTypeEnum.SysJV));
            param.Add(new SqlParameter("@StaffID", oneObject.AccountantStaffID));

            DataTable dtJournal = con.ExecSpSelect("JournalInsert", param);

            var JournalID = int.Parse(dtJournal.Rows[0]["Identity"].ToString());

            param.Clear();
            param.Add(new SqlParameter("@ID", SaleID));
            param.Add(new SqlParameter("@PageNumber", 1));
            param.Add(new SqlParameter("@PageSize", 1));
            DataTable dtSalte = con.ExecSpSelect("SalesSelect", param);
            var IssuedByName = dtSalte.Rows[0]["CreatedName"].ToString();
            var PaymentMethodName = dtSalte.Rows[0]["PaymentMethodTitle"].ToString();
            var SalesTypeName = dtSalte.Rows[0]["SalesTypeTitle"].ToString();

            //Credit-Cost-Not BSP
            if (oneObject.SalesTypeID != (int)SalesTypeEnum.BSP)
                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, 0, oneObject.TotalCost, oneObject.SubAccountID);

            param.Clear();
            param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
            DataTable dtMapping = con.ExecSpSelect("SalesMappingAccountSelect", param);
            foreach (DataRow row in dtMapping.Rows)
            {
                if (int.Parse(row["DebitCredit"].ToString()) == 1 && int.Parse(row["SalesCost"].ToString()) == 1 && oneObject.SalesTypeID == (int)SalesTypeEnum.BSP)
                {
                    AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, 0,
                        oneObject.TotalCost,
                        int.Parse(row["AccountID"].ToString()));
                }
                else if (int.Parse(row["DebitCredit"].ToString()) == 0 && int.Parse(row["SalesCost"].ToString()) == 1)
                {
                    AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.TotalCost,
                        0, int.Parse(row["AccountID"].ToString()));
                }
                else if (int.Parse(row["DebitCredit"].ToString()) == 1 && int.Parse(row["SalesCost"].ToString()) == 0)
                {
                    AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, 0,
                        oneObject.SalesAmount, int.Parse(row["AccountID"].ToString()));
                }
            }


            if (oneObject.Cash != null && oneObject.Cash > 0)
            {
                param.Clear();
                param.Add(new SqlParameter("@PaymentMethodID", PaymentMethodEnum.Cash));
                DataTable dtMappingCash = con.ExecSpSelect("SalesMappingAccountPaymentMethodSelect", param);

                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.Cash,
                    0, int.Parse(dtMappingCash.Rows[0]["AccountID"].ToString()));
            }
            if (oneObject.Card != null && oneObject.Card > 0)
            {
                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.Card,
                    0, oneObject.BankAccountID);
            }
            if (oneObject.Credit != null && oneObject.Credit > 0)
            {
                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.Credit,
                    0, oneObject.CreditAccountID);
            }
            if (oneObject.Visa != null && oneObject.Visa > 0)
            {
                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.Visa,
                    0, oneObject.BankAccountID);
            }
            if (oneObject.Advance != null && oneObject.Advance > 0)
            {
                param.Clear();
                param.Add(new SqlParameter("@PaymentMethodID", PaymentMethodEnum.From_Advanced_Recieved));
                DataTable dtMappingAdvance = con.ExecSpSelect("SalesMappingAccountPaymentMethodSelect", param);

                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.Advance,
                    0, int.Parse(dtMappingAdvance.Rows[0]["AccountID"].ToString()));
            }
            if (oneObject.Complementary != null && oneObject.Complementary > 0)
            {
                param.Clear();
                param.Add(new SqlParameter("@PaymentMethodID", PaymentMethodEnum.Complementary));
                DataTable dtMappingComplementary = con.ExecSpSelect("SalesMappingAccountPaymentMethodSelect", param);

                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.Complementary,
                    0, int.Parse(dtMappingComplementary.Rows[0]["AccountID"].ToString()));
            }
        }

        #region Notification
        param.Clear();
        param.Add(new SqlParameter("@RelatedID", oneObject.SaleID));
        con.ExecSpNone("NotificationUpdate", param);
        HubContext.Clients.All.NotificationCountDown("Notification Count Down");
        #endregion

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void DeleteSalesHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        param.Add(new SqlParameter("@ID", int.Parse(context.Request.QueryString["ID"])));
        con.ExecSpNone("SalesHistoryDelete", param);
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    private static void AddMovement(ArrayList param, int JournalID, SalesHistory oneObject, string SalesTypeName,
       string IssuedByName, string PaymentMethodName, float? Debit, float? Credit, int? Account)
    {
        DatabaseConnection con = new DatabaseConnection();
        param.Clear();
        param.Add(new SqlParameter("@JournalID", JournalID));
        param.Add(new SqlParameter("@AccountID", Account));
        param.Add(new SqlParameter("@Debit", Debit));
        param.Add(new SqlParameter("@Credit", Credit));
        param.Add(new SqlParameter("@MovementDescription",
            SalesTypeName + " - T/V: " + oneObject.TicketNumber + " - Issued By : " + IssuedByName + " - " +
            PaymentMethodName + " - PaxName : " + oneObject.PaxName));

        con.ExecSpNone("JournalMovementInsert", param);
    }
}