using AppXmlEditorConsole.XmlModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppXmlEditorConsole.XmlEditor
{
    public class XmlDeclarationEditor
    {

        /// <summary>
        /// Выделенный элемент
        /// </summary>
        private MyXmlDeclaration Cursor { get; set; }


        public void SetAttribute(string key, string value)
        {

        }

        public void Append(string key, string value)
        {

        }

        public void SelectElement(MyXmlDeclaration declaration)
        {
            Cursor = declaration;
        }
    }
}
