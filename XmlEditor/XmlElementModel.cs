using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompiler.XmlCompiler.Parser
{
    public class XmlElementModel: Component
    {
        public virtual string Tag { get; set; }
        public string ContainerText { get; set; }
        public string InnerText { get; set; }


        public List<XmlElementModel> ParseChildren()
        {
            List<XmlElementModel> children = new List<XmlElementModel>();
            string xml = InnerText;
            int startIndex = xml.IndexOf("<");

            //начинающийся текст
            if (startIndex > 0)
            {
                var xmlTextNode = new XmlElementModel()
                {
                    Tag = "",
                    ContainerText = "",
                    InnerText = xml.Substring(0, startIndex)
                };
                children.Add(xmlTextNode);
                xml = xml.Substring(startIndex);
            }

            //значит есть внутренний элеемент
            if (startIndex != -1)
            {

            }


            //завершающий текст
            if (xml.Length > 0)
            {
                var xmlTextNode = new XmlElementModel()
                {
                    Tag = "",
                    ContainerText = "",
                    InnerText = xml
                };
                children.Add(xmlTextNode);
                xml = "";
            }
            return children;
        }
    }

}
