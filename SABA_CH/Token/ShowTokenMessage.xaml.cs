using System.Windows;

namespace SABA_CH.Token
{
    /// <summary>
    /// Interaction logic for ShowTokenMessage.xaml
    /// </summary>
    public partial class ShowTokenMessage : System.Windows.Window
    {
        public bool DialogResult = false;
        public ShowTokenMessage()
        {
            InitializeComponent();
        
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.DialogResult DialogR = new System.Windows.Forms.DialogResult();
            DialogResult = true;

        }

        private void NOButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
