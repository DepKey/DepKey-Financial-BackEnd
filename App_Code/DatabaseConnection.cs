using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;

/// <summary>
/// Summary description for Common
/// </summary>
public class DatabaseConnection
{
    public DatabaseConnection()
    {
        Newtonsoft.Json.JsonConvert.DefaultSettings = () => new Newtonsoft.Json.JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,

            //please consider read format from configuration file
            DateFormatString = "dd/MM/yyyy"
        };
        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
    public DatabaseConnection(string connstring)
    {
        _dbConnectionString = connstring;
    }
    public List<T> DataReaderMapToList<T>(IDataReader dr)
    {
        List<T> list = new List<T>();
        T obj = default(T);
        while (dr.Read())
        {
            obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (!object.Equals(dr[prop.Name], DBNull.Value))
                {
                    prop.SetValue(obj, dr[prop.Name], null);

                }
            }
            list.Add(obj);
        }
        return list;
    }
    private string _dbConnectionString;
    public string ConnectionString
    {
        get
        {
            return _dbConnectionString;
        }
        set
        {
            _dbConnectionString = value;
        }
    }
    public SqlConnection OpenConnection(string connectionString)
    {
        try
        {
            SqlConnection myDbConnection = new SqlConnection(connectionString);
            myDbConnection.Open();
            return myDbConnection;
        }
        catch (Exception myException)
        {
            throw (new Exception(myException.Message));
        }
    }
    public void CloseConnection(SqlConnection myDbConnection)
    {
        try
        {
            if (myDbConnection.State != ConnectionState.Closed)
            {
                myDbConnection.Close();
            }
        }
        catch (Exception myException)
        {
            throw (new Exception(myException.Message));
        }
    }
    public DataTable ExecSpSelect(string spName, ArrayList pList)
    {
        DatabaseConnection myBase = this;
        //int iCount = 0;
        try
        {
            SqlConnection myConn = myBase.OpenConnection(myBase.ConnectionString);
            SqlCommand cmd = new SqlCommand(spName, myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            foreach (SqlParameter var in pList)
            {
                cmd.Parameters.Add(var);
            }
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = dr.GetSchemaTable().Clone();
            dt.Load(dr);
            dr.Close();
            dr.Dispose();
            cmd.Dispose();
            myBase.CloseConnection(myConn);
            return dt;

        }
        catch (Exception myErr)
        {
            throw (new Exception(myErr.ToString() + " error executing SP " + spName));
        }
    }
    public int ExecSpNone(string spName, ArrayList pList)
    {
        DatabaseConnection myBase = this;
        int iCount = 0;
        try
        {
            SqlConnection myConn = myBase.OpenConnection(myBase.ConnectionString);
            SqlCommand cmd = new SqlCommand(spName, myConn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter var in pList)
            {
                cmd.Parameters.Add(var);
            }

            SqlParameter returnValue = new SqlParameter("@Return_Value", DbType.Int32);
            returnValue.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnValue);

            iCount = cmd.ExecuteNonQuery();

            iCount = Int32.Parse(cmd.Parameters["@Return_Value"].Value.ToString());

            //iCount =int.Parse(cmd.ExecuteScalar().ToString());

            //cmd.ExecuteScalar();
            cmd.Dispose();
            myBase.CloseConnection(myConn);
            return iCount;
        }
        catch (Exception myErr)
        {
            throw (new Exception(myErr.Message + " error executing SP " + spName));
        }
        // return null;
    }
    public string DataTableToJson(DataTable table)
    {
        var list = new List<Dictionary<string, object>>();

        string[] dtDafult = new string[] { "ColumnName", "ColumnOrdinal", "ColumnSize", "NumericPrecision", "NumericScale", "IsUnique", "IsKey", "BaseServerName", "BaseCatalogName", "BaseColumnName", "BaseSchemaName", "BaseTableName", "DataType", "AllowDBNull", "ProviderType", "IsAliased", "IsExpression", "IsIdentity", "IsAutoIncrement", "IsRowVersion", "IsHidden", "IsLong", "IsReadOnly", "ProviderSpecificDataType", "DataTypeName", "XmlSchemaCollectionDatabase", "XmlSchemaCollectionOwningSchema", "XmlSchemaCollectionName", "UdtAssemblyQualifiedName", "NonVersionedProviderType", "IsColumnSet" };
        foreach (DataRow row in table.Rows)
        {
            var dict = new Dictionary<string, object>();

            foreach (DataColumn col in table.Columns)
            {
                if (!dtDafult.Contains(col.ColumnName))
                    dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        string s = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.None);
        return s;
    }
}