﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls.Primitives;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MassEffectPlyer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MediaPlayer warn = new MediaPlayer(); //плеер для сигналов
        private MediaState mediaState = MediaState.Stop; //состояние основного плеера
        private int trackNumber = 0; //номер трека
        private ListBoxItem lbi = new ListBoxItem(); //переменная для работы с пунктами listbox
        private List<string> videoPathList = new List<string>(); //контейнер адресов видеотреков 
        public DispatcherTimer timer = new DispatcherTimer(); //таймер, не помню для чего
        public static bool saveTrack; //сохранение треков
        public List<Audio> audioInfo = new List<Audio>();//для аудиотреков вк
        private bool vkStatus = false;//для адиотреков вк
        public static string vkPathSave;//директория для сохранения песен
        private bool isConnect = false;
        private string[] prohibitedArr = { "*", "|", "\\", "/", ":", "\"", "?", ">", "<"};


        //иконки разного состаяния звука
        private ImageBrush soundFull = new ImageBrush();
        private ImageBrush soundMiddle = new ImageBrush();
        private ImageBrush soundLow = new ImageBrush();
        private ImageBrush soundOff = new ImageBrush();

        private double saveSoudValue = 0.0; //уровень звук
        private bool trackBarMouseIsClik = false; //логическая переменная для перетаскивания ползунка прокрутки

        //для сервера
        //private delegate void addDelegate(); // делегат для работы с ListBoxMusic 
        //private addDelegate addListBoxDelegate; // делегат для работы с ListBoxMusic
        

        public MainWindow()
        {
            InitializeComponent();
            Sounds.clickSound.Volume = 1.0; //громкость нажатия клавишь          
            warn.Volume = 1.0; //громкость предупреждений
            TrackListClass.currentPath = "C:\\Users\\" + Environment.UserName + "\\Music"; //автоматический адрес при старте
            sliderVolume.Maximum = 1.0; //устанавливает максимальное значение слайдера прокрутки звука
            sliderVolume.Value = 0.5; //устанавливает значение при запуске
            media.Volume = this.sliderVolume.Value; //громкость звука аудиотреков

            //работа с инициализацией
            MainWindow.saveTrack = true; //сохранять треки при выключении
            Sounds.soundOnOff = true; //звук включен

            //переменные иконки звука
            soundFull.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/soundfull.png"));
            soundMiddle.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/soundmiddle.png"));
            soundLow.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/soundlow.png"));
            soundOff.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/soundoff.png"));

            ////переменные каталога видеотреков
            videoPathList.Add(AppDomain.CurrentDomain.BaseDirectory + "video\\load.avi");
            videoPathList.Add(AppDomain.CurrentDomain.BaseDirectory + "video\\dance1.avi");

            if (TrackListClass.trackList.Count != 0)
                addToLisBoxMusic();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);//событие запуска окна
            ListBoxMusic.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged); //делегат события изминния контейнера listBox
            media.MediaEnded += new RoutedEventHandler(media_MediaEnded);//событие окончания аудио
            videoPlayer.Source = new Uri(this.videoPathList[0]);//загрузка в видиоплеер пути к видеофайла
            videoPlayer.MediaEnded += new RoutedEventHandler(videoPlayer_MediaEnded);//событие окночания видео
            
        }

        //событие окончания видео
        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.mediaState == MediaState.Stop || this.mediaState == MediaState.Close || this.mediaState == MediaState.Pause)
                this.videoPlayer.Source = new Uri(this.videoPathList[0]);
            else
                this.videoPlayer.Source = new Uri(this.videoPathList[1]);
        }


        //событие происходящее после оканчания проигрывания трека
        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                textOptionsListBox();
                media.Close();
                mediaState = MediaState.Close;
                trackBar.Value = 0.0;
                videoPlayer.Source = new Uri(videoPathList[0]);          
                playFunction(++trackNumber);
               
            }
            catch
            {
            }
        }


        // метод-делегат  события изменения данных в контейнере ListBox
        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (mediaState != MediaState.Play && mediaState != MediaState.Pause)
                return;

            if (trackNumber >= TrackListClass.trackList.Count)
                trackNumber = TrackListClass.trackList.Count - 1;

            for (int index = trackNumber; index >= 0; --index)
            {
                if (media.Source.LocalPath == TrackListClass.trackList[index])
                {
                    trackNumber = index;
                    break;
                }
            }

            if (ListBoxMusic.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                try
                {
                    this.lbi = (ListBoxItem)this.ListBoxMusic.ItemContainerGenerator.ContainerFromIndex(this.trackNumber);
                    this.lbi.FontWeight = FontWeights.ExtraBold;
                    this.lbi.FontSize = 14.0;
                }
                catch
                {
                }
            }
        }

        //событие запуска окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //new Thread(new ThreadStart(this.serverArgs))
            //{
            //    IsBackground = true
            //}.Start();
            //playFunction(0);
            inicialisationFile();
            MemoryXML mem = new MemoryXML();

            if (mem.GetTracksFromXML() != null && MainWindow.saveTrack)
            {
                TrackListClass.trackList = mem.GetTracksFromXML();
                addToLisBoxMusic();
            }
            
        }

        //функция перетаскивания окна с помощью мыши
        private void customWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Щелчек по пункту ListView
        private void clickMouse(object sender, MouseButtonEventArgs e)
        {
            for (DependencyObject reference = (DependencyObject)e.OriginalSource; reference != null && reference != ListBoxMusic; reference = VisualTreeHelper.GetParent(reference))
            {
                if (reference.GetType() == typeof(ListBoxItem))
                {
                    textOptionsListBox();
                    trackNumber = ListBoxMusic.SelectedIndex;
                    playFunction(this.ListBoxMusic.SelectedIndex);
                    break;
                }
            }
        }

        //нажатие кнопки Закрыть
        private void Close_Buttn_Click(object sender, RoutedEventArgs e)
        {
            saveInitFile();
            if (MainWindow.saveTrack && !vkStatus)
            {
                saveMethodToXML();
            }
            Sounds.clikSoundField();
            
            this.media.Stop();
            this.videoPlayer.Source = new Uri(this.videoPathList[0]);
            this.warn.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "music\\Exit.wav"));
            this.warn.Play();
            Thread.Sleep(1800);
            Application.Current.Shutdown();
        }

        //изменение громкости звука
        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = sliderVolume.Value;
            if (sliderVolume.Value >= 0.6)
                soundButn.Background = soundFull;
            if (sliderVolume.Value >= 0.3 && sliderVolume.Value < 0.6)
                soundButn.Background = soundMiddle;
            if (sliderVolume.Value < 0.3 && sliderVolume.Value > 0.0)
                soundButn.Background = soundLow;
            if (sliderVolume.Value != 0.0)
                return;
            soundButn.Background = soundOff;
        }

        //запуск аудиотрека
        private void mediaOpened(object sender, RoutedEventArgs e)
        {

            

            this.trackBar.Maximum = this.media.NaturalDuration.TimeSpan.TotalSeconds;
            this.startVideoDance();
            if (!vkStatus)
            {
                TagLib.File file = TagLib.File.Create(this.media.Source.LocalPath);
                infoTextBloc.Text = "Название песни: " + file.Tag.Title + "\nИсполнитель: " + string.Join(", ", file.Tag.Performers) + "\nВремя трека: " + file.Properties.Duration.ToString("mm\\:ss");
            }
            else
            {
                TimeSpan elapsedTime = new TimeSpan(0, 0, audioInfo[trackNumber].duration);
                infoTextBloc.Text = "Название песни: " + audioInfo[trackNumber].title + "\nИсполнитель: " + audioInfo[trackNumber].artist + "\nВремя трека: " + elapsedTime.ToString();
            }
        }


        //добавления трека втреклист
        private void addToLisBoxMusic()
        {
            ListBoxMusic.Items.Clear();
            foreach (string track in TrackListClass.trackList)
            {
                int count = new Regex(Regex.Escape("\\")).Matches(track).Count;
                ListBoxMusic.Items.Add((object)track.Split('\\')[count]);
            }
        }

        //обработка нажатия кнопки пауза
        private void PauseButtn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
        }


        //функция запуска проигрывания музыки
        private void playFunction(int indexTrack)
        {
            try
            {
                media.Source = new Uri(TrackListClass.trackList[indexTrack]);
                timer.Tick += new EventHandler(SearchSliderPositionThread);
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Start();
                media.Play();
                mediaState = MediaState.Play;
                ViewButn.Content = (object)"II";
                lbi = (ListBoxItem)ListBoxMusic.ItemContainerGenerator.ContainerFromIndex(indexTrack);
                lbi.FontWeight = FontWeights.ExtraBold;
                lbi.FontSize = 14.0;
                //this.messageDonat(true);
            }
            catch
            {
            }
        }  

        //метод изменения положения ползунка таймера с течением времени
        private void SearchSliderPositionThread(object sender, EventArgs e)
        {
            if (trackBarMouseIsClik)
                return;
            trackBar.Value = this.media.Position.TotalSeconds;
            TextBlock textBlock = mediaTimer;
            TimeSpan timeSpan = media.Position;
            timeSpan = TimeSpan.FromMinutes(timeSpan.TotalSeconds);
            string @string = timeSpan.ToString();
            textBlock.Text = @string;
        }

        //метод события при щелчке на ползунке слайдера прокрутки аудиотрека
        private void trackBar_MouseClick(object sender, MouseButtonEventArgs e)
        {
            trackBarMouseIsClik = true;
        }

        //метод события при отпускании левой кнопки мыши при перетаскивании ползунка
        private void trackBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            media.Position = TimeSpan.FromSeconds(trackBar.Value);
            trackBarMouseIsClik = false;
        }

        //метод визуального изменения текста в треклисте 
        private void textOptionsListBox()
        {
            try
            {
                lbi = (ListBoxItem)this.ListBoxMusic.ItemContainerGenerator.ContainerFromIndex(this.trackNumber);
                lbi.FontWeight = FontWeights.Normal;
                lbi.FontSize = 12.0;
            }
            catch
            {
            }
        }

        //Запускает сервер
       /* private void serverArgs()
        {
            IPAddress address = Dns.GetHostEntry("localhost").AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(address, 11000);
            Socket socket1 = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket1.Bind((EndPoint)ipEndPoint);
                socket1.Listen(10);
                while (true)
                {
                    Socket socket2 = socket1.Accept();
                    string str1 = (string)null;
                    byte[] numArray = new byte[1024];
                    int count = socket2.Receive(numArray);
                    string str2 = str1 + Encoding.UTF8.GetString(numArray, 0, count);
                    TrackListClass.trackList.Add(str2);
                    socket2.Shutdown(SocketShutdown.Both);
                    socket2.Close();
                    Application.Current.Dispatcher.Invoke(addListBoxDelegate);
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("FUCK " + ex.ToString());
            }
        }*/

        //метод запуска проигрывания видео
        private void startVideoDance()
        {
            videoPlayer.Source = new Uri(this.videoPathList[1]);
            videoPlayer.Position = TimeSpan.FromSeconds((double)new Random().Next(380));
        }

        //обработка событиц кнопки старт(нажатие на кнопку)
        private void StartButn_Click(object sender, RoutedEventArgs e)
        {
            if (mediaState != MediaState.Play && mediaState != MediaState.Pause)
            {
                Sounds.clikSoundField();
                trackNumber = this.ListBoxMusic.SelectedIndex;
                playFunction(this.ListBoxMusic.SelectedIndex);
                
            }
            else
            {
                if (this.mediaState == MediaState.Play)
                {
                    this.media.Pause();
                    ViewButn.Content = (object)"►";
                    this.mediaState = MediaState.Pause;
                    this.videoPlayer.Source = new Uri(this.videoPathList[0]);
                }
                else if (this.mediaState == MediaState.Pause)
                {
                    this.media.Play();
                    ViewButn.Content = (object)"II";
                    this.mediaState = MediaState.Play;
                    this.startVideoDance();
                }
            }
        }

#region методы работы с забрасывание музыки***********************************************************

        private void mainWindow_PreviewDrop(object sender, DragEventArgs e)
        {
            if (!vkStatus)
            {
                foreach (string str in (string[])e.Data.GetData(DataFormats.FileDrop, true))
                {
                    if (File.Exists(str))
                    {
                        FileInfo fileInfo = new FileInfo(str);

                        if (fileInfo.Extension == ".mp3" || fileInfo.Extension == ".MP3" || fileInfo.Extension == ".wma" || fileInfo.Extension == ".WMA")
                        {
                            TrackListClass.trackList.Add(str);
                        }
                    }

                    if (Directory.Exists(str))
                    {
                        string[] mp3ins = Directory.GetFileSystemEntries(str, "*.mp3");
                        foreach (var mp3 in mp3ins)
                        {
                            TrackListClass.trackList.Add(mp3);
                        }
                    }


                }
                this.addToLisBoxMusic();
            }
           
        }
       
        #endregion*****************************************************

        //нажатие кнопки назад
        private void backTrackButtn_Click(object sender, RoutedEventArgs e)
        {
            textOptionsListBox();
            if (--trackNumber < 0)
                trackNumber = 0;
            playFunction(trackNumber);
        }

        //нажатие кнопки следующий трек
        private void nextTrackButtn_Click(object sender, RoutedEventArgs e)
        {
            textOptionsListBox();
            playFunction(++trackNumber);
        }

        //нажатие клавиши изменения звука
        private void soundButn_Click(object sender, RoutedEventArgs e)
        {
            if (sliderVolume.Value > 0.0)
            {
                saveSoudValue = sliderVolume.Value;
                sliderVolume.Value = 0.0;
            }
            else
                sliderVolume.Value = saveSoudValue;
        }

        //кнопка свернуть
        private void min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //кнопка перемешать
        private void mixBtn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            if (!vkStatus)
            {
                TrackListClass.mixerFunc();
                if (this.mediaState == MediaState.Play || this.mediaState == MediaState.Pause)
                {
                    string localPath = this.media.Source.LocalPath;
                    for (int index = 0; index < TrackListClass.trackList.Count; ++index)
                    {
                        if (TrackListClass.trackList[index] == localPath)
                        {
                            string str = TrackListClass.trackList[0];
                            TrackListClass.trackList[0] = localPath;
                            TrackListClass.trackList[index] = str;
                            break;
                        }
                    }
                }

                this.addToLisBoxMusic();
                this.trackNumber = 0;
            }
            else
            {
                int currentTrackID = audioInfo[trackNumber].aid;
                TrackListClass.mixerFunc<Audio>(audioInfo);

                if (this.mediaState == MediaState.Play || this.mediaState == MediaState.Pause)
                {
                    for (int i = 0; i < audioInfo.Count; i++)
                    {
                        if (audioInfo[i].aid == currentTrackID)
                        {
                            var newVar = audioInfo[0];
                            audioInfo[0] = audioInfo[i];
                            audioInfo[i] = newVar;
                            trackNumber = 0;
                            break;
                        }
                    }
                }
                
                
                TrackListClass.trackList.Clear();
                ListBoxMusic.Items.Clear();

                for (var i = 0; i < audioInfo.Count; i++)
                {
                    TrackListClass.trackList.Add(audioInfo[i].url);
                    ListBoxMusic.Items.Add(audioInfo[i].artist + "-" + audioInfo[i].title);
                }
                
            }

        }

        //событие кнопки очистить
        private void clearButn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            if (!vkStatus)
            {
                ClearMethod();
            }
            else
            {
                try
                {
                    string selectedTrack = TrackListClass.trackList[ListBoxMusic.SelectedIndex];
                    string trackFromListBox = (string)ListBoxMusic.Items[ListBoxMusic.SelectedIndex]; //prohibitedArr

                    foreach (var prohibited in prohibitedArr)
                    {
                        trackFromListBox = trackFromListBox.Replace(prohibited, " ");
                    }


                    if (!Directory.Exists(vkPathSave))
                    {
                        Directory.CreateDirectory(vkPathSave);
                    }

                    var newThread = new Thread(() => 
                    {
                        WebClient web = new WebClient();
                        web.DownloadFile(selectedTrack, vkPathSave + "\\" + trackFromListBox + ".mp3");
                    });
                    newThread.IsBackground = true;
                    newThread.Start();
                    
                }
                catch
                {

                }
            }
        }

        //метод очистки
        private void ClearMethod()
        {
            this.media.Stop();
            this.mediaState = MediaState.Stop;
            TrackListClass.trackList.Clear();
            this.ListBoxMusic.Items.Clear();
            this.media.Source = (Uri)null;
            this.videoPlayer.Source = new Uri(this.videoPathList[0]);
            this.infoTextBloc.Text = "";
        }

        //перехват нажатия кнопки основным 
        private void windowForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete || vkStatus)
                return;
            try
            {
                if (this.media.Source != (Uri)null && this.media.Source.LocalPath == TrackListClass.trackList[this.ListBoxMusic.SelectedIndex])
                {
                    this.media.Stop();
                    this.mediaState = MediaState.Stop;
                    
                    this.videoPlayer.Source = new Uri(this.videoPathList[0]);
                    
                }
                int selectedIndex = this.ListBoxMusic.SelectedIndex;
                TrackListClass.trackList.RemoveAt(this.ListBoxMusic.SelectedIndex);
                this.addToLisBoxMusic();
                this.ListBoxMusic.SelectedIndex = selectedIndex;
            }
            catch
            {
            }
        }

        //кнопка включения окна обзора
        private void ViewButtn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            ViewWindow viewWindow = new ViewWindow();
            Visibility = Visibility.Hidden;
            viewWindow.ShowDialog();
            Visibility = Visibility.Visible;
            try
            {
                this.addToLisBoxMusic();
            }
            catch
            {
                //this.WarningMess("Каталог не выбран, пуст или не действителен");
            }
            if (TrackListClass.trackList.Count != 1)
                return;
            this.playFunction(0);
        }

        //кнопка запуска меню
        private void setting_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            SettingWindow setting = new SettingWindow(this.Top, this.Left);
            setting.Owner = this;           
            setting.ShowDialog();
        }

        //загружает данные из файла
        private void inicialisationFile()
        {
            string[] iniString = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "data\\MEP.ini");
            
            string saveTrack = iniString[0].Split('=')[1];
            string sound = iniString[1].Split('=')[1];
            string path = iniString[2].Split('=')[1];
            vkPathSave = iniString[3].Split('=')[1];
            if (!Directory.Exists(path) || path == null)
            {
                path = TrackListClass.currentPath;
            }
            else
            {
                TrackListClass.currentPath = path;
            }
            MainWindow.saveTrack = Boolean.Parse(saveTrack);
            Sounds.soundOnOff = Boolean.Parse(sound);

            if (Sounds.soundOnOff)
                Sounds.setVolume(1.0);
            else
                Sounds.setVolume(0.0);
           
        }

        //записывает данные в фаил
        private void saveInitFile()
        {
            string[] iniString = { "Save=" + MainWindow.saveTrack.ToString(), "Sound=" + Sounds.soundOnOff.ToString(),
                "Path=" + TrackListClass.currentPath, "PathVK="+vkPathSave };

            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "data\\MEP.ini", iniString);
        }

        //запуск окна авторизации
        private void VKButn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            if (vkStatus == false)
            {
                
                Authorize autorize = new Authorize();
                Visibility = Visibility.Hidden;
                autorize.Show();
                
                autorize.Closed += Autorize_Closed;
                //Autorize_Closed();
                Start_Buttn.Visibility = Visibility.Hidden;
                ImageBrush butnChange = new ImageBrush();
                butnChange.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/butn3.png"));
                VKButn.Background = butnChange;                
                ListBoxMusic.Items.Clear();               
                if (isConnect) ListBoxMusic.Items.Add("Идет загрузка треков...");
                VKButn.Content = "МОИ АУДИО";
                clearButn.Content = "Сохранить песню";
            }
            else
            {
                ClearMethod();
                MemoryXML mem = new MemoryXML();
                if (mem.GetTracksFromXML() != null && MainWindow.saveTrack)
                {
                    TrackListClass.trackList = mem.GetTracksFromXML();
                    addToLisBoxMusic();                  
                }

                vkStatus = false;
                ImageBrush butnChange = new ImageBrush();
                butnChange.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/butn1.png"));
                VKButn.Background = butnChange;
                Start_Buttn.Visibility = Visibility.Visible;
                VKButn.Content = "Мои аудио ВК";
                clearButn.Content = "Очистить список";
            }          

        }

        //закрытие окна авторизации
        private void Autorize_Closed(object sender, EventArgs e)//object sender, EventArgs e
        {
            vkStatus = true;           
            Visibility = Visibility.Visible;
            WebResponse response = WebRequest.Create("https://api.vk.com/method/audio.get?owner_id=" + VKUserInfo.id + "&access_token=" + VKUserInfo.token).GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();
            saveMethodToXML();

            ClearMethod();

            try
            {
                audioInfo = JToken.Parse(System.Web.HttpUtility.HtmlDecode(end))["response"].Children().Skip(1).Select(c => c.ToObject<Audio>()).ToList();
                for (var i = 0; i < audioInfo.Count; i++)
                {
                    TrackListClass.trackList.Add(audioInfo[i].url);
                    ListBoxMusic.Items.Add(audioInfo[i].artist + "-" + audioInfo[i].title);
                }
                isConnect = true;
            }
            catch
            {
                ListBoxMusic.Items.Add("Ошибка подключения к серверу.");
                isConnect = false;
            }
            

        }

        //метод сохранения данных в xml
        private static void saveMethodToXML()
        {
            MemoryXML mem = new MemoryXML();
            mem.SaveTracksToXML(TrackListClass.trackList);
        }

        //перемещение элементов cпомощью клавиш
        private void ListBoxMusic_keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                if (e.Key == Key.Up)
                {
                    var trac = TrackListClass.trackList[ListBoxMusic.SelectedIndex];
                    TrackListClass.trackList[ListBoxMusic.SelectedIndex] = TrackListClass.trackList[ListBoxMusic.SelectedIndex - 1];
                    TrackListClass.trackList[ListBoxMusic.SelectedIndex - 1] = trac;
                    ListBoxMusic.Items.Clear();
                    addToLisBoxMusic();
                }
            }
        }
    }


    //класс жля хранения информации о пользователе
    static class VKUserInfo
    {
        public static string token { get; set; }
        public static string id { get; set; }
        public static bool auth { get; set; }
    }

    //класс для работы с вк аудио
    public class Audio
    {
        public int aid { get; set; }

        public int owner_id { get; set; }

        public string artist { get; set; }

        public string title { get; set; }

        public int duration { get; set; }

        public string url { get; set; }

        public string lurlcs_id { get; set; }

        public int genre { get; set; }
    }
}



