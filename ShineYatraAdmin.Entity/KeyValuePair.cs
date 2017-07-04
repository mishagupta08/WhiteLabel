using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShineYatraAdmin.Entity
{
    [XmlRoot(ElementName = "KeyValuePair")]
    public class KeyValuePair
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfKeyValuePair")]
    public class ArrayOfKeyValuePair
    {
        [XmlElement(ElementName = "KeyValuePair")]
        public List<KeyValuePair> KeyValuePair { get; set; }
    }

}
