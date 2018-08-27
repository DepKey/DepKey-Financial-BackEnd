using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for SalesMappingAccountPaymentMethod
/// </summary>
public class SalesMappingAccountPaymentMethod
{
    public SalesMappingAccountPaymentMethod()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int? ID { get; set; }
    public int? AccountID { get; set; }
    public int? PaymentMethodID { get; set; }


    public static void SelectSalesMappingAccountPaymentMethod(HttpContext context)
    {
        DatabaseConnection con = new DatabaseConnection();
        System.Collections.ArrayList param = new System.Collections.ArrayList();
        byte[] bytes = context.Request.BinaryRead(context.Request.TotalBytes);
        string jsonString = Encoding.UTF8.GetString(bytes);
        var oneObject = Newtonsoft.Json.JsonConvert.DeserializeObject<SalesMappingAccountPaymentMethod>(jsonString);

        param.Clear();
        param.Add(new System.Data.SqlClient.SqlParameter("@PaymentMethodID",oneObject.PaymentMethodID));
        DataTable dtMappingCash = con.ExecSpSelect("SalesMappingAccountPaymentMethodSelect", param);
        string s = "{\"result\": {\"result\": \"OK\",\"details\": \"NO ERROR\"},\"Rows\":" + con.DataTableToJson(dtMappingCash) + "}";
        context.Response.BinaryWrite(Encoding.UTF8.GetBytes(s));
    }
}