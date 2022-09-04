using Console7_Xml;

using Newtonsoft.Json;

using System;
using System.Xml;

public class XmlCompileProgram
{
    private static string MetaInf = $@"<?xml version=""1.0"" encoding=""utf-16""?>";
    public static void Main( ) => Test( );
    public static void Test()
    {
        
        canCompileWsdl();
        //canCompileXml();
    }

    private static void canCompileWsdl()
    {
        try
        {
            string reportXml = $@"{System.IO.Directory.GetCurrentDirectory()}\XmlResources\app.wsdl";
            Console.WriteLine(System.Convert.ToDateTime("01.01.2020"));
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reportXml);

            // получим корневой элемент        
            XmlNode pnode = xDoc.DocumentElement;
            Console.WriteLine(pnode.OuterXml);
            var root = new XmlNodeCompiler().Compile(pnode, null, true);
            var report = new ReportComponent(root);
            report.OnInit();
            Console.WriteLine(JsonConvert.SerializeObject(root));


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public static void canCompileXml()
    {
        try
        {
            string reportXml = $@"{System.IO.Directory.GetCurrentDirectory()}\XmlResources\catalog.xml";
            Console.WriteLine(System.Convert.ToDateTime("01.01.2020"));
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reportXml);

            // получим корневой элемент        
            XmlNode pnode = xDoc.DocumentElement;
            Console.WriteLine(pnode.OuterXml);
            var root = new XmlNodeCompiler().Compile(pnode, null, true);
            var report = new ReportComponent(root);
            report.OnInit();
            Console.WriteLine(JsonConvert.SerializeObject(root));


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }
}