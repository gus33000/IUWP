using System.Xml.Serialization;

namespace IUWP
{
    [XmlRoot(ElementName = "CompressedPartitions", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class CompressedPartitions
    {
        [XmlElement(ElementName = "Name", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "OEMDevicePlatform", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class OEMDevicePlatform
    {
        [XmlElement(ElementName = "MinSectorCount", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string MinSectorCount { get; set; }
        [XmlElement(ElementName = "AdditionalMainOSFreeSectorsRequest", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string AdditionalMainOSFreeSectorsRequest { get; set; }
        [XmlElement(ElementName = "DevicePlatformID", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string DevicePlatformID { get; set; }
        [XmlElement(ElementName = "MainOSRTCDataReservedSectors", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string MainOSRTCDataReservedSectors { get; set; }
        [XmlElement(ElementName = "CompressedPartitions", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public CompressedPartitions CompressedPartitions { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
