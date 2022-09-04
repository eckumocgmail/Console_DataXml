using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XmlCompiler.XmlCompiler.Parser;

namespace AppXmlEditorConsole.XmlModel
{


    /// <summary>
    /// Декларативная модель элементарной единицы XML-докумнета
    /// </summary>
    public abstract class MyXmlDeclaration: XmlElementModel
    {
        public override string Tag { get; set; } = "div";
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();



        /// <summary>
        /// Возвращает текст формата XML внутреннего обьекта
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected abstract string GetInnerXml(int level);


        /// <summary>
        /// Возвращает текст формата XML
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual string GetXml(int level)
        {
            return GetOpenXml(level) + GetInnerXml(level+1) + GetCloseXml(level);
        }


        /// <summary>
        /// Возращет текст вхождения в контейнер
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual string GetOpenXml(int level)
        {
            if (Attributes.Count() == 0)
            {
                return $"{GetSpace(level)}<{Tag}>";
            }
            else
            {
                string AttributesText = "";
                foreach (var attr in Attributes)
                {
                    AttributesText += $"{attr.Key}=\"{attr.Value}\" ";
                }

                return $"<{Tag} {AttributesText.TrimEnd()}>\n";
            }
        }


        /// <summary>
        /// Возвращает текст отступа на заданном уровне
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual string GetSpace(int level)
        {
            string s = "";
            for(int i=0; i<level; i++)
            {
                s += "    ";
            }
            return s;
        }


        /// <summary>
        /// Возращет текст выхода из контейнера
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual string GetCloseXml(int level)
        {
            return $"</{Tag}>\n";
        }

        
    }
}
