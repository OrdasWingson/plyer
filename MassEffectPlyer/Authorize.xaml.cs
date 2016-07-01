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
using System.Net;
using System.IO;
using System.Xml;
using System.Threading;

namespace MassEffectPlyer
{
    /// <summary>
    /// Логика взаимодействия для Authorize.xaml
    /// </summary>
    public partial class Authorize : Window
    {
        string strAdr;
        string usName ="";

        public Authorize()
        {
            InitializeComponent();
            webBrowser1.Visibility = Visibility.Hidden;
            webBrowser1.LoadCompleted += WebBrowser1_LoadCompleted;
            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=5231153&display=popup&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.52");
        }

        private void WebBrowser1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
               
                textBlock.Text = "СВЯЗЬ С ЦИТАДЕЛЬЮ УСТАНОВЛЕНА!";
                
                webBrowser1.Visibility = Visibility.Visible;
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
                
                responsName();
                textBlock.Text = "Добро пожаловать на борт "+ usName + " Шепaрд";
                
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

        private void responsName()
        {
            WebResponse response = WebRequest.Create("https://api.vk.com/method/users.get.xml?user_ids=" + VKUserInfo.id + "&fields=bdate&v=5.52").GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();

            XmlDocument xm = new XmlDocument();
            xm.LoadXml(string.Format(end));

            XmlNodeList nodeList = xm.GetElementsByTagName("first_name");

            foreach (XmlElement t in nodeList)
            {
                usName = t.InnerText;
            }



            //usName = JToken.Parse(System.Web.HttpUtility.HtmlDecode(end))["response"].Children().Skip(1).Select(c => c.ToObject<user>()).ToList();
            //MessageBox.Show(usName[0].first_name);
        }
    }

    public class user
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

    }
}

    


