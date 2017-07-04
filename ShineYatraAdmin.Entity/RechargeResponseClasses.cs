using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ShineYatraAdmin.Entity
{
    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "service_type")]
        public string Service_type { get; set; }
        [XmlElement(ElementName = "service_provider")]
        public string Service_provider { get; set; }
        [XmlElement(ElementName = "service_desc")]
        public string Service_desc { get; set; }
        [XmlElement(ElementName = "provider_key")]
        public string Provider_key { get; set; }
        //[XmlElement(ElementName = "margin")]
        //public string Margin { get; set; }
    }

    [XmlRoot(ElementName = "xml")]
    public class Xml
    {
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }

}