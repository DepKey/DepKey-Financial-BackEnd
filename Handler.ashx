<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Linq;
using System.Web;

public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        #region LookupTitle
        if (context.Request.QueryString["action"] == "InsertLookupType")
            LookupType.InsertLookupType(context);
        else if (context.Request.QueryString["action"] == "UpdateLookupType")
            LookupType.UpdateLookupType(context);
        else if (context.Request.QueryString["action"] == "SelectLookupType")
            LookupType.SelectLookupType(context);
        else if (context.Request.QueryString["action"] == "DeleteLookupType")
            LookupType.DeleteLookupType(context);
        #endregion

        #region Lockup
        else if (context.Request.QueryString["action"] == "InsertLookup")
            Lookup.InsertLookup(context);
        else if (context.Request.QueryString["action"] == "UpdateLookup")
            Lookup.UpdateLookup(context);
        else if (context.Request.QueryString["action"] == "SelectLookup")
            Lookup.SelectLookup(context);
        else if (context.Request.QueryString["action"] == "DeleteLookup")
            Lookup.DeleteLookup(context);
        #endregion

        #region Staff
        else if (context.Request.QueryString["action"] == "InsertStaff")
            Staff.InsertStaff(context);
        else if (context.Request.QueryString["action"] == "UpdateStaff")
            Staff.UpdateStaff(context);
        else if (context.Request.QueryString["action"] == "SelectStaff")
            Staff.SelectStaff(context);
        else if (context.Request.QueryString["action"] == "DeleteStaff")
            Staff.DeleteStaff(context);
        #endregion

        #region Branch
        else if (context.Request.QueryString["action"] == "SelectBranch")
            Branch.SelectBranch(context);
        #endregion

        #region Account
        else if (context.Request.QueryString["action"] == "InsertAccount")
            Account.InsertAccount(context);
        else if (context.Request.QueryString["action"] == "UpdateAccount")
            Account.UpdateAccount(context);
        else if (context.Request.QueryString["action"] == "SelectAccount")
            Account.SelectAccount(context);
        else if (context.Request.QueryString["action"] == "SelectAccountDC")
            Account.SelectAccountDC(context);
        else if (context.Request.QueryString["action"] == "SelectAccountWithSub")
            Account.SelectAccountWithSub(context);
        else if (context.Request.QueryString["action"] == "DeleteAccount")
            Account.DeleteAccount(context);
        else if (context.Request.QueryString["action"] == "SelectAccountsForTrialBalanceReport")
            Account.SelectAccountsForTrialBalanceReport(context);
        #endregion

        #region Airline
        else if (context.Request.QueryString["action"] == "SelectAirline")
            Airline.SelectAirline(context);
        #endregion

        #region Journal
        else if (context.Request.QueryString["action"] == "InsertJournal")
            Journal.InsertJournal(context);
        else if (context.Request.QueryString["action"] == "UpdateJournal")
            Journal.UpdateJournal(context);
        else if (context.Request.QueryString["action"] == "SelectJournal")
            Journal.SelectJournal(context);
        else if (context.Request.QueryString["action"] == "DeleteJournal")
            Journal.DeleteJournal(context);
        #endregion

        #region Movement
        else if (context.Request.QueryString["action"] == "SelectMovement")
            Movement.SelectMovement(context);
        else if (context.Request.QueryString["action"] == "UpdateMovement")
            Movement.UpdateMovement(context);
        #endregion

        #region PaymentMethodAccount
        else if (context.Request.QueryString["action"] == "SelectSalesMappingAccountPaymentMethod")
            SalesMappingAccountPaymentMethod.SelectSalesMappingAccountPaymentMethod(context);
        #endregion

        #region Sale
        else if (context.Request.QueryString["action"] == "InsertSales")
            Sales.InsertSales(context);
        else if (context.Request.QueryString["action"] == "SelectSales")
            Sales.SelectSales(context);
        else if (context.Request.QueryString["action"] == "TransferSales")
            Sales.TransferSales(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForAirlineReport")
            Sales.SelectSalesForAirlineReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForTotalAirlineReport")
            Sales.SelectSalesForTotalAirlineReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesPerPNRReport")
            Sales.SelectSalesPerPNRReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForDailySalesReport")
            Sales.SelectSalesForDailySalesReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForSalesReport")
            Sales.SelectSalesForSalesReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForExpesesReport")
            Sales.SelectSalesForExpesesReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForIncomeReport")
            Sales.SelectSalesForIncomeReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForProfitReport")
            Sales.SelectSalesForProfitReport(context);
        else if (context.Request.QueryString["action"] == "SelectSalesForStaffTotalSalesReport")
            Sales.SelectSalesForStaffTotalSalesReport(context);
        else if (context.Request.QueryString["action"] == "InsertSalesAfterRefund")
            Sales.InsertSalesAfterRefund(context);
        #endregion

        #region Staff
        else if (context.Request.QueryString["action"] == "SelectStaff")
            Staff.SelectStaff(context);
        #endregion

        #region SalesHistory
        else if (context.Request.QueryString["action"] == "InsertSalesHistory")
            SalesHistory.InsertSalesHistory(context);
        else if (context.Request.QueryString["action"] == "SelectSalesHistory")
            SalesHistory.SelectSalesHistory(context);
        else if (context.Request.QueryString["action"] == "UpdateSalesHistory")
            SalesHistory.UpdateSalesHistory(context);
        else if (context.Request.QueryString["action"] == "DeleteSalesHistory")
            SalesHistory.DeleteSalesHistory(context);
        #endregion

        #region Notification
        else if (context.Request.QueryString["action"] == "CountNotification")
            Notification.CountNotification(context);
        else if (context.Request.QueryString["action"] == "SelectNotification")
            Notification.SelectNotification(context);
        #endregion

        #region MovementHistory
        else if (context.Request.QueryString["action"] == "InsertMovementHistory")
            JournalMovementHistory.InsertMovementHistory(context);
        else if (context.Request.QueryString["action"] == "SelectMovementHistory")
            JournalMovementHistory.SelectMovementHistory(context);
        else if (context.Request.QueryString["action"] == "UpdateMovementHistory")
            JournalMovementHistory.UpdateMovementHistory(context);
         else if (context.Request.QueryString["action"] == "InsertJournalHistory")
            JournalMovementHistory.InsertJournalHistory(context);
        #endregion

        #region Outstanding
        else if (context.Request.QueryString["action"] == "InsertOutstanding")
            Outstanding.InsertOutstanding(context);
        else if (context.Request.QueryString["action"] == "SelectOutstanding")
            Outstanding.SelectOutstanding(context);
        else if (context.Request.QueryString["action"] == "SelectOutstandingPartial")
            Outstanding.SelectOutstandingPartial(context);
        else if (context.Request.QueryString["action"] == "OutStandingSelectMaxOsNumber")
            Outstanding.OutStandingSelectMaxOsNumber(context);
        #endregion

        #region Log
        else if (context.Request.QueryString["action"] == "InsertLog")
            Log.InsertLog(context);
        else if (context.Request.QueryString["action"] == "SelectLog")
            Log.SelectLog(context);
        #endregion

        #region Dashboard
        else if (context.Request.QueryString["action"] == "DashboardGetProfit")
            Dashboard.DashboardGetProfit(context);
        else if (context.Request.QueryString["action"] == "DashboardGetStaffAchievements")
            Dashboard.DashboardGetStaffAchievements(context);
        else if (context.Request.QueryString["action"] == "DashboardGetSalesCounts")
            Dashboard.DashboardGetSalesCounts(context);
        else if (context.Request.QueryString["action"] == "DashboardGetSalesStatistics")
            Dashboard.DashboardGetSalesStatistics(context);
        #endregion

        #region Sales Group
        else if (context.Request.QueryString["action"] == "InsertSalesGroup")
            SalesGroup.InsertSalesGroup(context);
        else if (context.Request.QueryString["action"] == "InsertSalesGroupByID")
            SalesGroup.InsertSalesGroupByID(context);
        else if (context.Request.QueryString["action"] == "SelectSalesGroup")
            SalesGroup.SelectSalesGroup(context);
        else if (context.Request.QueryString["action"] == "DeleteSalesGroup")
            SalesGroup.DeleteSalesGroup(context);
        #endregion
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}