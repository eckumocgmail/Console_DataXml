using AppXmlEditorConsole.XmlModel;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XmlCompiler.XmlCompiler.Parser
{

    using static System.Console;

    public class MyXmlParser: MyXmlContainer
    {

        public static void Parse(string xml)
        {

            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(System.IO.File.ReadAllText(@"D:\System-Config\DataStore\xml\catalog.xml"));

            var pnode = document.DocumentElement;

            ForEach(pnode, (element) => {
                Console.WriteLine(JsonConvert.SerializeObject(element));
                return true;
            });
        }

        internal static void Test()
        {
            Parse($@"<?xml version=""1.0"" encoding=""utf-16""?>
            <report>
	            <data-sources>
		            <odbc-data-source connection-string=""Driver="+ "{"+$@"SQL Server"+"}"+$@";Server=DESKTOP-I4N78PV\SQLEXPRESS;Database=Catalog;Trusted_Connection=True;MultipleActiveResultSets=true""
						              alias=""Catalog"">			
		            </odbc-data-source>
	            </data-sources>
	            <data-sets>
		            <data-table data-source=""Catalog"" table-name=""Categories"">
			            <data-column-collection name=""Columns"">
				            <data-column column-name=""CategoryID"" data-type=""int"" primary-key=""true""></data-column>
				            <data-column column-name=""CategoryName"" data-type=""float""></data-column>

			            </data-column-collection>
		            </data-table>
		
		            <data-table data-source=""Catalog"" table-name=""Products"">
			            <data-column-collection name=""Columns"">
				            <data-column column-name=""UnitPrice"" data-type=""float""></data-column>
				            <data-column column-name=""ProductName"" data-type=""nvarchar""></data-column>
				            <data-column column-name=""ProductID"" data-type=""int"" primary-key=""true""></data-column>
				            <data-column column-name=""CategoryID"" data-type=""int"" foreign-key=""Categories""></data-column>
			            </data-column-collection>
		            </data-table>
	            </data-sets>
	            <body>
		            <data-list title=""Анализ продаж"" dataset=""Categories"" bind="""+"{"+@$" Title: 'CategoryName', Href: '/DatabaseEditor/Categories/Edit/{{ID}}' "+"}"+$@""">			
		            </data-list>
	            </body>
            </report>");
        }

        private static void ForEach(XmlElement pnode, Predicate<MyXmlElement> WriteLine)
        {
            WriteLine(new MyXmlElement
            {
                Tag = pnode.Value,
                Attributes = ToDictionary(pnode.Attributes)
            });
        }

        private static Dictionary<string, string> ToDictionary(XmlAttributeCollection attributes)
        {
            var result = new Dictionary<string, string>();
            foreach(XmlAttribute attr in attributes)
            {
                result[attr.Name] = attr.Value;
            }
            return result;
        }

        public XmlElementModel Parse(XmlElementModel model, string xml)
        {
            var pnode = new XmlElementModel();
            
            int openIndex = xml.IndexOf("<");
            //return model;


            string elementText = "";
            string innerText = "";
            bool intag = false;
            for (int i = 0; i < xml.Length; i++)
            {
                if(intag == false)
                {
                    if (xml[i] == '<')
                    {
                        intag = true;
                    }
                    else
                    {

                    }

                }
                else
                {
                    if( xml[i] == '>')
                    {
                        intag = false;
                        elementText = "";
                    }
                    else
                    {
                        elementText += xml[i];
                    }
                }
            }

            return pnode;
        }
    }
}
