
using System;
using System.ComponentModel;
using System.Data.Objects;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Card;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH
{
	/// <summary>
	/// Interaction logic for IssuanceCard.xaml
	/// </summary>
	public partial class IssuanceCard : System.Windows.Window
	{
		public readonly int windowID=47;
		 private TabControl tabCtrl;
        private TabItem tabPag;
        public TabItem TabPag
        {
            get { return tabPag; }

            set { tabPag = value; }
        }
        public TabControl Tab { set { tabCtrl = value; } }
		public IssuanceCard()
		{
			this.InitializeComponent();
            txtSerialNumbr.Text = CommonData.SelectedMeterNumber;
			// Insert code required on object creation below this point.
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			 tabCtrl.SelectedItem = tabPag;
            if (!tabCtrl.IsVisible)
            {

                tabCtrl.Visibility = Visibility.Visible;

            }
			
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
            try
            {
                ClassControl.OpenWin[47] = false;
                tabCtrl.Items.Remove(tabPag);
                if (!tabCtrl.HasItems)
                {

                    tabCtrl.Visibility = Visibility.Hidden;

                }
                CommonData.mainwindow.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
            }
        }


        private void txtSerialNumbr_TextInput(object sender, TextChangedEventArgs e)
        {
            if (txtSerialNumbr.Text.StartsWith("207"))
            {
                lblWellNumber.Visibility = Visibility.Visible;
                txtWellNumber.Visibility = Visibility.Visible;
            }
            else
            {
                lblWellNumber.Visibility = Visibility.Hidden;
                txtWellNumber.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonData.mainwindow.changeProgressBarValue(0);
            CommonData.mainwindow.changeProgressBarTag("");
            CardManager cm = new CardManager();
            string cardnumber = "";
            if (txtSerialNumbr.Text.StartsWith("207"))
            {
                if (txtSerialNumbr.Text.Length != 8)
                {
                    System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message28);
                    return;
                }
                else
                {
                    if (txtWellNumber.Text.Length > 0)
                    {
                        string Res = SaveNotExistsMeterNumber(txtSerialNumbr.Text);
                        if (Res == "true")
                            return;
                       else if(Res == "NO Allowed")
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message17);
                            this.Close();
                            return;
                        }
                        else
                        {
                            string MeterCustomerORGroup = CheckMeterForCustomerandGroup(txtSerialNumbr.Text);
                            string MeterNumber= txtSerialNumbr.Text;
                            if (MeterCustomerORGroup=="False" || MeterCustomerORGroup=="NO Allowed")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message17);
                                this.Close();
                                return;
                            }
                            else if(MeterCustomerORGroup== "NO Customer")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message93);
                                CommonData.mainwindow.ShowMeters(1, MeterNumber, false);
                                //this.Close();
                                return;
                            }
                            else if(MeterCustomerORGroup== "NO Group")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message94);
                                CommonData.mainwindow.ShowMeters(1, MeterNumber, false);
                                //this.Close();
                                return;
                            }
                            else if (MeterCustomerORGroup == "NO Group and Customer")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message96);
                                CommonData.mainwindow.ShowMeters(1, MeterNumber, false);
                               // this.Close();
                                return;
                            }

                            int m=cm.WriteNew207Card(txtWellNumber.Text, CommonData.Citycode,ref cardnumber);
                            CardReadOut cr = cm.getCardData();
                            if (m==-1)
                            {
                                 CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                                 MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                                return;
                            }
                            else if (m == 0)
                            {
                                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                                return;
                            }
                            else if (m == 2) //شماره کارت صفر
                            {
                                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message50);
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message50);
                                return;
                            }
                            else if (m == 1 )//|| m == 2)
                            {

                                CommonData.mainwindow.changeProgressBarValue(1000);
                                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                                ObjectParameter CardID = new ObjectParameter("CardID", 1000000000000);
                                ObjectParameter Result = new ObjectParameter("Result", 1000000000000);
                                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", 1000000000000);
                                SQLSPS.INSCards(cardnumber, CardID, Result, ErrMSG);

                                ObjectParameter MeterID = new ObjectParameter("MeterID", 1000000000000);
                                Result = new ObjectParameter("Result", 1000000000000);
                                ErrMSG = new ObjectParameter("ErrMSG", 1000000000000);

                                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                                SQLSPS.ShowMeterNumber(CommonData.MeterNumber, IsExits, Result, ErrMSG);
                                if (!Convert.ToBoolean(IsExits.Value))
                                    SQLSPS.InsMeter(txtSerialNumbr.Text, null, true,null, null, null, null, 1,1, true, MeterID, ErrMSG, Result);
                                ShowMeter_Result MeterID1 = new ShowMeter_Result();
                                MeterID1 = SQLSPS.ShowMeter(" and Main.MeterNumber = '" + txtSerialNumbr.Text + "'");
                                
                                Result = new ObjectParameter("Result", 1000000000000);
                                ErrMSG = new ObjectParameter("ErrMSG", 1000000000000);
                                SQLSPS.InsCardToMeter(Convert.ToDecimal(CardID.Value),MeterID1.MeterID, DateTime.Now.ToPersianString(), CommonData.UserID, Result, ErrMSG);
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message85.ToString());
                                this.Close();

                            }
                            else if (m == 3)
                            {
                                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message65);
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message65);
                                return;
                            }
                            else if (m == 5)
                            {
                                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message42);
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message42);
                                return;
                            }

                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message30);
                        return;
                    }
                }
            }
            else
            {


                if (txtSerialNumbr.Text.StartsWith("19"))
                {
                    if (txtSerialNumbr.Text.Length == 11 || txtSerialNumbr.Text.Length == 13)
                    {
                        string Result = SaveNotExistsMeterNumber(txtSerialNumbr.Text);
                        if (Result == "true")
                            return;
                        else if (Result == "NO Allowed")
                        {
                            MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message17);
                            return;
                        }


                        else
                        {
                            string MeterCustomerORGroup = CheckMeterForCustomerandGroup(txtSerialNumbr.Text);
                            string MeterNumber = txtSerialNumbr.Text;
                            if (MeterCustomerORGroup == "False" || MeterCustomerORGroup == "NO Allowed")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message17);
                                this.Close();
                                return;
                            }
                            else if (MeterCustomerORGroup == "NO Customer")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message93);
                                CommonData.mainwindow.ShowMeters(1, MeterNumber, false);
                                //this.Close();
                                return;
                            }
                            else if (MeterCustomerORGroup == "NO Group")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message94);
                                CommonData.mainwindow.ShowMeters(1, MeterNumber, false);
                                //this.Close();
                                return;
                            }
                            else if (MeterCustomerORGroup == "NO Group and Customer")
                            {
                                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message96);
                                CommonData.mainwindow.ShowMeters(1, MeterNumber, false);
                                //this.Close();
                                return;
                            }

                            create303Newcard(txtSerialNumbr.Text);
                        }
                    }

                    else
                    {
                        //"لطفا شماره کنتور را به درستی وارد کنید"
                        System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message28);
                        return;
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message28);
                    return;
                }
            }
        }
        public string  SaveNotExistsMeterNumber(string MeterNumber)
        {
            try
            {
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");
                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                ObjectParameter IsAllow = new ObjectParameter("IsAllow", false);
                SQLSPS.ShowMeterNumberIssuance(MeterNumber, IsExits, IsAllow, Result, ErrMSG);
                if (!Convert.ToBoolean(IsExits.Value))
                {

                    MessageBoxResult result = MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message32, "", MessageBoxButton.OK);
                    if (result == MessageBoxResult.OK)
                    {
                        CommonData.mainwindow.ShowMeters(1, MeterNumber, true);
                       // this.Close();
                        return "true";
                    }
                }
                else if (Convert.ToBoolean(IsExits.Value) && !Convert.ToBoolean(IsAllow.Value))
                    return "NO Allowed";
               
                return "false";

            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message.ToString());                         CommonData.WriteLOG(ex);
                return "false";
            }
        }

        public string CheckMeterForCustomerandGroup(string meternumber)
        {
            try
            {
                ObjectParameter Result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", "");               
                ObjectParameter IsAllow = new ObjectParameter("IsAllow", false);
                ObjectParameter CustomerID = new ObjectParameter("CustomerID", 10000000000000000);
                ObjectParameter GroupID = new ObjectParameter("GroupID", 10000000000000000);


                ShowMeter_Result Meter = new ShowMeter_Result();
                Meter = SQLSPS.ShowMeter(" and Main.MeterNumber='" + meternumber + "'");
                if (Meter == null) Meter.MeterID = 0;

                    SQLSPS.ShowCustomerIDMeterID(Meter.MeterID, IsAllow, CustomerID, GroupID, Result, ErrMSG);
                if (Convert.ToBoolean(IsAllow.Value))
                {
                    if (NumericConverter.IntConverter(CustomerID.Value.ToString())!=0)
                    {
                        if (NumericConverter.IntConverter(GroupID.Value.ToString()) != 0)
                            return "True";
                        else
                            return "NO Group";
                    }
                    else
                    {
                        if (NumericConverter.IntConverter(GroupID.Value.ToString()) != 0)
                            return "NO Customer";
                        else
                            return "NO Group and Customer";
                    }
                }
                else
                    return "NO Allowed";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString().ToString()); CommonData.WriteLOG(ex);
                return "False";
            }
        }



        void create303Newcard(string serialNumber)
        {
            CommonData.mainwindow.changeProgressBarValue(0);

            CardManager cm = new CardManager();
            string cardnumber="";

            string LogicalName = "RSASEWM303";

            string ch = serialNumber.Substring(2, 1);

            if (ch != "4" && ch != "3")
            {
                //"رقم سوم شماره کنتور عدد 3 یا 4 می باشد"
                System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message31);
                txtSerialNumbr.Text = "";
                return;
            }
            if (ch == "4" && serialNumber.Length == 11)
            {
                LogicalName = "RSASEWM304";

            }
            int meterNo = -1;
            if (serialNumber.Length == 11)
            {
                Int32.TryParse(serialNumber.Substring(5, 6), out meterNo);
                LogicalName = LogicalName + serialNumber.Substring(5, 6);
            }
            if (serialNumber.Length == 13)
            {
                Int32.TryParse(serialNumber.Substring(7, 6), out meterNo);
                LogicalName = LogicalName + serialNumber.Substring(7, 6);
            }


            if (meterNo < 1)
            {
                System.Windows.Forms.MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message28);
                txtSerialNumbr.Text = "";
                return;
            }

            CardReadOut cr = cm.getCardData();
            if (cr.ErrorMessage.CardErrorMessage.Count > 0)
            {
                CommonData.mainwindow.changeProgressBarTag(cr.ErrorMessage.CardErrorMessage[0].message);
                MessageBox.Show(cr.ErrorMessage.CardErrorMessage[0].message);
                return;
            }
            if (cr.SerialNumber != "1197643879")
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message22);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message22);
                return;
            }
            if (cr.CardNumber == "0")
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message23);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message23);
                return;
            }

            int m = cm.WriteNew303Card(LogicalName, CommonData.Citycode, ref cardnumber);
            Thread.Sleep(1000);
            if (m==-1)
            {
                 CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message14);
                return;
            }
            else if (m == 0)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message15);
                return;
            }
            else if (m == 2)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message50);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message50);
                return;
            }
            else if (m == 1)
            {
                CommonData.mainwindow.changeProgressBarValue(1000);
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message24);
                ObjectParameter CardID = new ObjectParameter("CardID", 1000000000000);
                ObjectParameter Result = new ObjectParameter("Result", 1000000000000);
                ObjectParameter ErrMSG = new ObjectParameter("ErrMSG", 1000000000000);
                SQLSPS.INSCards(cr.CardNumber, CardID, Result, ErrMSG);

                ObjectParameter MeterID = new ObjectParameter("MeterID", 1000000000000);
                Result = new ObjectParameter("Result", 1000000000000);
                ErrMSG = new ObjectParameter("ErrMSG", 1000000000000);
                ObjectParameter IsExits = new ObjectParameter("IsExits", false);
                SQLSPS.ShowMeterNumber(CommonData.MeterNumber, IsExits, Result, ErrMSG);
                if(!Convert.ToBoolean(IsExits.Value))
                    SQLSPS.InsMeter(txtSerialNumbr.Text, null, true,null, null, null, null, 1,1, true, MeterID, ErrMSG, Result);

                ShowMeter_Result MeterID1 = new ShowMeter_Result();
                MeterID1 = SQLSPS.ShowMeter(" and Main.MeterNumber = '" + txtSerialNumbr.Text + "'");


                Result = new ObjectParameter("Result", 1000000000000);
                ErrMSG = new ObjectParameter("ErrMSG", 1000000000000);
                SQLSPS.InsCardToMeter(Convert.ToDecimal(CardID.Value), MeterID1.MeterID, DateTime.Now.ToPersianString(), CommonData.UserID, Result, ErrMSG);
                this.Close();
            }
            else if (m == 3)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message65);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message65);
                return;
            }
            else if (m == 4)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message66);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message66);
                return;
            }
            else if (m == 5)
            {
                CommonData.mainwindow.changeProgressBarTag(CommonData.mainwindow.tm.TranslateofMessage.Message42);
                MessageBox.Show(CommonData.mainwindow.tm.TranslateofMessage.Message42);
                return;
            }

        }
               
    }
}