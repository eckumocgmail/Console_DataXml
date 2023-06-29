using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class ComponentParams
{
    public string Tag { get; set; }
    [XmlIgnore]
    public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();
    public string Text { get; set; }


    public string BeginText()
    {
        string attrsstr = "";
        foreach (var attr in Attrs)
        {
            attrsstr += $" {attr.Key}='{attr.Value}'";
        }
        return $"<{Tag}{attrsstr}>";
    }


    public string EndText()
    {
        string attrsstr = "";
        foreach (var attr in Attrs)
        {
            attrsstr += $" {attr.Key}='{attr.Value}'";
        }
        return $"</{Tag}{attrsstr}>";
    }


    public string GetNameAttribute()
    {
        if (Attrs.ContainsKey("Name"))
        {
            return Attrs["Name"];
        }
        return null;
    }
}