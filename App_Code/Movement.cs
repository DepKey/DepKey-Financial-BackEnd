using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Movement
/// </summary>
public class Movement
{
    public Movement()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreationDate { get; set; }
    public float? Debit { get; set; }
    public float? Credit { get; set; }
    public string MovementDescription { get; set; }
    public int? JournalID { get; set; }
    public int? AccountID { get; set; }

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? JournalNumber { get; set; }

    public static void SelectMovement(HttpContext context)
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

        DataTable dt = con.ExecSpSelect("JournalMovementSelect", param);
        string result = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(result));
    }

    public static void UpdateMovement(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JournalMovement>>(jsonString);

        if (oneObject != null)
        {
            foreach (var item in oneObject)
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
                if (item.ID != null)
                    param.Add(new SqlParameter("@ID", item.ID));
                con.ExecSpNone("JournalMovementUpdate", param);
            }
        }
        
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes("{\"result\":{\"result\":\"OK\",\"details\":\"NO ERROR\"}}"));
    }
}