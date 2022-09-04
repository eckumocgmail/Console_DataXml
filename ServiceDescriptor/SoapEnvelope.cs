using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml.ServiceDescriptor
{
    public class SoapEnvelope: Report
    {
        public SoapHeader SoapHeader { get; set; }
        public SoapBody SoapBody { get; set; }

    }

    public class SoapBody
    {
        public MGetprice MGetprice { get; set; }
        public SoapFault SoapFault { get; set; }
    }
    
    public class SoapFault
    {
        public MTrans MTrans { get; set; }
    }
    public class SoapHeader
    {
        public MTrans MTrans { get; set; }
    }
    public class MGetprice
    {
        public MItem MItem { get; set; }
        public HyperText text { get; set; }
    }
    public class MTrans
    {
        public HyperText text { get; set; }
    }
    public class MItem
    {
        public HyperText text { get; set; }

    }
  
}
