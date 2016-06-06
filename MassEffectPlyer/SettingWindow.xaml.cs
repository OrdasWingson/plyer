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
    delegate void dragMethod();
    /// <summary>
    /// Логика взаимодействия для SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        double mainTop;
        double mainLeft;
       


        public SettingWindow(double x, double y)
        {
            InitializeComponent();
            mainTop = x;
            mainLeft = y;
            checkBoxButtSound.IsChecked = Sounds.soundOnOff;
            checkBoxSaveTrackList.IsChecked = MainWindow.saveTrack;

        }

        //расположение  окна
        private void settingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = mainTop+57;
            this.Left = mainLeft;
        }

        //передвижение окна
        private void Drag_SettingWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();           
        }

        //событие выхода из меню настроек
        private void okButn_Click(object sender, RoutedEventArgs e)
        {
            Sounds.clikSoundField();
            this.Close();
        }

        //изменение положения окна
        private void settingWindow_LocationChanged(object sender, EventArgs e)
        {

            this.Owner.Top = this.Top - 57;
            this.Owner.Left = this.Left;
            
        }

              
        //выбор включон ли звукжжж
        private void checkBoxButtSound_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxButtSound.IsChecked == true)
            {
                Sounds.setVolume(1.0);
                Sounds.soundOnOff = true;
            }
            else
            {
                Sounds.setVolume(0.0);
                Sounds.soundOnOff = false;
            }
        }

        //сохронять или нет треки после выключения
        private void checkBoxSaveTrackList_Click(object sender, RoutedEventArgs e)
        {
            if(checkBoxSaveTrackList.IsChecked == true)
            {
                MainWindow.saveTrack = true;
            }
            else
            {
                MainWindow.saveTrack = false;
            }
        }
    }
}
