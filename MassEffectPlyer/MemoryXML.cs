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


        public void ShowTracks()
        {
            XmlElement xRootElem = xDoc.DocumentElement;

            foreach(XmlNode xNode in xRootElem.ChildNodes)
            {
                MessageBox.Show(xNode.InnerText);
            }
            //foreach(XmlNode xNode in xRootElem)
            //{

            //}
        }
    }
}
