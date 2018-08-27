using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SalesOnline
/// </summary>
public class SalesOnline
{
    public SalesOnline()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public class TempSales
    {
        public string TicketNumber { get; set; }
    }

    public void Test()
    {
        SqlConnection conOnline = new SqlConnection(@"Data Source=5.79.121.153;Database=depkey;Integrated Security=SSPI;User ID=AspNetDBuser;Password=!SQL@DB#Depkey$2017;Trusted_Connection=False;");

        try
        {
            conOnline.Open();

            string selectQuery = @" SELECT TD.TransactionID,TD.[TransactionDate] ,TD.ModifiedBy,AIR.Code AS AirlineCode 
                                    		,AIR.[Name] AS AirlineName,FTD.[TicketNumber] AS TicketNo,PGD.[DisplayName] AS PaymentGateway
                                    		,PSD.BaseFare AS IndividualBase, PSD.AirlineTax as IndividualTax,PSD.BaseFare+PSD.AirlineTax            AS TotalCost,   
                                    		(PSD.[BaseFare]+PSD.[AirlineTax]+PSD.[Markup]+PSD.[MarkupTax] - PSD.[Discount] - PSD.[LeadPaxDiscount] - PSD.[AdditionalPaxDiscount] - PSD.[OtherDiscount]) as IndividaulAmount,
											(PSD.[BaseFare]+PSD.[AirlineTax]+PSD.[Markup]+PSD.[MarkupTax] - PSD.[Discount] - PSD.[LeadPaxDiscount] - PSD.[AdditionalPaxDiscount] - PSD.[OtherDiscount]) -( PSD.BaseFare+PSD.AirlineTax) AS Profit
                                    		,FBD.StartPoint,FBD.[EndPoint]
                                    		, PASSD.[LastName] + '/' +PASSD.FirstName   AS PassengerName
                                    		, Case When FBD.BookingType =1 then 'One way'
                                    			When FBD.BookingType =2 then 'Round trip'
                                    			When FBD.BookingType =3 then 'Multi city' END As BookingType
                                    		--,TD.PaymentGatewayTransactionId 
                                    		--FBD.*
                                    		--PD.*
                                    		,FBD.SupplierPNR as 'PNR'
											

                                    		FROM dbo.TransactionDetail TD 
                                    		JOIN dbo.TransactionTypeDetails TTD
                                    		ON TD.TransactionId = TTD.transactionid
                                    		
                                    		JOIN dbo.FlightBookingDetail FBD 
                                    		ON FBD.FlightBookingId = TTD.BookingId
                                    		
                                    		JOIN [dbo].[FlightTicketDetails] FTD ON FTD.[TransactionTypeDetailId]= TTD. [TransactionTypeDetailId]
                                    		JOIN dbo.PassengerDetail PASSD ON PASSD.PassengerId = FTD.PassengerId
                                    		
                                    		JOIN dbo.Airline AIR ON AIR.Code = FBD.OperatingVender
                                    		
                                    		JOIN dbo.PaymentDetails PD
                                    		ON PD.TransactionId = TD.TransactionId
                                    		
                                    		JOIN dbo.PaymentGatewayTransaction PGTD ON PGTD.[ID] = TD.PaymentGatewayTransactionId
                                    		JOIN [dbo].[PaymentGatewayDetails] PGD  ON PGD.[Name] = PGTD.PaymentType 
                                    		JOIN dbo.FlightItenaryMain FIM ON FIM.flightbookingid =FBD.FlightBookingId
                                    		JOIN dbo.PaymentSplitUpDetails PSD ON PSD.FlightItenaryMainId = FIM.ItenaryMainID 
                                    		AND PSD.PassengerType = PASSD.PassengerType
                                    	
                                    	WHERE TD.[TransactionDate] >= '7-16-2018'  AND TD.[TransactionStatus]=3";
            
            SqlCommand cmdSelect = new SqlCommand(selectQuery, conOnline);
            
            SqlConnection con = new SqlConnection("Data Source=5.79.102.57;Initial Catalog=CentralDepKey;User ID=aspnetuser;Password=DK_P@ssw0rd");
            con.Open();

            Hashtable AirlineList = new Hashtable();
            AirlineList.Clear();
            var query = "select ID, SabreCode from Airline";
            SqlCommand cmdAirline = new SqlCommand(query, con);
            SqlDataReader rdrAirline = cmdAirline.ExecuteReader();
            while (rdrAirline.Read())
                AirlineList.Add(rdrAirline["SabreCode"].ToString(), rdrAirline["ID"].ToString());
            rdrAirline.Close();
            List<string> AirlineName = new List<string>();
            string selectAirlineQuery = @"select Name from Airline";
            SqlCommand cmdAirlineName = new SqlCommand(selectAirlineQuery, con);
            SqlDataReader rdrAirlineName = cmdAirlineName.ExecuteReader();
            while (rdrAirlineName.Read())
                AirlineName.Add(rdrAirlineName["Name"].ToString());
            rdrAirlineName.Close();

            Hashtable StaffList = new Hashtable();
            StaffList.Clear();
            var query2 = "select ID, Name from Staff";
            SqlCommand cmdStaff = new SqlCommand(query2, con);
            SqlDataReader rdrStaff = cmdStaff.ExecuteReader();
            while (rdrStaff.Read())
                StaffList.Add(rdrStaff["Name"].ToString(), rdrStaff["ID"].ToString());
            rdrStaff.Close();

            Reload:
            List<Sales> Sales = new List<Sales>();
            string selectTempSales = @"select TicketNumber from Sales";
            SqlCommand cmdTempSales = new SqlCommand(selectTempSales, con);
            SqlDataReader rdrTempSales = cmdTempSales.ExecuteReader();

            while (rdrTempSales.Read())
            {
                Sales temp = new Sales();
                temp.TicketNumber = (string)rdrTempSales["TicketNumber"];
                Sales.Add(temp);
            }
            rdrTempSales.Close();


            using (SqlDataReader rdr = cmdSelect.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        var ticket = rdr[5].ToString().Substring(rdr[5].ToString().Length - 10);

                        var result = Sales.Where(a => a.TicketNumber == ticket).ToList();
                        if (result.Count == 0)
                        {


                            double fare = Math.Round(double.Parse(rdr[7].ToString()), 3);
                            double tax = Math.Round(double.Parse(rdr[8].ToString()), 3);
                            double totalCost = Math.Round(double.Parse(rdr[9].ToString()), 3);
                            double salesAmount = Math.Round(double.Parse(rdr[10].ToString()), 3);
                            double Profit = Math.Round(double.Parse(rdr[11].ToString()), 3);
                            double VisaOrCard = Math.Round(double.Parse(rdr[10].ToString()), 3);


                            foreach (var item in AirlineName.ToList())
                            {
                                if (item == rdr[4].ToString())
                                {

                                    var codeLength = rdr[5].ToString().Length - 10;

                                    var code = rdr[5].ToString().Substring(0, codeLength);

                                    if (code.ToString().Length == 1)
                                    {
                                        code = "00" + code;
                                    }
                                    else if (code.ToString().Length == 2)
                                    {
                                        code = "0" + code;
                                    }
                                    else
                                    {
                                        code = code;
                                    }

                                    // update code in airline table 

                                    string updateQuery = @"update Airline set Code = @Code where Name = @AirlineName";

                                    SqlCommand cmdUpdateAirline = new SqlCommand(updateQuery, con);

                                    cmdUpdateAirline.Parameters.AddWithValue("@Code", code);
                                    cmdUpdateAirline.Parameters.AddWithValue("AirlineName", item);

                                    SqlDataReader rdrUpdateAirline = cmdUpdateAirline.ExecuteReader();
                                    rdrUpdateAirline.Close();

                                }
                            }

                            string insertQuery = @"INSERT INTO Sales(AirlineID,TicketNumber,SalesTypeID,SalesStatusID,PaymentMethodID,Fare,
                                            Tax,TotalCost,SalesAmount,Profit,PaxName,
                                            Destination,InvoiceNumber,Card,Visa,Cash,Credit,
                                            Complementary,Advance,PNR,DKNumber,AccountID,IsDeleted,CreatedStaffID)

                                   VALUES(@AirlineId,@TicketNumber,27,4,@PaymentMethodId,@Fare,
                                            @Tax,@TotalCost,@SalesAmount,@Profit,@PaxName,
                                            @Description,@InvoiceNumber,@Card,@Visa,@Cash,@Credit,
                                            @Complementary,@Advance,@PNR,@DepkeyTransactionId,245,0,@StaffID)";


                            SqlCommand cmd = new SqlCommand(insertQuery, con);



                            cmd.Parameters.AddWithValue("@CreationDate", DateTime.Parse(rdr[1].ToString()));
                            
                            if (rdr[3].ToString() != null)
                            {
                                int AirLineCode = int.Parse(AirlineList[rdr[3].ToString()].ToString());// dictionary[rdr[4].ToString()];
                                cmd.Parameters.AddWithValue("@AirlineId", AirLineCode);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@AirlineId", null);
                            }

                            if (rdr[2].ToString() != null)
                            {
                                int StaffID = int.Parse(StaffList[rdr[2].ToString()].ToString());
                                cmd.Parameters.AddWithValue("@StaffID", StaffID);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@StaffID", null);
                            }

                            cmd.Parameters.AddWithValue("@TicketNumber", ticket);
                            int PaymentMethodId = 0;
                            if (rdr[6].ToString() == "Knet")
                            {
                                PaymentMethodId = 9;

                                cmd.Parameters.AddWithValue("@Card", VisaOrCard);
                                cmd.Parameters.AddWithValue("@Visa", 0);
                            }
                            if (rdr[6].ToString() == "Kfh")
                            {
                                PaymentMethodId = 11;
                                cmd.Parameters.AddWithValue("@Visa", VisaOrCard);
                                cmd.Parameters.AddWithValue("@Card", 0);
                            }
                            cmd.Parameters.AddWithValue("@Cash", 0);
                            cmd.Parameters.AddWithValue("@Credit", 0);
                            cmd.Parameters.AddWithValue("@Complementary", 0);
                            cmd.Parameters.AddWithValue("@Advance", 0);
                            cmd.Parameters.AddWithValue("@PaymentMethodId", PaymentMethodId);
                            cmd.Parameters.AddWithValue("@Fare", fare);
                            cmd.Parameters.AddWithValue("@Tax", tax);
                            cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                            cmd.Parameters.AddWithValue("@SalesAmount", salesAmount);
                            cmd.Parameters.AddWithValue("@Profit", Profit);
                            cmd.Parameters.AddWithValue("@Description", (" From:  " + rdr[12].ToString() + " To: " + rdr[13].ToString()));
                            cmd.Parameters.AddWithValue("@PaxName", rdr[14]);
                            cmd.Parameters.AddWithValue("@SalesTypeId", 14);
                            cmd.Parameters.AddWithValue("@TransactionNumber", 0);
                            cmd.Parameters.AddWithValue("@InvoiceNumber", 0);
                            cmd.Parameters.AddWithValue("@BranchId", 13);
                            cmd.Parameters.AddWithValue("@DepkeyTransactionId", int.Parse(rdr[0].ToString()));
                            cmd.Parameters.AddWithValue("@PNR", rdr[16]);



                            //rdr.Close();
                            cmd.ExecuteNonQuery();
                            goto Reload;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var msg = ex;
        }
    }
}