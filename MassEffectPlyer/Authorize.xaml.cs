using System;
using System.Collections.Generic;
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

namespace MassEffectPlyer
{
    /// <summary>
    /// Логика взаимодействия для Authorize.xaml
    /// </summary>
    public partial class Authorize : Window
    {
        string strAdr;
        public Authorize()
        {
            InitializeComponent();
            webBrowser1.LoadCompleted += WebBrowser1_LoadCompleted;
            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=5231153&display=popup&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.52");
        }

        private void WebBrowser1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                strAdr = webBrowser1.Source.AbsoluteUri.ToString().Split('#')[1];
                if (strAdr == null)
                {
                    MessageBox.Show(strAdr.Length.ToString() + " !!! " + strAdr);
                    return;
                }
                VKUserInfo.id = strAdr.Split('=')[3];
                VKUserInfo.token = strAdr.Split('&')[0].Split('=')[1];
                VKUserInfo.auth = true;
                webBrowser1.Visibility = Visibility.Hidden;
                this.Close();
                //MessageBox.Show(strAdr + "  --  " + VKUserInfo.id + "  -2-  " + VKUserInfo.token);
            }
            catch
            {

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(strAdr + "  --  " + VKUserInfo.id + "  -2-  " + VKUserInfo.token);
            this.Close();
        }

        private void authorWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
