using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for JournalMovementHistory
/// </summary>
public class JournalMovementHistory
{
    private static IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<SalesHub>();

    public JournalMovementHistory()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreationDate { get; set; }
    public double? Debit { get; set; }
    public double? Credit { get; set; }
    public string MovementDescription { get; set; }
    public int? JournalID { get; set; }
    public int? JournalMovementID { get; set; }
    public int? AccountID { get; set; }
    public int? ApprovalStatusID { get; set; }
    public string RejectionReason { get; set; }

    List<JournalMovementHistory> MovementHistoryList = new List<JournalMovementHistory>();

    public static void InsertMovementHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var MovementList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JournalMovementHistory>>(jsonString);

        if (MovementList != null)
        {
            foreach (var item in MovementList)
            {
                param.Clear();
                param.Add(new SqlParameter("@JournalID", item.JournalID));
                param.Add(new SqlParameter("@ApprovalStatusID", 32)); //Pending
                param.Add(new SqlParameter("@JournalMovementID", item.ID));
                if (item.AccountID != null)
                    param.Add(new SqlParameter("@AccountID", item.AccountID));
                if (item.MovementDescription != null)
                    param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                if (item.Debit != null)
                    param.Add(new SqlParameter("@Debit", item.Debit));
                if (item.Credit != null)
                    param.Add(new SqlParameter("@Credit", item.Credit));
                if (item.IsDeleted != null)
                    param.Add(new SqlParameter("@IsDeleted", item.IsDeleted));

                con.ExecSpNone("JournalMovementHistoryInsert", param);
            }
        }

        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }

    public static void SelectMovementHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Movement>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.AccountID != null)
                param.Add(new SqlParameter("@AccountID", oneObject.AccountID));
            if (oneObject.MovementDescription != null)
                param.Add(new SqlParameter("@MovementDescription", oneObject.MovementDescription));
            if (oneObject.DateFrom != null)
                param.Add(new SqlParameter("@DateFrom", oneObject.DateFrom));
            if (oneObject.DateTo != null)
                param.Add(new SqlParameter("@DateTo", oneObject.DateTo));
            if (oneObject.IsDeleted != null)
                param.Add(new SqlParameter("@IsDeleted", oneObject.IsDeleted));
            if (oneObject.JournalNumber != null)
                param.Add(new SqlParameter("@JournalNumber", oneObject.JournalNumber));
            if (oneObject.JournalID != null)
                param.Add(new SqlParameter("@JournalID", oneObject.JournalID));
        }

        DataTable dt = con.ExecSpSelect("JournalMovementHistorySelect", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void UpdateMovementHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var newObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JournalMovementHistory>>(jsonString);
        if (newObject != null)
        {
            //select old movments
            param.Add(new SqlParameter("@JournalID", newObject[0].JournalID));
            DataTable dt = con.ExecSpSelect("JournalMovementSelect", param);
            string jsonstring = con.DataTableToJson(dt);
            var OldSaleObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JournalMovement>>(jsonstring);

            if (int.Parse(context.Request.QueryString["ApprovalStatusID"]) == 33)
            {
                if (OldSaleObject.Count != 0)
                {
                    param.Clear();
                    param.Add(new SqlParameter("@JournalID", newObject[0].JournalID));
                    con.ExecSpNone("JournalMovementAndHistoryDelete", param);
                }
                else
                {
                    foreach (var item in newObject)
                    {
                        param.Clear();
                        param.Add(new SqlParameter("@ApprovalStatusID", int.Parse(context.Request.QueryString["ApprovalStatusID"])));
                        if (item.AccountID != null)
                            param.Add(new SqlParameter("@AccountID", item.AccountID));
                        if (item.MovementDescription != null)
                            param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                        if (item.Debit != null)
                            param.Add(new SqlParameter("@Debit", item.Debit));
                        if (item.Credit != null)
                            param.Add(new SqlParameter("@Credit", item.Credit));
                        if (item.ID != null)
                            param.Add(new SqlParameter("@ID", item.ID));
                        if (item.IsDeleted != null)
                            param.Add(new SqlParameter("@IsDeleted", item.IsDeleted));
                        if (item.JournalID != null)
                            param.Add(new SqlParameter("@JournalID", item.JournalID));

                        con.ExecSpNone("JournalMovementHistoryUpdate", param);
                    }
                }
                //Insert Movments With new Data
                if (newObject != null)
                {
                    foreach (var item in newObject)
                    {
                        param.Clear();
                        if (item.AccountID != null)
                            param.Add(new SqlParameter("@AccountID", item.AccountID));
                        if (item.MovementDescription != null)
                            param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                        if (item.Debit != null)
                            param.Add(new SqlParameter("@Debit", item.Debit));
                        if (item.Credit != null)
                            param.Add(new SqlParameter("@Credit", item.Credit));
                        if (item.JournalID != null)
                            param.Add(new SqlParameter("@JournalID", item.JournalID));
                        con.ExecSpNone("JournalMovementInsert", param);
                    }
                }
                //select New movments ID
                param.Clear();
                param.Add(new SqlParameter("@JournalID", newObject[0].JournalID));
                DataTable dtNewID = con.ExecSpSelect("JournalMovementSelect", param);
                string jsonstringNewID = con.DataTableToJson(dtNewID);
                var NewIDList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JournalMovement>>(jsonstringNewID);
                //Insert history with old data
                if (OldSaleObject != null)
                {
                    for (int i = 0; i < OldSaleObject.Count; i++)
                    {
                        param.Clear();
                        param.Add(new SqlParameter("@ApprovalStatusID", int.Parse(context.Request.QueryString["ApprovalStatusID"])));
                        if (OldSaleObject[i].AccountID != null)
                            param.Add(new SqlParameter("@AccountID", OldSaleObject[i].AccountID));
                        if (OldSaleObject[i].MovementDescription != null)
                            param.Add(new SqlParameter("@MovementDescription", OldSaleObject[i].MovementDescription));
                        if (OldSaleObject[i].Debit != null)
                            param.Add(new SqlParameter("@Debit", OldSaleObject[i].Debit));
                        if (OldSaleObject[i].Credit != null)
                            param.Add(new SqlParameter("@Credit", OldSaleObject[i].Credit));
                        if (OldSaleObject[i].ID != null)
                            param.Add(new SqlParameter("@JournalMovementID", NewIDList[i].ID));
                        if (OldSaleObject[i].IsDeleted != null)
                            param.Add(new SqlParameter("@IsDeleted", OldSaleObject[i].IsDeleted));
                        if (OldSaleObject[i].JournalID != null)
                            param.Add(new SqlParameter("@JournalID", OldSaleObject[i].JournalID));

                        con.ExecSpNone("JournalMovementHistoryInsert", param);
                    }
                }
                if (newObject[0].IsDeleted == true)
                {
                    param.Add(new SqlParameter("@ID", newObject[0].JournalID));
                    con.ExecSpNone("JournalDelete", param);
                }
                context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
            }
            else
            {
                //update history => rejected
                if (newObject != null)
                {
                    foreach (var item in newObject)
                    {
                        param.Clear();
                        param.Add(new SqlParameter("@ApprovalStatusID", int.Parse(context.Request.QueryString["ApprovalStatusID"])));
                        param.Add(new SqlParameter("@RejectionReason", context.Request.QueryString["RejectionReason"].ToString()));

                        if (item.AccountID != null)
                            param.Add(new SqlParameter("@AccountID", item.AccountID));
                        if (item.MovementDescription != null)
                            param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                        if (item.Debit != null)
                            param.Add(new SqlParameter("@Debit", item.Debit));
                        if (item.Credit != null)
                            param.Add(new SqlParameter("@Credit", item.Credit));
                        if (item.ID != null)
                            param.Add(new SqlParameter("@ID", item.ID));
                        if (item.IsDeleted != null)
                            param.Add(new SqlParameter("@IsDeleted", item.IsDeleted));
                        if (item.JournalID != null)
                            param.Add(new SqlParameter("@JournalID", item.JournalID));

                        con.ExecSpNone("JournalMovementHistoryUpdate", param);
                    }
                }
            }
        }
        #region Notification
        param.Clear();
        param.Add(new SqlParameter("@RelatedID", newObject[0].JournalID));
        con.ExecSpNone("NotificationUpdate", param);
        HubContext.Clients.All.NotificationCountDown("Notification Count Down");
        #endregion
    }

    public static void InsertJournalHistory(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var JournalObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Journal>(jsonString);
        param.Clear();
        param.Add(new SqlParameter("@BranchID", JournalObject.BranchID));
        param.Add(new SqlParameter("@JournalTypeID", JournalObject.JournalTypeID));
        param.Add(new SqlParameter("@StaffID", JournalObject.StaffID));
        param.Add(new SqlParameter("@JournalDate", JournalObject.JournalDate));

        DataTable dtJournal = con.ExecSpSelect("JournalInsert", param);

        var JournalID = int.Parse(dtJournal.Rows[0]["Identity"].ToString());

        if (JournalObject.JournalMovements != null)
        {
            foreach (var item in JournalObject.JournalMovements)
            {
                param.Clear();
                param.Add(new SqlParameter("@JournalID", JournalID));
                param.Add(new SqlParameter("@ApprovalStatusID", 32)); //Pending
                param.Add(new SqlParameter("@JournalMovementID", item.ID));
                if (item.AccountID != null)
                    param.Add(new SqlParameter("@AccountID", item.AccountID));
                if (item.MovementDescription != null)
                    param.Add(new SqlParameter("@MovementDescription", item.MovementDescription));
                if (item.Debit != null)
                    param.Add(new SqlParameter("@Debit", item.Debit));
                if (item.Credit != null)
                    param.Add(new SqlParameter("@Credit", item.Credit));
                if (item.IsDeleted != null)
                    param.Add(new SqlParameter("@IsDeleted", item.IsDeleted));
                if (item.CreationDate != null)
                    param.Add(new SqlParameter("@CreationDate", item.CreationDate));

                con.ExecSpNone("JournalMovementHistoryInsert", param);
            }
        }

        #region Notification
        string Msg = "";
        Msg = "Journal : " + JournalID + " has been requested to Create Journal with Difference Date ";

        param.Clear();
        param.Add(new SqlParameter("@Description", Msg));
        param.Add(new SqlParameter("@RelatedID", JournalID));
        con.ExecSpNone("NotificationInsert", param);

        HubContext.Clients.All.NotificationFired(Msg);
        #endregion

        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dtJournal) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

}