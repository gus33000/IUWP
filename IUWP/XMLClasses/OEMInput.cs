using System.Collections.Generic;
using System.Xml.Serialization;

namespace IUWP
{
    [XmlRoot(ElementName = "UserInterface", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class UserInterface
    {
        [XmlElement(ElementName = "Language", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> Language { get; set; }
    }

    [XmlRoot(ElementName = "Keyboard", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class Keyboard
    {
        [XmlElement(ElementName = "Language", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> Language { get; set; }
    }

    [XmlRoot(ElementName = "Speech", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class Speech
    {
        [XmlElement(ElementName = "Language", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> Language { get; set; }
    }

    [XmlRoot(ElementName = "SupportedLanguages", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class SupportedLanguages
    {
        [XmlElement(ElementName = "UserInterface", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public UserInterface UserInterface { get; set; }
        [XmlElement(ElementName = "Keyboard", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public Keyboard Keyboard { get; set; }
        [XmlElement(ElementName = "Speech", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public Speech Speech { get; set; }
    }

    [XmlRoot(ElementName = "Resolutions", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class Resolutions
    {
        [XmlElement(ElementName = "Resolution", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> Resolution { get; set; }
    }

    [XmlRoot(ElementName = "Microsoft", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class Microsoft
    {
        [XmlElement(ElementName = "Feature", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> Feature { get; set; }
    }

    [XmlRoot(ElementName = "OEM", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class OEM
    {
        [XmlElement(ElementName = "Feature", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> Feature { get; set; }
    }

    [XmlRoot(ElementName = "Features", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class Features
    {
        [XmlElement(ElementName = "Microsoft", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public Microsoft Microsoft { get; set; }
        [XmlElement(ElementName = "OEM", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public OEM OEM { get; set; }
    }

    [XmlRoot(ElementName = "AdditionalFMs", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class AdditionalFMs
    {
        [XmlElement(ElementName = "AdditionalFM", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public List<string> AdditionalFM { get; set; }
    }

    [XmlRoot(ElementName = "OEMInput", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
    public class OEMInput
    {
        [XmlElement(ElementName = "Description", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string Description { get; set; }
        [XmlElement(ElementName = "SV", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string SV { get; set; }
        [XmlElement(ElementName = "SOC", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string SOC { get; set; }
        [XmlElement(ElementName = "Device", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string Device { get; set; }
        [XmlElement(ElementName = "ReleaseType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string ReleaseType { get; set; }
        [XmlElement(ElementName = "BuildType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string BuildType { get; set; }
        [XmlElement(ElementName = "SupportedLanguages", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public SupportedLanguages SupportedLanguages { get; set; }
        [XmlElement(ElementName = "BootUILanguage", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string BootUILanguage { get; set; }
        [XmlElement(ElementName = "BootLocale", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public string BootLocale { get; set; }
        [XmlElement(ElementName = "Resolutions", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public Resolutions Resolutions { get; set; }
        [XmlElement(ElementName = "Features", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public Features Features { get; set; }
        [XmlElement(ElementName = "AdditionalFMs", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public AdditionalFMs AdditionalFMs { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
