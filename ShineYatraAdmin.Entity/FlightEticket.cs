using System.Xml.Serialization;


namespace ShineYatraAdmin.Entity
{
    [XmlRoot(ElementName = "Eticket")]
    public class Eticket
    {
        [XmlElement(ElementName = "givenName")]
        public string GivenName { get; set; }
        [XmlElement(ElementName = "surName")]
        public string SurName { get; set; }
        [XmlElement(ElementName = "nameReference")]
        public string NameReference { get; set; }
        [XmlElement(ElementName = "eticketno")]
        public string Eticketno { get; set; }
        [XmlElement(ElementName = "flightuid")]
        public string Flightuid { get; set; }
        [XmlElement(ElementName = "passuid")]
        public string Passuid { get; set; }
    }

    [XmlRoot(ElementName = "eticketdto")]
    public class Eticketdto
    {
        [XmlElement(ElementName = "Eticket")]
        public Eticket Eticket { get; set; }
    }

    [XmlRoot(ElementName = "OriDestPNRRequest")]
    public class OriDestPnrRequest
    {
        [XmlElement(ElementName = "flightno")]
        public string Flightno { get; set; }
        [XmlElement(ElementName = "eticketdto")]
        public Eticketdto Eticketdto { get; set; }
        [XmlElement(ElementName = "confirmationid")]
        public string Confirmationid { get; set; }
        [XmlElement(ElementName = "pnrnumber")]
        public string Pnrnumber { get; set; }
    }

    [XmlRoot(ElementName = "origindestinationoptions")]
    public class Origindestinationoptions
    {
        [XmlElement(ElementName = "OriDestPNRRequest")]
        public OriDestPnrRequest OriDestPnrRequest { get; set; }
    }

    [XmlRoot(ElementName = "requestedPNR")]
    public class RequestedPnr
    {
        [XmlElement(ElementName = "origindestinationoptions")]
        public Origindestinationoptions Origindestinationoptions { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "partnerRefId")]
        public string PartnerRefId { get; set; }
    }

    [XmlRoot(ElementName = "EticketDetails")]
    public class EticketDetails
    {
        [XmlElement(ElementName = "requestedPNR")]
        public RequestedPnr RequestedPnr { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }

}
