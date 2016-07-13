using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;


namespace MassEffectPlyer
{
    public partial class ViewWindow : Window, IComponentConnector
    {
        public MediaPlayer warn = new MediaPlayer();
        private string saveDeepDirectory;


        public ViewWindow()
        {
            this.InitializeComponent();
            this.TextBox1.Text = TrackListClass.currentPath;
            this.saveDeepDirectory = TrackListClass.currentPath;
            this.showInsideFolder();
            foreach(var driver in DriveInfo.GetDrives())
                comboBox.Items.Add(driver);
        }

        //кнопка закрыть
        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //добавить в треклист
        private void addInTrackList_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            try
            {
                if (Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) == ".mp3" || Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) == ".MP3" || Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) == ".wma" || Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) == ".WMA")
                    TrackListClass.trackList.Add(TrackListClass.generalInside[this.listView1.SelectedIndex]);
                else
                    TrackListClass.audioFromFolderToTrackList(TrackListClass.generalInside[this.listView1.SelectedIndex]);
            }
            catch
            {
            }
        }

        //показать внутри папки
        private void showInsideFolder()
        {
            string currentPath = TrackListClass.currentPath;
            try
            {
                TrackListClass.currentPath = this.TextBox1.Text;
                string[] allInside = TrackListClass.takeAllInside(TrackListClass.currentPath);
                listView1.Items.Clear();
                foreach (string str in allInside)
                {
                    int count = new Regex(Regex.Escape("\\")).Matches(str).Count;
                    if (Path.GetExtension(str) == ".mp3" || Path.GetExtension(str) == ".MP3" || Path.GetExtension(str) == ".wma")
                        this.listView1.Items.Add((object)new FileInFolder(str.Split('\\')[count], this.iconMp3.Source));
                    else
                        this.listView1.Items.Add((object)new FileInFolder(str.Split('\\')[count], this.iconFile.Source));
                }
            }
            catch
            {
                this.warn.Open(new Uri(Environment.CurrentDirectory + "\\music\\Warning-DefaultBeep.wav"));
                this.warn.Play();
                this.TextBox1.Text = TrackListClass.currentPath = currentPath;
            }
        }

        //нажатие кнопки при выборе пути
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
                return;
            this.showInsideFolder();
        }

        private void customWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //движение назад
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            string currentPath = TrackListClass.currentPath;
            if (new Regex(Regex.Escape("\\")).Matches(currentPath).Count > 1)
            {
                int startIndex = currentPath.LastIndexOf('\\');
                TrackListClass.currentPath = null;
                TrackListClass.currentPath = currentPath.Remove(startIndex);
                this.TextBox1.Text = TrackListClass.currentPath;
                this.showInsideFolder();
            }
            else
            {
                int startIndex = currentPath.LastIndexOf('\\');
                TrackListClass.currentPath = null;
                TrackListClass.currentPath = currentPath.Remove(startIndex);
                TrackListClass.currentPath += '\\';
                this.TextBox1.Text = TrackListClass.currentPath;
                this.showInsideFolder();
            }
            for (int index = 0; index < this.listView1.Items.Count; ++index)
            {
                if (currentPath == TrackListClass.generalInside[index])
                {
                    this.listView1.SelectedIndex = index;
                    this.listView1.ScrollIntoView(this.listView1.Items[index]);
                    break;
                }
            }
        }

        //кнопка вперед
        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            string str1 = this.saveDeepDirectory;
            string currentPath = TrackListClass.currentPath;
            if (str1.Equals(currentPath))
                return;
            int length = currentPath.Length;
            if ((int)currentPath[currentPath.Length - 1] != 92)
                ++length;
            string str2 = str1.Remove(0, length);
            int startIndex = str2.IndexOf('\\');
            if (startIndex > 0)
                str2 = str2.Remove(startIndex);
            if ((int)currentPath[currentPath.Length - 1] != 92)
                currentPath += '\\';
            TrackListClass.currentPath = currentPath + str2;
            this.TextBox1.Text = TrackListClass.currentPath;
            this.showInsideFolder();
            try
            {
                listView1.ScrollIntoView(this.listView1.Items[0]);
            }
            catch
            {
            }
        }

        private void listview_previewDoublClik(object sender, MouseButtonEventArgs e)
        {
            for (DependencyObject reference = (DependencyObject)e.OriginalSource; reference != null && reference != this.listView1; reference = VisualTreeHelper.GetParent(reference))
            {
                if (reference.GetType() == typeof(ListViewItem))
                {
                    if (Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) != ".mp3" && Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) != ".MP3" && Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) != ".wma" && Path.GetExtension(TrackListClass.generalInside[this.listView1.SelectedIndex]) != ".WMA")
                    {
                        this.TextBox1.Text = this.saveDeepDirectory = TrackListClass.generalInside[this.listView1.SelectedIndex];
                        this.showInsideFolder();
                        try
                        {
                            this.listView1.ScrollIntoView(this.listView1.Items[0]);
                        }
                        catch
                        {

                        }
                        break;
                    }
                    Sounds.clikSoundField();
                    TrackListClass.trackList.Clear();
                    TrackListClass.trackList.Add(TrackListClass.generalInside[this.listView1.SelectedIndex]);
                    this.Close();
                    break;
                }
            }
        }

        //выбор диска в комбобокс
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TrackListClass.currentPath = comboBox.SelectedItem.ToString();
            this.TextBox1.Text = saveDeepDirectory = TrackListClass.currentPath;
            listView1.Items.Clear();
            showInsideFolder();
        }

        public class FileInFolder
        {
            public string FileName { get; set; }

            public ImageSource image { get; set; }

            public FileInFolder(string name, ImageSource img)
            {
                this.FileName = name;
                this.image = img;
            }
        }

        
    }
}