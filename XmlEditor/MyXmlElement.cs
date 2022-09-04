using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppXmlEditorConsole.XmlModel
{
    class MyXmlElement: MyXmlDeclaration
    {
        public string Text { get; set; }


        /// <summary>
        /// Возращет текст вхождения в контейнер
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected override string GetOpenXml(int level) {
            string text = base.GetOpenXml(level);
            return text.Substring(0, text.Length - 1);
        }

      


        /// <summary>
        /// Возвращает текст внутреннего элемента
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected override string GetInnerXml(int level)
        {
            return Text;
        }
    }
}
