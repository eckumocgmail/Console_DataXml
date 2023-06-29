using Microsoft.Extensions.Logging;

namespace Console_DataXml.DataModel
{
    public class ReportComponent : ComponentModel
    {
        private XmlNode<ComponentModel> root;
        private ILogger logger = LoggerFactory.Create((builder) => { builder.AddConsole(); }).CreateLogger(nameof(DataReport));

        public ReportComponent(XmlNode<ComponentModel> root)
        {
            this.root = root;
            logger.LogInformation("CREATED");
        }

        public DataSources DataSources { get; internal set; }
        public DataSets DataSets { get; internal set; }
    }
}