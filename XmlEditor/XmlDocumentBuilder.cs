using AppXmlEditorConsole.XmlModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppXmlEditorConsole.XmlEditor
{
    public class XmlDocumentBuilder : XmlDeclarationEditor
    {
        private MyXmlDocument XmlDocument { get; set; } = new MyXmlDocument();

       

        public void OpenXmlDocument(string filename)
        {
            throw new NotImplementedException();
        }

        public void SaveXmlDocument(string filename)
        {
            System.IO.File.WriteAllText(filename, XmlDocument.GetXml());
        }



        public string GetXml()
        {
            return XmlDocument.GetXml();
        }

    }
}
