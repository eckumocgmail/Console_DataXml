using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using XmlCompiler;
using XmlCompiler.XmlCompiler.Parser;

namespace Console7_Xml
{
    using static System.Console;

    public class XmlDatabase 
    {
        private IDictionary<string, string> xmls = new ConcurrentDictionary<string, string>();
       

        //@"C:\ftp\NetProjects\Mbd_Feature_Excel\Console7_Xml\DataStore\data"
        public XmlDatabase( string dir )
        {
            WriteLine( dir );
            foreach(string file in System.IO.Directory.GetFiles(dir))
            {
                var parser = new MyXmlParser();
                try
                {
                    WriteLine(file);
                    xmls[file] = System.IO.File.ReadAllText(file);
                    
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(file);

                    // получим корневой элемент

                    XmlNode pnode = xDoc.DocumentElement;
                    Console.WriteLine(pnode.OuterXml);
                    var root = new XmlNodeCompiler().Compile(pnode, null, true);
                    var report = new ReportComponent(root);
                    report.OnInit();
 
                   
                }catch (Exception ex)
                {
                    Console.WriteLine(file+ex);
                }

            }
        }
    }
}
