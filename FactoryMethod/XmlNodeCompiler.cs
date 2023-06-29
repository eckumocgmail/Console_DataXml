

using Console_DataXml.DataModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
 

public class XmlNodeCompiler 
{
    public static ReportComponent CompileXml(string reportXml)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(reportXml);

        // получим корневой элемент        
        XmlNode pnode = xDoc.DocumentElement;
        Console.WriteLine(pnode.OuterXml);
        var root = new XmlNodeCompiler().Compile(pnode, null, true);
        var report = new ReportComponent(root);
        report.OnInit();
        return report;
    }

    private readonly IComponentFactory _componentFactory;
    private DataReport root;

    public XmlNodeCompiler():this(new ComponentFactory())
    {
    }

    public XmlNodeCompiler( IComponentFactory componentFactory ) {

        _componentFactory = componentFactory;
    }


    public XmlNode<ComponentModel> Compile(string xml)
    {
        using (var reader = new StringReader(xml))
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reader);
    

            // получим корневой элемент
            XmlNode pnode = xDoc.DocumentElement;
            return Compile(pnode, null, true);
        }
    }


    public XmlNode<ComponentModel> Compile(XmlNode pnode, XmlNode<ComponentModel> parent, bool isRoot)
    {
        if (isRoot == false)
        {
            if (pnode == null)
            {
                throw new ArgumentException("pnode");
            }
            if (parent == null)
            {
                throw new ArgumentException("parent");
            }
        }
        XmlNode<ComponentModel> compiled = null;
        if (pnode != null)
        {
            if (isRoot)
            {
                root = null;
            }
            ComponentParams inputParams = ParseCompileParams(pnode);
            ComponentModel parentComponent = parent != null ? parent.Item: null;
            object targetComponent = _componentFactory.Create(inputParams, parentComponent, root);
            ComponentModel CurrentComponent =   new ComponentModel() { 
                Parameters = inputParams,
                
            };
            CurrentComponent.SetTarget(targetComponent);
            compiled = new XmlNode<ComponentModel>(pnode.GetHashCode() + "", CurrentComponent, parent);
            if (isRoot)
            {
                root = (DataReport)CurrentComponent.GetTarget();
            }
            for (int i = 0; i < pnode.ChildNodes.Count; i++)
            {
                XmlNode node = pnode.ChildNodes.Item(i);
                //compiled.Append();
                Compile(node, compiled, false);
            }
        }

        return compiled;
    }



    private ComponentParams ParseCompileParams(XmlNode pnode)
    {

        string tag = pnode.Name;
        string log = pnode.Name;
        var attrs = new Dictionary<string, string>();
        if (pnode.Attributes != null)
        {
            for (int i = 0; i < pnode.Attributes.Count; i++)
            {
                attrs[ToCapitalStyle(pnode.Attributes.Item(i).Name)] = pnode.Attributes.Item(i).Value;
                log += " " + pnode.Attributes.Item(i).Name + "=" + pnode.Attributes.Item(i).Value;
            }
        }
        string text = "";
        if (pnode.ChildNodes.Count == 0)
        {
            text = pnode.InnerText;
        }
        return new ComponentParams()
        {
            Tag = tag,
            Attrs = attrs,
            Text = text
        };
    }

    private string ToCapitalStyle(string name) => name.ToCapitalStyle();
}
