using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Loockup
/// </summary>
public class Branch
{
    public Branch()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public string Name { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime CreationDate { get; set; }
    public string Telephone { get; set; }
    public string Address { get; set; }

    public static void SelectBranch(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Branch>(jsonString);

        if (oneObject != null)
        {
            if (oneObject.ID != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@ID", oneObject.ID));
            if (oneObject.Name != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@Name", oneObject.Name));
            if (oneObject.IsDeleted != null)
                param.Add(new System.Data.SqlClient.SqlParameter("@IsDeleted", oneObject.IsDeleted));
        }

        param.Add(new System.Data.SqlClient.SqlParameter("@PageNumber", int.Parse(context.Request.QueryString["PageNumber"])));
        param.Add(new System.Data.SqlClient.SqlParameter("@PageSize", int.Parse(context.Request.QueryString["PageSize"])));

        DataTable dt = con.ExecSpSelect("BranchSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dt) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }
}