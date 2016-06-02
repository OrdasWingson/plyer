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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls.Primitives;
using System.IO;

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
        
        //иконки разного состаяния звука
        private ImageBrush soundFull = new ImageBrush();
        private ImageBrush soundMiddle = new ImageBrush();
        private ImageBrush soundLow = new ImageBrush();
        private ImageBrush soundOff = new ImageBrush();

        private double saveSoudValue = 0.0; //уровень звук
        private bool trackBarMouseIsClik = false; //логическая переменная для перетаскивания ползунка прокрутки
        private delegate void addDelegate(); // делегат для работы с ListBoxMusic
        private MainWindow.addDelegate addListBoxDelegate; // делегат для работы с ListBoxMusic
        

        public MainWindow()
        {
            InitializeComponent();
            TrackListClass.currentPath = "C:\\Users\\" + Environment.UserName + "\\Music"; //автоматический адрес при старте
            sliderVolume.Maximum = 1.0; //устанавливает максимальное значение слайдера прокрутки звука
            sliderVolume.Value = 0.5; //устанавливает значение при запуске
            media.Volume = this.sliderVolume.Value; //громкость звука аудиотреков

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
            new Thread(new ThreadStart(this.serverArgs))
            {
                IsBackground = true
            }.Start();
            playFunction(0);
        }

        //функция перетаскивания окна с помощью мыши
        private void customWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Щелчек по пункту ListView
        private void clickMouse(object sender, MouseButtonEventArgs e)
        {
            textOptionsListBox();
            trackNumber = ListBoxMusic.SelectedIndex;
            playFunction(this.ListBoxMusic.SelectedIndex);
        }

        //нажатие кнопки Закрыть
        private void Close_Buttn_Click(object sender, RoutedEventArgs e)
        {
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
            TagLib.File file = TagLib.File.Create(this.media.Source.LocalPath);
            infoTextBloc.Text = "Название песни: " + file.Tag.Title + "\nИсполнитель: " + string.Join(", ", file.Tag.Performers) + "\nВремя трека: " + file.Properties.Duration.ToString("mm\\:ss");
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
                HZButn.Content = (object)"ПАУЗА";
                lbi = (ListBoxItem)this.ListBoxMusic.ItemContainerGenerator.ContainerFromIndex(indexTrack);
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
        private void serverArgs()
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
                    Application.Current.Dispatcher.Invoke((Delegate)this.addListBoxDelegate);
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("FUCK " + ex.ToString());
            }
        }

        //метод запуска проигрывания видео
        private void startVideoDance()
        {
            videoPlayer.Source = new Uri(this.videoPathList[1]);
            videoPlayer.Position = TimeSpan.FromSeconds((double)new Random().Next(220));
        }

        //обработка событиц кнопки старт(нажатие на кнопку)
        private void StartButn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            trackNumber = this.ListBoxMusic.SelectedIndex;
            playFunction(this.ListBoxMusic.SelectedIndex);
        }

        #region методы работы с забрасывание музыки
        private void mainWindow_PreviewDragEnter(object sender, DragEventArgs e)
        {
            bool flag = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                foreach (string str in (string[])e.Data.GetData(DataFormats.FileDrop, true))
                {
                    if (!System.IO.File.Exists(str))
                    {
                        flag = false;
                        break;
                    }
                    FileInfo fileInfo = new FileInfo(str);
                    if (fileInfo.Extension != ".mp3" && fileInfo.Extension != ".MP3" && fileInfo.Extension != ".wma" && fileInfo.Extension != ".WMA")
                    {
                        flag = false;
                        break;
                    }
                }
            }
            e.Effects = !flag ? DragDropEffects.None : DragDropEffects.All;
            e.Handled = true;
        }

        private void mainWindow_PreviewDrop(object sender, DragEventArgs e)
        {
            foreach (string str in (string[])e.Data.GetData(DataFormats.FileDrop, true))
                TrackListClass.trackList.Add(str);
            this.addToLisBoxMusic();
        }
        #endregion

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

        //событие кнопки очистить
        private void clearButn_Click(object sender, RoutedEventArgs e)
        {
            this.media.Stop();
            this.mediaState = MediaState.Stop;
            this.HZButn.Content = (object)"ПАУЗА";
            TrackListClass.trackList.Clear();
            this.ListBoxMusic.Items.Clear();
            this.media.Source = (Uri)null;
            this.videoPlayer.Source = new Uri(this.videoPathList[0]);
            this.infoTextBloc.Text = "";
            //this.messageDonat(false);
        }

        //перехват нажатия кнопки основным 
        private void windowForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;
            try
            {
                if (this.media.Source != (Uri)null && this.media.Source.LocalPath == TrackListClass.trackList[this.ListBoxMusic.SelectedIndex])
                {
                    this.media.Stop();
                    this.mediaState = MediaState.Stop;
                    this.HZButn.Content = (object)"ПАУЗА";
                    this.videoPlayer.Source = new Uri(this.videoPathList[0]);
                    //this.messageDonat(false);
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
    }
}
