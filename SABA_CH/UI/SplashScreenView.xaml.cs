using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;

namespace SABA_CH.UI
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : System.Windows.Window
    {
        Thread loadingThread;
        Storyboard Showboard;
        Storyboard Hideboard;
        private delegate void ShowDelegate(string txt);
        private delegate void HideDelegate();
        ShowDelegate showDelegate;
        HideDelegate hideDelegate;
        public SplashScreenView()
        {
            InitializeComponent();
            showDelegate = new ShowDelegate(this.showText);
            hideDelegate = new HideDelegate(this.hideText);
            Showboard = this.Resources["showStoryBoard"] as Storyboard;
            Hideboard = this.Resources["HideStoryBoard"] as Storyboard;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadingThread = new Thread(load);
            loadingThread.Start();             
        }
        private void load()
        {
            //DBAccess DB;
            Thread.Sleep(1000);
            this.Dispatcher.Invoke(showDelegate, "Initialization data to loading");
            Thread.Sleep(500);
            

            this.Dispatcher.Invoke(hideDelegate);
            Thread.Sleep(500);
            
        }
        private void showText(string txt)
        {
            //txtLoading.Text = txt;
            BeginStoryboard(Showboard);
        }



        private void hideText()
        {
            BeginStoryboard(Hideboard);
        }

    }
}
