using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppXmlEditorConsole.XmlModel
{

    /// <summary>
    /// Обьединяет множество элементов
    /// </summary>
    public class MyXmlContainer: MyXmlDeclaration
    {
        private List<MyXmlDeclaration> ChildNodes { get; set; } = new List<MyXmlDeclaration>();


        

        /// <summary>
        /// Добавление внутренного элемента
        /// </summary>
        /// <param name="pchild"></param>
        public void Append(MyXmlDeclaration pchild)
        {
            ChildNodes.Add(pchild);
        }


        /// <summary>
        /// Удаление внутреннего элемента
        /// </summary>
        /// <param name="pchild"></param>
        public void Remove(MyXmlDeclaration pchild)
        {
            ChildNodes.Remove(pchild);
        }


        /// <summary>
        /// Возвращает текст формата XML для внутренних элементов
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected override string GetInnerXml(int level)
        {
            string text = "";
            ChildNodes.ForEach(pchild=> {
                text += pchild.GetXml(level);
            });
            return text;
        }
    }
}
