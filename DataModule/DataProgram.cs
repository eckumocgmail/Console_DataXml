using AttrsReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using COM;

using StatReports.BusinessResources.DataADO.ADODataSource;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; 

namespace COM
{

}
namespace DataModule
{


    public class DataProgram: SuperType
    {
        public static string URL { get; set; } = System.IO.Directory.GetCurrentDirectory();
        public object ActionsMetaData { get; set; }

        
        public Dictionary<string, string> TypeMetaData { get; set; }
        private Dictionary<string, Dictionary<string, string>> PropertiesMetaData { get; set; }
        

        public override IDictionary<string, string> GetTypeAttributes()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, string> GetPropertiesAttributes()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, string> GetMethodAttributes()
        {
            throw new NotImplementedException();
        }




        static void Startup(string[] args)
        {
            Run(args);
            var odbc = new OdbcDriverManager();
            odbc.GetOdbcDrivers();
            using(var sql= new SqlServerADODataSource())
            {
                sql.EnsureIsValide();                
            }
        
        }


        
        static void Run(string[] args)
        {
            System.Console.WriteLine($"Вызов: {Assembly.GetCallingAssembly().GetName().Name }");
            System.Console.WriteLine($"Впроцессе: {Assembly.GetExecutingAssembly().GetName().Name }");

            foreach (var type in Assembly.GetExecutingAssembly().GetDataContexts())
            {
                var forProperties = Utils.ForAllPropertiesInType(type);
                var forMethods = Utils.ForAllMethodsInType(type);
                var forType = Utils.ForType(type);

                var superType = new DataProgram();
                superType.TypeMetaData = forType;
                superType.PropertiesMetaData = forProperties;
                superType.ActionsMetaData = forMethods;                
            }
        }

        
    }




}
