using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrsReader
{
    public abstract class SuperType: 
            Dictionary<string, IDictionary<string, string>>,
            IDictionary<string, IDictionary<string, string>>
    {
        public abstract IDictionary<string, string> GetTypeAttributes();
        public abstract IDictionary<string, string> GetPropertiesAttributes();
        public abstract IDictionary<string, string> GetMethodAttributes();
    }
}
