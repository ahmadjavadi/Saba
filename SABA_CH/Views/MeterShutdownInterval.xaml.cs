using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SABA_CH.DataBase;
using SABA_CH.Global;

namespace SABA_CH.Views
{
    /// <summary>
    /// Interaction logic for MeterShutdownInterval.xaml
    /// </summary>
    public partial class MeterShutdownInterval : System.Windows.Window
    {
        private static MeterShutdownInterval _instance;
        public static MeterShutdownInterval Instance
        {
            get { return _instance == null ? new MeterShutdownInterval() : _instance; }
        }

        public bool? WasTheMeterOff { get; private set; }

        public MeterShutdownInterval()
        {
            InitializeComponent();
           
        }

       

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObjectParameter result = new ObjectParameter("Result", 10000000000000000);
                ObjectParameter errMsg = new ObjectParameter("ErrMSG", "");

                RsaDateTime.PersianDate pc1 = new RsaDateTime.PersianDate();
                RsaDateTime.PersianDate pc2 = new RsaDateTime.PersianDate();
                var ps = pc1.ConvertToPersianDate(datePickerStart.Text);
                var pe = pc2.ConvertToPersianDate(datePickerEnd.Text);
                
                
                int m1 = 0;
                if (ps.Year == pe.Year)
                {
                    m1 = pe.Month - ps.Month;
                    if (m1 < 0)
                    {
                        System.Windows.Forms.MessageBox.Show(" تاریخ شروع بعد از تاریخ پایان است");
                        return;
                    }
                }
                else if (pe.Year > ps.Year)
                {
                    m1 = 12 + pe.Month - ps.Month;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(" تاریخ شروع بعد از تاریخ پایان است");
                    return;
                }
                SQLSPS.InsMeterShutdownInterval(CommonData.CardMeterID, datePickerStart.Text, datePickerEnd.Text, CommonData.UserID, m1, errMsg, result);
                CommonData.MeterShutdownStartDate = ps;
                CommonData.MeterShutdownEndDate = pe;
                CommonData.MeterShutdownMonths = m1;
                CommonData.WasTheMeterOff = MeterIsShutdownCheckBox.IsChecked;
                this.Close();
            }
            catch (Exception ex)
            {                 
            }                      
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
