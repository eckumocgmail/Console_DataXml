 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class DataServiceModule: IServiceModule
    {
        public void ConfigureServices(IDictionary<string, string> configuration, IServiceCollection services)
        {
            string DefaultConnectionString = configuration["DefaultConnectionString"].ToString();
            services.AddSingleton<DataContext>();
            //services.AddSingleton<DataContext>(services,options => options.UseSqlServer(DefaultConnectionString));            
        }
    }
}
