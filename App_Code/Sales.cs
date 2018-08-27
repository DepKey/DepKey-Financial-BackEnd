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
public class Sales
{

    private static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<SalesHub>();

    public Sales()
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
    public int? RecievableMainAccountID { get; set; }
    public int? CreditAccountID { get; set; }
    public int? BranchID { get; set; }
    public int? SalesStatusID { get; set; }

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? RefPaid { get; set; }

    public int? ParentID { get; set; }

    public float? RefAmountFromProvider { get; set; }
    public float? RefAmountToCustomer { get; set; }
    public string RefRemarks { get; set; }
    public string RefDescription { get; set; }
    public string Vendor { get; set; }

    public static void InsertSales(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonString);

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
        param.Add(new SqlParameter("@AccountID  ", oneObject.AccountID));

        if (oneObject.CardNumber != null)
            param.Add(new SqlParameter("@CardNumber", oneObject.CardNumber));
        if (oneObject.CardNumber != null)
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
            param.Add(new SqlParameter("@BranchID  ", oneObject.BranchID));
        if (oneObject.RecievableMainAccountID != null)
            param.Add(new SqlParameter("@RecievableMainAccountID  ", oneObject.RecievableMainAccountID));
        if (oneObject.BankAccountID != null)
            param.Add(new SqlParameter("@BankAccountID  ", oneObject.BankAccountID));
        if (oneObject.CreditAccountID != null)
            param.Add(new SqlParameter("@CreditAccountID  ", oneObject.CreditAccountID));
        if (oneObject.PaymentMethodID != (int)PaymentMethodEnum.Complementary)
            param.Add(new SqlParameter("@SalesStatusID", SalesStatusEnum.Transfered));
        else
            param.Add(new SqlParameter("@SalesStatusID", SalesStatusEnum.PendingComplementary));

        DataTable dt = con.ExecSpSelect("SalesInsert", param);
        var TicketExist = dt.Rows[0]["TicketExist"].ToString();
        if (int.Parse(TicketExist) == 1)
        {
            string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"TicketExist\":" + TicketExist + "}";
            context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
        }
        else
        {
            var SaleID = int.Parse(dt.Rows[0]["Identity"].ToString());
            var InvoiceNumber = int.Parse(dt.Rows[0]["InvoiceNumberPNR"].ToString());

            param.Clear();
            param.Add(new SqlParameter("@SalesID", SaleID));
            param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
            param.Add(new SqlParameter("@SalesStatusID  ", oneObject.SalesStatusID));
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

            param.Clear();
            param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
            DataTable dtMapping = con.ExecSpSelect("SalesMappingAccountSelect", param);

            //Credit-Cost-Not BSP
            if (oneObject.SalesTypeID != (int)SalesTypeEnum.BSP)
                AddMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, 0, oneObject.TotalCost, oneObject.SubAccountID);

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

            string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
            context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
        }
    }

    public static void TransferSales(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonString);

        param.Add(new SqlParameter("@ID", oneObject.ID));
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
        param.Add(new SqlParameter("@AccountID  ", oneObject.AccountID));
        param.Add(new SqlParameter("@CreationDate", oneObject.CreationDate));

        if (oneObject.CardNumber != null)
            param.Add(new SqlParameter("@CardNumber", oneObject.CardNumber));
        if (oneObject.CardNumber != null)
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
        else
            param.Add(new SqlParameter("@Commision", 0));
        if (oneObject.SubAccountID != null)
            param.Add(new SqlParameter("@SubAccountID", oneObject.SubAccountID));
        if (oneObject.AccountantStaffID != null)
            param.Add(new SqlParameter("@AccountantStaffID", oneObject.AccountantStaffID));
        if (oneObject.BranchID != null)
            param.Add(new SqlParameter("@BranchID  ", oneObject.BranchID));
        if (oneObject.PaymentMethodID != (int)PaymentMethodEnum.Complementary)
            param.Add(new SqlParameter("@SalesStatusID", SalesStatusEnum.Transfered));
        else
            param.Add(new SqlParameter("@SalesStatusID", SalesStatusEnum.PendingComplementary));

        con.ExecSpNone("SalesUpdate", param);

        var SaleID = oneObject.ID;

        param.Clear();
        param.Add(new System.Data.SqlClient.SqlParameter("@SalesID", SaleID));
        param.Add(new System.Data.SqlClient.SqlParameter("@BranchID", oneObject.BranchID));
        param.Add(new System.Data.SqlClient.SqlParameter("@SalesStatusID  ", SalesStatusEnum.Transfered));
        param.Add(new System.Data.SqlClient.SqlParameter("@JournalTypeID", JournalTypeEnum.SysJV));
        param.Add(new System.Data.SqlClient.SqlParameter("@StaffID", oneObject.AccountantStaffID));
        param.Add(new System.Data.SqlClient.SqlParameter("@JournalDate", oneObject.CreationDate));

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

        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"}}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void InsertSalesAfterRefund(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonString);

        param.Add(new SqlParameter("@ID", oneObject.ID));
        param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
        param.Add(new SqlParameter("@CardNumber", oneObject.CardNumber));
        param.Add(new SqlParameter("@PaxName", oneObject.PaxName));
        param.Add(new SqlParameter("@Remarks", oneObject.Remarks));
        param.Add(new SqlParameter("@Destination", oneObject.Destination));
        param.Add(new SqlParameter("@PNR", oneObject.PNR));
        param.Add(new SqlParameter("@Fare", oneObject.Fare));
        param.Add(new SqlParameter("@Tax", oneObject.Tax));
        param.Add(new SqlParameter("@TotalCost", oneObject.TotalCost));
        param.Add(new SqlParameter("@SalesAmount", oneObject.SalesAmount));
        param.Add(new SqlParameter("@Profit", oneObject.Profit));
        param.Add(new SqlParameter("@Cash", oneObject.Cash));
        param.Add(new SqlParameter("@Credit", oneObject.Credit));
        param.Add(new SqlParameter("@Visa", oneObject.Visa));
        param.Add(new SqlParameter("@Advance", oneObject.Advance));
        param.Add(new SqlParameter("@Card", oneObject.Card));
        param.Add(new SqlParameter("@Complementary", oneObject.Complementary));
        param.Add(new SqlParameter("@Commision", oneObject.Commision));
        param.Add(new SqlParameter("@DKNumber", oneObject.DKNumber));
        param.Add(new SqlParameter("@InvoiceNumberPNR", oneObject.InvoiceNumberPNR));
        param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
        param.Add(new SqlParameter("@PaymentMethodID", oneObject.PaymentMethodID));
        param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
        param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
        param.Add(new SqlParameter("@AccountantStaffID", oneObject.AccountantStaffID));
        param.Add(new SqlParameter("@SalesStatusID", SalesStatusEnum.Refunded));
        param.Add(new SqlParameter("@AccountID  ", oneObject.AccountID));
        param.Add(new SqlParameter("@SubAccountID  ", oneObject.SubAccountID));
        param.Add(new SqlParameter("@BranchID  ", oneObject.BranchID));
        param.Add(new SqlParameter("@RefAmountFromProvider  ", oneObject.RefAmountFromProvider));
        param.Add(new SqlParameter("@RefDescription  ", oneObject.RefDescription));
        param.Add(new SqlParameter("@RefPaid", false));
        param.Add(new SqlParameter("@CreationDate", oneObject.CreationDate));
        con.ExecSpNone("SalesUpdate", param);

        if (oneObject.SalesStatusID == (int)SalesStatusEnum.Refunded)
        {
            param.Clear();
            param.Add(new SqlParameter("@JournalTypeID", JournalTypeEnum.SysJV));
            param.Add(new SqlParameter("@BranchID", oneObject.BranchID));
            param.Add(new SqlParameter("@StaffID", oneObject.CreatedStaffID));
            param.Add(new SqlParameter("@SalesID", oneObject.ID));
            param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));

            DataTable dtJournal = con.ExecSpSelect("JournalInsert", param);
            var JournalID = int.Parse(dtJournal.Rows[0]["Identity"].ToString());

            param.Clear();

            param.Add(new SqlParameter("@ID", oneObject.ID));
            DataTable dtSalteSelect = con.ExecSpSelect("SalesSelect", param);
            var RefundSalesTypeID = dtSalteSelect.Rows[0]["RelatedID"].ToString();
            var AccountID = dtSalteSelect.Rows[0]["AccountID"].ToString();
            var SubAccountID = dtSalteSelect.Rows[0]["SubAccountID"].ToString();
            param.Clear();
            param.Add(new SqlParameter("@SalesTypeID", RefundSalesTypeID));
            DataTable dtMapping = con.ExecSpSelect("SalesMappingAccountSelect", param);

            param.Clear();
            param.Add(new SqlParameter("@ID", oneObject.ID));
            param.Add(new SqlParameter("@PageNumber", 1));
            param.Add(new SqlParameter("@PageSize", 1));

            DataTable dtSalte = con.ExecSpSelect("SalesSelect", param);
            var IssuedByName = dtSalte.Rows[0]["CreatedName"].ToString();
            var PaymentMethodName = dtSalte.Rows[0]["PaymentMethodTitle"].ToString();
            var SalesTypeName = dtSalte.Rows[0]["SalesTypeTitle"].ToString();

            //BSP => main account from sales
            if (int.Parse(RefundSalesTypeID) == (int)SalesTypeEnum.RefundBSP)
                AddRefundMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.RefAmountFromProvider, 0, int.Parse(AccountID));

            // not BSP sub account form sales
            if (int.Parse(RefundSalesTypeID) != (int)SalesTypeEnum.RefundBSP)
                AddRefundMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, oneObject.RefAmountFromProvider, 0, int.Parse(SubAccountID.ToString()));

            foreach (DataRow row in dtMapping.Rows)
            {
                if (int.Parse(RefundSalesTypeID) == (int)SalesTypeEnum.RefundBSP)
                {
                    AddRefundMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, 0,
                        oneObject.RefAmountFromProvider,
                        int.Parse(row["AccountID"].ToString()));
                }
                else if (int.Parse(RefundSalesTypeID) != (int)SalesTypeEnum.RefundBSP)
                {
                    AddRefundMovement(param, JournalID, oneObject, SalesTypeName, IssuedByName, PaymentMethodName, 0,
                        oneObject.RefAmountFromProvider, int.Parse(row["AccountID"].ToString()));
                }
            }
        }

        #region New Sale With ParentID

        param.Clear();

        param.Add(new SqlParameter("@ParentID", oneObject.ID));
        param.Add(new SqlParameter("@TicketNumber", "Ref - " + oneObject.TicketNumber));
        param.Add(new SqlParameter("@CardNumber", oneObject.CardNumber));
        param.Add(new SqlParameter("@PaxName", oneObject.PaxName));
        param.Add(new SqlParameter("@Remarks", oneObject.Remarks));
        param.Add(new SqlParameter("@Destination", oneObject.Destination));
        param.Add(new SqlParameter("@PNR", oneObject.PNR));
        param.Add(new SqlParameter("@Fare", 0));
        param.Add(new SqlParameter("@Tax", 0));
        param.Add(new SqlParameter("@TotalCost", oneObject.RefAmountFromProvider * -1));
        param.Add(new SqlParameter("@SalesAmount", oneObject.SalesAmount));
        param.Add(new SqlParameter("@Profit", oneObject.RefAmountFromProvider));
        param.Add(new SqlParameter("@Cash", 0));
        param.Add(new SqlParameter("@Credit", 0));
        param.Add(new SqlParameter("@Visa", 0));
        param.Add(new SqlParameter("@Advance", 0));
        param.Add(new SqlParameter("@Card", 0));
        param.Add(new SqlParameter("@Complementary", 0));
        param.Add(new SqlParameter("@Commision", 0));
        param.Add(new SqlParameter("@DKNumber", oneObject.DKNumber));
        param.Add(new SqlParameter("@InvoiceNumberPNR", oneObject.InvoiceNumberPNR));
        param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
        param.Add(new SqlParameter("@PaymentMethodID", oneObject.PaymentMethodID));
        param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
        param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
        param.Add(new SqlParameter("@AccountantStaffID", oneObject.AccountantStaffID));
        param.Add(new SqlParameter("@SalesStatusID", SalesStatusEnum.Refunded));
        param.Add(new SqlParameter("@AccountID  ", oneObject.AccountID));
        param.Add(new SqlParameter("@BranchID  ", oneObject.BranchID));
        param.Add(new SqlParameter("@RefAmountFromProvider  ", oneObject.RefAmountFromProvider));
        param.Add(new SqlParameter("@RefDescription  ", oneObject.RefDescription));
        param.Add(new SqlParameter("@RefPaid", false));

        con.ExecSpNone("SalesInsert", param);

        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"}}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));

        #endregion
    }

    public static void SelectSales(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonString);

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
                param.Add(new SqlParameter("@DateFrom", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new SqlParameter("@DateTo", oneObject.DateTo));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.SalesStatusID != null)
                param.Add(new SqlParameter("@SalesStatusID", oneObject.SalesStatusID));
            if (oneObject.TicketNumber != null)
                param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
            if (oneObject.RefPaid != null)
                param.Add(new SqlParameter("@RefPaid", oneObject.RefPaid));
            if (oneObject.InvoiceNumberPNR != null)
                param.Add(new SqlParameter("@InvoiceNumberPNR", oneObject.InvoiceNumberPNR));
        }

        param.Add(new SqlParameter("@BranchID", int.Parse(context.Request.QueryString["BranchID"])));
        param.Add(new SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("SalesSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }

    public static void SelectSalesForAirlineReport(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Sales>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.AirlineID != null)
                param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
            if (oneObject.CreatedStaffID != null)
                param.Add(new SqlParameter("@StaffID", oneObject.CreatedStaffID));
            if (oneObject.DateFrom != null)
                param.Add(new SqlParameter("@DateFrom", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new SqlParameter("@DateTo", oneObject.DateTo));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));

        }

        DataTable dt = con.ExecSpSelect("RptSalesSelectForAirLine", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectSalesForTotalAirlineReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));

        }

        DataTable dt = con.ExecSpSelect("RptSalesSelectForTotalAirLine", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectSalesPerPNRReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if(oneObject.PNR != string.Empty)
                param.Add(new SqlParameter("@PNR", oneObject.PNR));
            if (oneObject.SalesTypeID != null)
                param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
        }

        DataTable dt = con.ExecSpSelect("RptSalesPerPNRReport", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectSalesForSalesReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.CreatedStaffID != null)
                param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
            if (oneObject.SalesTypeID != null)
                param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
            if (oneObject.AirlineID != null)
                param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
            if (oneObject.TicketNumber != null)
                param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
            if (oneObject.AccountID != null)
                param.Add(new SqlParameter("@AccountID", oneObject.AccountID));
            if (oneObject.SubAccountID != null)
                param.Add(new SqlParameter("@SubAccountID", oneObject.SubAccountID));
        }

        DataTable dt = con.ExecSpSelect("RptSalesSelectForSales", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }
    public static void SelectSalesForDailySalesReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.CreatedStaffID != null)
                param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));
            if (oneObject.SalesTypeID != null)
                param.Add(new SqlParameter("@SalesTypeID", oneObject.SalesTypeID));
            if (oneObject.AirlineID != null)
                param.Add(new SqlParameter("@AirlineID", oneObject.AirlineID));
            if (oneObject.TicketNumber != null)
                param.Add(new SqlParameter("@TicketNumber", oneObject.TicketNumber));
            if (oneObject.Vendor != null)
                param.Add(new SqlParameter("@Vendor", oneObject.Vendor));
        }

        DataTable dt = con.ExecSpSelect("RptSalesSelectForDailySales", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectSalesForExpesesReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));

            param.Add(new SqlParameter("@ExpensesORIncome", '4'));

        }
        DataTable dt = con.ExecSpSelect("RptSalesSelectForExpensesOrIncome", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectSalesForIncomeReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));

            param.Add(new SqlParameter("@ExpensesORIncome", '3'));

        }
        DataTable dt = con.ExecSpSelect("RptSalesSelectForExpensesOrIncome", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void SelectSalesForProfitReport(HttpContext context)
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
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));

        }
        DataTable dt = con.ExecSpSelect("RptSalesSelectForProfit", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }


    public static void SelectSalesForStaffTotalSalesReport(HttpContext context)
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
            if (oneObject.CreatedStaffID != null)
                param.Add(new SqlParameter("@CreatedStaffID", oneObject.CreatedStaffID));

        }
        DataTable dt = con.ExecSpSelect("RptSalesSelectForStaffTotalSalesReport", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    private static void AddMovement(ArrayList param, int JournalID, Sales oneObject, string SalesTypeName,
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

    private static void AddRefundMovement(ArrayList param, int JournalID, Sales oneObject, string SalesTypeName,
       string IssuedByName, string PaymentMethodName, float? Debit, float? Credit, int? Account)
    {
        DatabaseConnection con = new DatabaseConnection();
        param.Clear();
        param.Add(new SqlParameter("@JournalID", JournalID));
        param.Add(new SqlParameter("@AccountID", Account));
        param.Add(new SqlParameter("@Debit", Debit));
        param.Add(new SqlParameter("@Credit", Credit));
        param.Add(new SqlParameter("@MovementDescription",
            " - Refund " + SalesTypeName + " - T/V: " + oneObject.TicketNumber + " - Issued By : " + IssuedByName + " - " +
            PaymentMethodName + " - PaxName : " + oneObject.PaxName));

        con.ExecSpNone("JournalMovementInsert", param);
    }

}