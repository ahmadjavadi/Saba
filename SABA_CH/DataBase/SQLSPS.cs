using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Windows;
using DevExpress.Charts.Native;
using SABA_CH.Global;

namespace SABA_CH.DataBase
{
    public static class SQLSPS
    {

        public static void INSOBISs(string ObisCode, string Obis, string ObisFarsiDesc, string ObisLatinDesc, string ObisArabicDesc, decimal? DeviceTypeID, string ObisUnit, decimal? ObisTypeID, 
            string Format, int? ClassID, string CardFormatType, string HHuFormatType, string UnitConvertType, 
            ObjectParameter FixedOBISCode, ObjectParameter ReturnUnitConvertType, ObjectParameter ReturnOBISType, ObjectParameter OBISID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSOBISs(ObisCode, Obis, ObisFarsiDesc, ObisLatinDesc, ObisArabicDesc, DeviceTypeID, ObisUnit, ObisTypeID, Format, ClassID, CardFormatType, HHuFormatType,
                    UnitConvertType, FixedOBISCode, ReturnUnitConvertType, ReturnOBISType, OBISID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {                
                CommonData.WriteLOG(ex);
            }
        }
        public static void DelOBISs(decimal? OBISID, ObjectParameter Resul, ObjectParameter ErrMSG)
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {

                Bank.DelOBISs(OBISID, Resul, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);    
            }
        }
        public static void ImportFromDLMSClient(decimal? UserID, decimal? GroupID ,int? GroupType  ,decimal? LanguageID ,string Path ,string BasicPath,string SEWMPath )            
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {

                Bank.ImportFromDLMSClient( UserID,  GroupID , GroupType  , LanguageID , Path , BasicPath, SEWMPath );            
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex); 
            }
        }
        public static void ImportFromHHU303(decimal? UserID, decimal? GroupID, int? GroupType, decimal? LanguageID, string Path, string BasicPath, string SEWMPath)
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {
                Bank.ImportFromDLMSClient(UserID, GroupID, GroupType, LanguageID, Path, BasicPath, SEWMPath);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
        public static void DropTableFromSQLCompact(ObjectParameter Result, ObjectParameter ErrMSG)
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {

                Bank.DropTableFromSQLCompact(Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex); 
            }
        }
        public static void InsReadValueFromHHUXML(string Path,decimal? UserID,decimal? LanguageID,ObjectParameter Result, ObjectParameter ErrMSG)
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2400;
                Bank.InsReadValueFromHHUXML(Path, UserID, LanguageID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
                MessageBox.Show(ex.Message.ToString());                             
                CommonData.WriteLOG(ex);
            }
        }
        public static void ExportMeterError207(decimal? Resul, string ErrMSG)
        {
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2400;
                Bank.ExportMeterError207(Resul, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {
                Bank.Database.Connection.Close();
                Bank.Dispose();
                MessageBox.Show(ex.Message.ToString());                            
                CommonData.WriteLOG(ex);
            }
        }
        public static void UPDOBISsType(string Type, decimal? OBISID, ObjectParameter Resul, ObjectParameter ErrMSG, bool IsEndOFData)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.UPDOBISsType(Type, OBISID, Resul, ErrMSG);
                if (IsEndOFData)
                {
                    Bank.Database.Connection.Close();
                    Bank.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                             
                CommonData.WriteLOG(ex);
            }
        }
        public static void DelOBISsType(string Type, decimal? OBISID, ObjectParameter Resul, ObjectParameter ErrMSG, bool IsEndOFData)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.DelOBISsType(Type, OBISID, Resul, ErrMSG);
                if (IsEndOFData)
                {
                    Bank.Database.Connection.Close();
                    Bank.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                               
                CommonData.WriteLOG(ex);
            }
        }

        public static ShowMeter_Result ShowMeter(string filter)
        {
            ShowMeter_Result MeterInfo = new ShowMeter_Result();
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowMeter(filter, CommonData.UserID);
                foreach (var item in Bank.ShowMeter(filter, CommonData.UserID))
                {
                    MeterInfo.MeterID = item.MeterID;
                    MeterInfo.MeterNumber = item.MeterNumber;
                    MeterInfo.ModemDeviceModelID = item.ModemDeviceModelID;
                    MeterInfo.ModemID = item.ModemID;
                    MeterInfo.ModemSoftversion = item.ModemSoftversion;
                    MeterInfo.ModemSoftversionToDeviceModelID = item.ModemSoftversionToDeviceModelID;
                    MeterInfo.DeviceModelID = item.DeviceModelID;
                    MeterInfo.CustomerID = item.CustomerID;
                    MeterInfo.Softversion = item.Softversion;
                    MeterInfo.SoftversionToDeviceModelID = item.SoftversionToDeviceModelID;
                }
                Bank.Database.Connection.Close();
                Bank.Dispose();
               
            }
            catch (Exception ex)
            {

                CommonData.WriteLOG(ex);
            }
            return MeterInfo;
        }
        public static void ShowMeterNumber(string MeterNumber, ObjectParameter IsExits, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowMeterNumber(MeterNumber, IsExits, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

               // System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                CommonData.WriteLOG(ex);
            }
        }
        public static void ShowMeterNumberIssuance(string MeterNumber ,ObjectParameter IsExits, ObjectParameter IsAllow,ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowMeterNumberIssuance(MeterNumber,CommonData.UserID, IsExits,IsAllow, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                // System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                CommonData.WriteLOG(ex);
            }
        }

        public static void ShowCustomerIDMeterID(decimal? MeterID, ObjectParameter IsAllow,ObjectParameter CustomerID, ObjectParameter GroupID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowCustomerIDGroupID(MeterID, CommonData.UserID,IsAllow, CustomerID, GroupID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                CommonData.WriteLOG(ex);
            }
        }
        public static ShowMeterToCustomer_Result ShowMeterToCustomer(string filter)
        {
            ShowMeterToCustomer_Result meterToCustomer = null;
            SabaNewEntities Bank = new SabaNewEntities();
            try
            {
                Bank.Database.Connection.Open();
                foreach (var item in Bank.ShowMeterToCustomer(filter))
                {
                    meterToCustomer = item;
                    break;
                }
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
            return meterToCustomer;
        }

        public static void INSCards(string CardNumber, ObjectParameter CardID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSCards(CardNumber, CardID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
        public static void InsChangeDB(string changeDate, string softVersion, ObjectParameter ID, ObjectParameter result, ObjectParameter errMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsChangeDB(changeDate, softVersion, ID, result, errMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.ToString().ToString());
                //CommonData.WriteLOG(ex);
            }
        }
        public static void INSConsumedWater(decimal? MeterID, string ConsumedWater, string VEEConsumedWater, string ConsumedDate, string ReadDate, decimal OBISID, string DateOfReceivedFromSource, decimal? OBISValueHeaderID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSConsumedWater(MeterID, ConsumedWater, VEEConsumedWater, ConsumedDate, ReadDate, OBISID, DateOfReceivedFromSource, OBISValueHeaderID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void UPDConsumedWater(decimal? MeterID, string ConsumedWater, string VEEConsumedWater, string VEEMaxFlow, string TotalConsumedWater, string ReadDate, string ConsumedOBIS, string FlowOBIS, string ErrorMessage,ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.UPDConsumedWater(MeterID, ConsumedWater, VEEConsumedWater, VEEMaxFlow, TotalConsumedWater, ReadDate, ConsumedOBIS, FlowOBIS,ErrorMessage, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);    
                
            }
        }
        public static string UPDConsumedWaterForVEE(decimal? MeterID, string ReadDate, DataTable dtDetails, string ErrorMessage, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                string strMsg = "";
                SabaNewEntities Bank = new SabaNewEntities();
                try
                {
                    Bank.Database.Connection.Open();
                    int col = dtDetails.Columns.Count;
                    int row = dtDetails.Rows.Count;

                    string s = Bank.Database.Connection.ConnectionString + ";password=88102351-7";
                    SqlConnection connection = new SqlConnection(s);
                    connection.Open();
                    SqlCommand cmdProc = new SqlCommand("UPDConsumedWaterforVEE", connection);
                    cmdProc.CommandType = CommandType.StoredProcedure;
                    cmdProc.Parameters.AddWithValue("@MeterID", MeterID);
                    cmdProc.Parameters.AddWithValue("@ReadDate", ReadDate);
                    cmdProc.Parameters.AddWithValue("@Details", dtDetails);
                    cmdProc.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
                    //cmdProc.Parameters.AddWithValue("@Result", Result);
                    //cmdProc.Parameters.AddWithValue("@ErrMSG", ErrMSG);
                    int n = dtDetails.Rows.Count;
                    cmdProc.ExecuteNonQuery();
                    strMsg = "Saved successfully.";
                }
                catch (SqlException e)
                {
                    //strMsg = "Data not saved successfully.";
                    strMsg = e.Message.ToString();
                    MessageBox.Show(strMsg);
                }
                finally
                {
                    Bank.Database.Connection.Close();
                    Bank.Dispose();//Function for closing connection

                }
                return strMsg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
                return "";
            }
            
        }
        public static string UPDVeeConsumedWater(DataTable dtDetails)
        {
            try
            { 
                string strMsg = "";
                SabaNewEntities Bank = new SabaNewEntities();
                try
                {
                    Bank.Database.Connection.Open();
                    int col = dtDetails.Columns.Count;
                    int row = dtDetails.Rows.Count;

                    string s = Bank.Database.Connection.ConnectionString + ";password=88102351-7";
                    SqlConnection connection = new SqlConnection(s);
                    
                    connection.Open();

                    SqlCommand cmdProc = new SqlCommand("UPDVeeConsumedWater", connection);
                    cmdProc.CommandType = CommandType.StoredProcedure;
                    
                    cmdProc.Parameters.AddWithValue("@Details", dtDetails);
                    cmdProc.CommandTimeout = 0;


                    int n = dtDetails.Rows.Count;
                    cmdProc.ExecuteNonQuery();
                    strMsg = "Saved successfully.";
                }
                catch (SqlException e)
                {
                    //strMsg = "Data not saved successfully.";
                    strMsg = e.Message.ToString();
                    MessageBox.Show(strMsg);
                }
                finally
                {
                    Bank.Database.Connection.Close();
                    Bank.Dispose();//Function for closing connection

                }
                return strMsg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                CommonData.WriteLOG(ex);
                return "";
            }

        }
        public static string INSReadValueFromHHu(decimal? OBISValueHeaderID, decimal? MeterID, string ReadDate,string TransferDate, DataTable dtDetails, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                string strMsg = "";
                SabaNewEntities Bank = new SabaNewEntities();
                try
                {
                    Bank.Database.Connection.Open();
                    int col = dtDetails.Columns.Count;
                    int row = dtDetails.Rows.Count;

                    string s = Bank.Database.Connection.ConnectionString + ";password=88102351-7";
                    SqlConnection connection = new SqlConnection(s);
                    connection.Open();
                    SqlCommand cmdProc = new SqlCommand("INSReadValueFromHHu", connection);
                    cmdProc.CommandType = CommandType.StoredProcedure;
                    cmdProc.Parameters.AddWithValue("@OBISValueHeaderID", OBISValueHeaderID);
                    cmdProc.Parameters.AddWithValue("@MeterID", MeterID);
                    cmdProc.Parameters.AddWithValue("@ReadDate", ReadDate);
                    cmdProc.Parameters.AddWithValue("@TransferDate", TransferDate);
                    cmdProc.Parameters.AddWithValue("@Details", dtDetails);
                   
                    int n = dtDetails.Rows.Count;
                    cmdProc.ExecuteNonQuery();
                    strMsg = "Saved successfully.";
                }
                catch (SqlException e)
                {
                    //strMsg = "Data not saved successfully.";
                    strMsg = e.Message.ToString();
                    MessageBox.Show(strMsg);
                }
                finally
                {
                    Bank.Database.Connection.Close();
                    Bank.Dispose();//Function for closing connection

                }
                return strMsg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
                return "";
            }

        }
        public static void INSConsumedActiveEnergy(decimal? MeterID, string ConsumedWater, string ConsumedDate, string ReadDate, decimal OBISID, string DateOfReceivedFromSource, decimal? OBISValueHeaderID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSConsumedActiveEnergy(MeterID, ConsumedWater, ConsumedDate, ReadDate, OBISID, DateOfReceivedFromSource, OBISValueHeaderID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);    
                
            }
        }
        public static void InsCardToMeter(decimal? cardId, decimal? meterId, string setDate, decimal? userId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsCardToMeter(cardId, meterId, setDate, userId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void RestoreDatabase(string dbName, string dbAddress,  ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.RestoreDatabase(dbName, dbAddress, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void ImportSmartCardReader(decimal? groupId,int? groupType,  ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                ((IObjectContextAdapter)bank).ObjectContext.CommandTimeout = 2400;
                bank.Database.Connection.Open();
                bank.ImportSmartCardReader(groupId, groupType,result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void ImportFromSabaCandHNew(decimal? groupId, int? groupType, decimal? sabaCandHid, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                string errMSG = "";
                SabaNewEntities Bank = new SabaNewEntities();
                ((IObjectContextAdapter)Bank).ObjectContext.CommandTimeout = 2400;
                Bank.Database.Connection.Open();
                Bank.ImportFromSabaCandHNew(Convert.ToDecimal(CommonData.UserID), 1,groupId, groupType, result, errMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
        public static void ImportdataFromDlmsClient(decimal? userId, decimal? groupId, int? groupType, decimal? languageId, string path, string basicPath, string sewmPath)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                ((IObjectContextAdapter)bank).ObjectContext.CommandTimeout = 24000;
                bank.Database.Connection.Open();
                bank.ImportFromDLMSClient( userId,  groupId,  groupType,  languageId,  path,  basicPath,  sewmPath);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);    
                
            }
        }
        public static void InsObiSsFromFile(string filename, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.InsOBISsFromFile(filename, result, errMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void InsButtonAccess(decimal? buttonId, decimal? userId, bool canShow, bool canDelete, bool canEdit, bool canInsert, bool canImportFromFile, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.InsButtonAccess(buttonId, userId, canShow, canDelete, canEdit, canInsert, canImportFromFile, result, errMsg);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void InsMeter(string meterNumber, decimal? deviceModelId, bool isDirect, string softversion, decimal? softversionToDeviceModelId, decimal? moemId, decimal? customerId, decimal? groupId,
            int? groupType, bool valid, ObjectParameter meterId, ObjectParameter errMsg, ObjectParameter result)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSMeter(meterNumber, deviceModelId, isDirect, softversionToDeviceModelId, softversion, moemId, customerId, groupId, groupType, true, meterId, errMsg, result);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());   
               // CommonData.WriteLOG(ex);      
                
            }

        }


        public static void InsMeterShutdownInterval(decimal? customerId, string startDate, string endDate, decimal? UserID,int? NumberOfMonths, ObjectParameter errMsg, ObjectParameter result)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsMeterShutdownInterval(customerId, startDate, endDate, UserID, NumberOfMonths, DateTime.Now.ToPersianDate(),  errMsg, result);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                
            }

        }
        public static void UpdMeter(string meterNumber, decimal? deviceModelId, bool isDirect, decimal? softversionToDeviceModelId, decimal? moemId, decimal? customerId,
            bool valid, decimal? meterId, ObjectParameter errMsg, ObjectParameter result)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                if (deviceModelId == 0) deviceModelId = null;
                bank.UPDMeter(meterNumber, deviceModelId, isDirect, softversionToDeviceModelId, moemId,customerId, true, null, meterId, errMsg, result);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                              
                CommonData.WriteLOG(ex);
            }

        }
        public static void Insmodem(string modemNumber, string simNumber, decimal? softversionToDeviceModelId, decimal? deviceModelId, ObjectParameter modemId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.INSMODEM(modemNumber, simNumber, softversionToDeviceModelId, deviceModelId, modemId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                                
                CommonData.WriteLOG(ex);
            }
        }
        public static void Updmodem(string modemNumber, string simNumber, decimal? softversionToDeviceModelId, decimal? deviceModelId, decimal? modemId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.UPDMODEM(modemNumber, simNumber, softversionToDeviceModelId, deviceModelId, modemId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                               
                CommonData.WriteLOG(ex);
            }
        }
        public static void InsMeterToCustomer(decimal? meterId, decimal? customerId, bool valid, string setDate, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsMeterToCustomer(customerId, meterId, valid, setDate, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                              
                CommonData.WriteLOG(ex);
            }
        }
        public static void DelMeterToCustomer(decimal? meterId, decimal? customerId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.DelMeterToCustomer(meterId, customerId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
        public static void InsCustomers(string customerName, string customerfamily, string customerTel, string customerAddress, string watersubscriptionNumber, string elecsubscriptionNumber, decimal? locationId, decimal? nationalCode, string mobileNumber, string longitude, string latitude, string postCode, decimal? officeId, decimal? eofficeId, decimal? subofficeId, decimal? esubofficeId, string dossierNumber, string flowindossier, string diameterofpipe, string welldepth, string wellLicense, string wellAddress, string FatherName, int? typeOfUse, ObjectParameter customerId, ObjectParameter result, ObjectParameter errMsg)
        {
            SabaNewEntities bank = new SabaNewEntities();
            try
            {
               
                bank.Database.Connection.Open();
                bank.INSCustomers(customerName, customerfamily, customerTel, customerAddress, watersubscriptionNumber, elecsubscriptionNumber,
                    locationId, nationalCode, mobileNumber, longitude, latitude, postCode, officeId, eofficeId, subofficeId, esubofficeId, dossierNumber, flowindossier, diameterofpipe, welldepth, wellLicense, wellAddress, FatherName,typeOfUse, CommonData.LanguagesID,customerId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
               // System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                CommonData.WriteLOG(ex);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
        }
        public static void UpdCustomers(string customerName, string customerfamily, string customerTel, string customerAddress, string watersubscriptionNumber, string elecsubscriptionNumber, 
            decimal? locationId, decimal? nationalCode, string mobileNumber, string longitude, string latitude, string postCode, 
            decimal? officeId, decimal? eofficeId, decimal? wSubofficeId, decimal? eSubofficeId, string dossierNumber, string flowindossier, int? diameterofpipe, int? welldepth, string wellLicense, string wellAddress, string fatherName, int? typeOfUse, decimal? customerId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.UPDCustomers(customerName, customerfamily, customerTel, customerAddress, watersubscriptionNumber, elecsubscriptionNumber, locationId, nationalCode, mobileNumber, longitude, latitude, postCode, officeId, eofficeId, wSubofficeId, eSubofficeId, dossierNumber, flowindossier, diameterofpipe.ToString(), welldepth.ToString(), wellLicense, wellAddress, fatherName, typeOfUse, customerId, result, errMsg);
                
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                      
                CommonData.WriteLOG(ex);                      
            }
        }
        public static void DelCustomer(decimal? customerId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.DelCustomer(customerId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
        public static void DelVeeConsumedWater(decimal? customerId, decimal? meterId, string startDate,string endDate, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.DelVeeConsumedWater(customerId, meterId, startDate, endDate, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                
            }
        }
        public static void InsObisToWindow(decimal? obisid, int windowId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsOBISToWindow(obisid, windowId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void DelObisToWindow(decimal? obisid, int windowId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelOBISToWindow(obisid, windowId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }

        public static void DelMeter(decimal? meterId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelMeter(meterId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void DelModem(decimal? modemId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelModem(modemId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsLocations(decimal? plainId, decimal? catchmentId, decimal? areaId, decimal? cityId, ObjectParameter locationId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSLocations(plainId, catchmentId, areaId, cityId, locationId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void InsCities(string cityCode, string cityName, decimal? countryId, decimal? provinceId, ObjectParameter cityId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsCities(cityCode, cityName, countryId, provinceId, cityId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsMeterToGroup(decimal? meterId, decimal? groupId, int groupType, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsMeterToGroup(meterId, groupId, groupType, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());        
                CommonData.WriteLOG(ex);
            }
        }
        public static void DelMeterToGroup(decimal? meterId, decimal? groupId, int groupType, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelMeterToGroup(meterId, groupId, groupType, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsGroups(string groupName, string hashValue, ObjectParameter groupId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsGroups(groupName, hashValue, groupId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                CommonData.WriteLOG(ex);                    
            }
        }
        public static void UpdGroups(string groupName, string hashValue, decimal? groupId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.UPDGroups(groupName, hashValue, groupId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());       
                CommonData.WriteLOG(ex);
            }
        }
        public static void DelGroups(decimal? groupId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.DelGroups(groupId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void DelUserToGroup(decimal? userId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelUserToGroup(userId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void InsUserToGroup(decimal? groupId, decimal? userId, bool valid, int? groupType, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsUserToGroup(groupId, userId, valid, groupType, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsReports(decimal? userId, string reportName, ObjectParameter reportId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.INSReports(userId, reportName, reportId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);   
                
            }
        }
        public static void UpdReports( string reportName, decimal? reportId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.UPDReports(reportName, reportId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void InsobisToReport(decimal? reportId, decimal? obisid, decimal? sheetId, decimal? obisTypeId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.INSOBISToReport(reportId, obisid, sheetId,obisTypeId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsObisTypeToReport(decimal? reportId, int? obisTypeId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsOBISTypeToReport(obisTypeId, reportId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void DelObisToReport(decimal reportId,decimal? obisTypeId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelOBISToReport(reportId,obisTypeId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void DelObisTypeToReport(decimal reportId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelOBISTypeToReport(reportId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void DelReport(decimal reportId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelReport(reportId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);      
                
            }
        }
        public static void InsDeviceModel(string deviceModelName, string manufacturerName, string deviceName, string messageVersion, ObjectParameter deviceTypeId, ObjectParameter deviceModelId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsDeviceModel(deviceModelName, manufacturerName, deviceName, messageVersion, deviceTypeId, deviceModelId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);    
                
            }
        }
        public static void UpdDeviceModel(string deviceModelName, string manufacturerName, decimal? deviceTypeId, decimal? deviceModelId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.UPDDeviceModel(deviceModelName, manufacturerName, deviceTypeId, deviceModelId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());        
                CommonData.WriteLOG(ex);
            }
        }
        public static void DelDeviceModel(decimal? deviceModelId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.DelDeviceModel(deviceModelId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());     
                CommonData.WriteLOG(ex);
            }
        }

        public static bool ShowClockObisValue(string value, decimal? meterId)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("Result", false);
                ObjectParameter obisValueHeaderId = new ObjectParameter("OBISValueHeaderID", 10000000);
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowClockOBISValue(value, meterId, result, obisValueHeaderId);
                Bank.Database.Connection.Close();
                Bank.Dispose();
                CommonData.OBISValueHeaderID = Convert.ToDecimal(obisValueHeaderId.Value);
                return Convert.ToBoolean(result.Value);
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
                return false;
            }
        }
        public static void InsobisValueHeader(string readDate, decimal? deviceTypeId, decimal? userId, string transferDate, decimal? meterId, decimal? sourceTypeId, ObjectParameter obisValueHeaderId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.INSOBISValueHeader(readDate, deviceTypeId, userId, transferDate, meterId, sourceTypeId, obisValueHeaderId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());         
                CommonData.WriteLOG(ex);
            }
        }

        public static void InsobisValueDetail(decimal? obisValueHeaderId, string softwareVersion, decimal? obisid, string value, string veeValue, string readValuUnitName, ObjectParameter obisValueId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSOBISValueDetail(obisValueHeaderId, obisid,softwareVersion, value, veeValue, readValuUnitName, obisValueId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsCurve(decimal? curveId, decimal? readOutId, string curveCode, string point1Flow, string point2Flow,
            string point3Flow, string point4Flow, string point5Flow, string point6Flow, string point1Power, string point2Power,
            string point3Power, string point4Power, string point5Power, string point6Power, string noloadPower, string calibrationFlow,
            string calibrationPower, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsCurve(curveId, readOutId,curveCode,point1Flow,point2Flow,point3Flow,point4Flow,point5Flow,point6Flow, point1Power,
                  point2Power,point3Power,point4Power,point5Power,point6Power,noloadPower,calibrationFlow,calibrationPower, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString()); CommonData.WriteLOG(ex);
            }
        }
        public static void InsSoftversionToDeviceModel(string Softversion, decimal? DeviceModelID, ObjectParameter SoftversionToDeviceModelID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSSoftversionToDeviceModel(Softversion, DeviceModelID, SoftversionToDeviceModelID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);     
                
            }
        }
        public static void InsobisToSoftversion(decimal? obisid, decimal? softversionToDeviceModelId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.INSOBISToSoftversion(obisid, softversionToDeviceModelId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);    
                
            }
        }
        public static void UpdobiSs(string obisCode, string obis, string obisFarsiDesc, string obisLatinDesc, string obisArabicDesc, decimal? deviceTypeId, string obisUnitDesc, int? obisTypeId, string format, int? classId, string cardFormatType, string hHuFormatType, decimal? obisid, decimal? unitIdForshow, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.UPDOBISs(obisCode, obis, obisFarsiDesc, obisLatinDesc, obisArabicDesc, deviceTypeId, obisUnitDesc, obisTypeId, format, classId, cardFormatType, hHuFormatType, obisid, unitIdForshow, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
     
        public static void UpdUser(string userName, string userPass, decimal? userId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.UPDUser(userName, userPass, userId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
        public static void DelUser(decimal? userId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.DelUser(userId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
        public static void InsUsers(string userName, string userPass, ObjectParameter userId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsUsers(userName, userPass, userId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);                       
            }
        }
        public static void ShowMaxSequenceNumber(decimal? meterId, ObjectParameter maxSequenceNumber, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.showMaxSequenceNumber(meterId, maxSequenceNumber, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                //System.Windows.MessageBox.Show(ex.Message.ToString());                                
                CommonData.WriteLOG(ex);
            }
        }
        public static void InsDangelInfo(string dangelPass, string dangelSerial, string connectingDevice, string softwareVersion, string country, string maxprivilege, string expireDate, string sump, string lsbCode, string provinceCode, ObjectParameter dangelId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities bank = new SabaNewEntities();
                bank.Database.Connection.Open();
                bank.InsDangelInfo(dangelPass, dangelSerial, connectingDevice, softwareVersion, country, maxprivilege, expireDate, sump, lsbCode,provinceCode, dangelId, result, errMsg);
                bank.Database.Connection.Close();
                bank.Dispose();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message.ToString());                          
                CommonData.WriteLOG(ex);
            }
        }
        public static void ShowMinReadDate(decimal? meterId, ObjectParameter minReadDate)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowMinReadDate(meterId, minReadDate);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message.ToString());                                
                CommonData.WriteLOG(ex);
            }
        }
        public static void ShowMaxReadDate(decimal? meterId, ObjectParameter maxReadDate)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.ShowMaxReadDate(meterId, maxReadDate);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                //System.Windows.MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                CommonData.WriteLOG(ex);
            }
        }
        public static void InsCreateTokenByDongle(int dayNumber, int minuteOfDay, decimal? systemId, int sequenceNumber, string tokenId, int currentYear, decimal? meterId, ObjectParameter tokenFromDongleId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsCreateTokenByDongle(dayNumber, minuteOfDay, systemId, sequenceNumber, tokenId, currentYear, meterId, tokenFromDongleId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }

        }
        public static void InsToken(string usbDeviceCode, decimal userId, string computerCode, int? sequenceNumber, string token, 
            
            string buildDate, decimal? cardId, decimal? hash, decimal? meterId, string creditTransferModes, string startDateTime,
            string endDateTime, decimal? creditValue, string usbSignature, decimal? obisValueHeaderId, int? crediteActivation, 
            string creditStartDate, int? negativeCredit, int? expireCredit, ObjectParameter tokenId, ObjectParameter result, ObjectParameter errMsg)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsToken(usbDeviceCode, userId, computerCode, sequenceNumber, token, buildDate, cardId, hash, meterId, creditTransferModes, startDateTime, endDateTime, creditValue, usbSignature, obisValueHeaderId, crediteActivation, creditStartDate, negativeCredit, expireCredit, tokenId, result, errMsg);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }

        }
         
        public static void InsCredit303(decimal? MeterID,int? CrediteActivation,string CreditStartDate,int? NegativeCredit, int? ExpireCredit , decimal? OBISValueHederID, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {

                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.InsCredit303(MeterID, CrediteActivation, CreditStartDate, NegativeCredit, ExpireCredit, OBISValueHederID, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString().ToString());
                CommonData.WriteLOG(ex);
            }
        }
        public static void SaveOBISVALUE()
        {
            try
            {

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }
        public static void UPDOBISValueDetailAfterins(string CardNumber, decimal? OBISValueHeaderID, bool IsDuplicateData, ObjectParameter Result, ObjectParameter ErrMSG)
        {
            try
            {
                SabaNewEntities Bank = new SabaNewEntities();
                Bank.Database.Connection.Open();
                Bank.UPDOBISValueDetailAfterins(CardNumber, OBISValueHeaderID, IsDuplicateData, Result, ErrMSG);
                Bank.Database.Connection.Close();
                Bank.Dispose();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);       
                
            }
        }


    }
}
