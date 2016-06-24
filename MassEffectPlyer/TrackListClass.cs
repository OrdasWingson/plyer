using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MassEffectPlyer
{
    class TrackListClass
    {
        public static List<string> trackList = new List<string>(); //коллекция аддресов аудиотреков
        private static string[] foldersInside; //коллекция папок внутри каталога
        private static string[] mediaInsideMP3; //коллекция аудиофайлов внутри каталога
        private static string[] mediaInsideWMA; //коллекция аудиофайлов внутри каталога
        public static string[] generalInside; //общая коллекция для папок и аудиофайлов
        public static string currentPath { get; set; } //переменная пути текущего каталога

        //метод возвращающий общую коллекцию файлов и папок текущей директории
        public static string[] takeAllInside(string currDir)
        {
            foldersInside = Directory.GetDirectories(currDir);
            mediaInsideMP3 = Directory.GetFileSystemEntries(currDir, "*.mp3");
            mediaInsideWMA = Directory.GetFileSystemEntries(currDir, "*.wma");
            generalInside = new string[foldersInside.Length + mediaInsideMP3.Length + mediaInsideWMA.Length];

            for (int index = 0; index < foldersInside.Length; ++index)
                generalInside[index] = foldersInside[index];

            for (int index = 0; index < mediaInsideMP3.Length; ++index)
                generalInside[foldersInside.Length + index] = mediaInsideMP3[index];

            for (int index = 0; index < mediaInsideWMA.Length; ++index)
                generalInside[foldersInside.Length + mediaInsideMP3.Length + index] = mediaInsideWMA[index];

            return generalInside;
        }


        //метод добавления адресов аудиофайлов в TrackList
        public static void audioFromFolderToTrackList(string currDir)
        {
            mediaInsideMP3 = Directory.GetFileSystemEntries(currDir, "*.mp3");
            mediaInsideWMA = Directory.GetFileSystemEntries(currDir, "*.wma");

            foreach (string str in mediaInsideMP3)
                trackList.Add(str);

            foreach (string str in mediaInsideWMA)
                trackList.Add(str);
        }


        //медот миксования треков в плейлисте
        public static void mixerFunc()
        {
            if (trackList == null)
                return;
            Random random = new Random();
            for (int index1 = 0; index1 < trackList.Count; ++index1)
            {
                int index2 = random.Next(trackList.Count);
                string str = trackList[index1];
                trackList[index1] = trackList[index2];
                trackList[index2] = str;
            }
        }

        public static void mixerFunc<type>(List<type> arrList)
        {
            if (arrList == null)
                return;
            Random random = new Random();
            for (int index1 = 0; index1 < arrList.Count; ++index1)
            {
                int index2 = random.Next(trackList.Count);
                var str = arrList[index1];
                arrList[index1] = arrList[index2];
                arrList[index2] = str;
            }
        }

        
    }
}
