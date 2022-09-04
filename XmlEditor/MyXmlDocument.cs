using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppXmlEditorConsole.XmlModel
{
    public class MyXmlDocument: MyXmlContainer
    {

        /// <summary>
        /// Возвращает текст формата XML
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            return GetXml(0);
        }
    }
}
