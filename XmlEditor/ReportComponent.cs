using Microsoft.Extensions.Logging;

namespace Console7_Xml
{
    internal class ReportComponent: ComponentModel
    {
        private XmlNode<ComponentModel> root;
        private ILogger logger = LoggerFactory.Create((builder) => { builder.AddConsole(); }).CreateLogger(nameof(Report));

        public ReportComponent(XmlNode<ComponentModel> root)
        {
            this.root = root;
            logger.LogInformation("CREATED");
        }
    }
}