using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows;

namespace MassEffectPlyer
{
    class MemoryXML
    {
        XmlDocument xDoc = new XmlDocument();
        string pathXML = AppDomain.CurrentDomain.BaseDirectory + "data\\TracksStartLoading.xml";
       

        public MemoryXML()
        {
            xDoc.Load(pathXML);
            
        }

        //функция выводящая сообщения о содержимом xml
        public void ShowTracks()
        {
            XmlElement xRootElem = xDoc.DocumentElement;

            foreach(XmlNode xNode in xRootElem.ChildNodes)
            {
                MessageBox.Show(xNode.InnerText);
            }
            
        }

        //возвращает треки
        public List<string> GetTracksFromXML()
        {
            List<string> tracks = new List<string>();
            XmlElement xRoot = xDoc.DocumentElement;

            foreach(XmlNode xNode in xRoot.ChildNodes)
            {
                tracks.Add(xNode.InnerText);
            }
            return tracks;
        }


        //сохраняет треки
        public void SaveTracksToXML(List<string> tracks)
        {           
            XmlNode node = xDoc.DocumentElement;
            node.RemoveAll();

            foreach (var track in tracks)
            {

                XmlNode xmlSave = xDoc.CreateElement("track");
                xmlSave.InnerText = track;
                node.AppendChild(xmlSave);

            }
            
            xDoc.Save(pathXML);
        }
    }
}
