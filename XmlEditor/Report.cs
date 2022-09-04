
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

public class Report 
{
    public List<object> DataSources { get; set; }  
    public List<object> DataSets { get; set; }
    public List<object> DataRelations { get; set; }
    public List<object> Parameters { get; set; }
    public List<object> Setup { get; set; }
    public List<object> Body { get; set; }



    private ConcurrentDictionary<string, object> Scope = new ConcurrentDictionary<string, object>();


    public void AddGlobalDeclaration( string key, object value)
    {
        Scope[key] = value;
    }

    public object Select( string query )
    {
        if (Scope.ContainsKey(query))
        {
            return Scope[query];
        }
        else
        {
            return null;
        }
        
    }

    public void BindInterpolate(object target, string attrName, string attrValue)
    {
        //TODO: 
        /*if(target is ViewItem)
        {

            ((ViewItem)target).InterpolationBindings[attrName] = attrValue;
            
        }
        else
        {
            throw new Exception("Выполнить интерполяцию может только компонент представления");
        }*/

    }


    private ILogger logger = LoggerFactory.Create((builder) => { builder.AddConsole(); }).CreateLogger(nameof(Report));
    public Report()
    {
        logger.LogInformation("CREATED");
    }
}