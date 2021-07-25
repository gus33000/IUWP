using System.Collections.Generic;
using System.Xml.Serialization;

namespace IUWP
{
    public class UpdateHistoryClass
    {
        [XmlRoot(ElementName = "Version", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class Version
        {
            [XmlAttribute(AttributeName = "Major")]
            public string Major { get; set; }
            [XmlAttribute(AttributeName = "Minor")]
            public string Minor { get; set; }
            [XmlAttribute(AttributeName = "QFE")]
            public string QFE { get; set; }
            [XmlAttribute(AttributeName = "Build")]
            public string Build { get; set; }
        }

        [XmlRoot(ElementName = "Identity", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class Identity
        {
            [XmlElement(ElementName = "Owner", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Owner { get; set; }
            [XmlElement(ElementName = "Component", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Component { get; set; }
            [XmlElement(ElementName = "SubComponent", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string SubComponent { get; set; }
            [XmlElement(ElementName = "Version", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public Version Version { get; set; }
        }

        [XmlRoot(ElementName = "Package", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class Package
        {
            [XmlElement(ElementName = "Identity", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public Identity Identity { get; set; }
            [XmlElement(ElementName = "ReleaseType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string ReleaseType { get; set; }
            [XmlElement(ElementName = "OwnerType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string OwnerType { get; set; }
            [XmlElement(ElementName = "BuildType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string BuildType { get; set; }
            [XmlElement(ElementName = "CpuType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string CpuType { get; set; }
            [XmlElement(ElementName = "Partition", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Partition { get; set; }
            [XmlElement(ElementName = "GroupingKey", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string GroupingKey { get; set; }
            [XmlElement(ElementName = "IsRemoval", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string IsRemoval { get; set; }
            [XmlElement(ElementName = "IsBinaryPartition", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string IsBinaryPartition { get; set; }
            [XmlElement(ElementName = "PackageFile", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string PackageFile { get; set; }
            [XmlElement(ElementName = "PackageType", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string PackageType { get; set; }
            [XmlElement(ElementName = "Culture", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Culture { get; set; }
            [XmlElement(ElementName = "Resolution", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Resolution { get; set; }
            [XmlElement(ElementName = "Platform", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Platform { get; set; }
            [XmlElement(ElementName = "PackageIdentity", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string PackageIdentity { get; set; }
        }

        [XmlRoot(ElementName = "Packages", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class Packages
        {
            [XmlElement(ElementName = "Package", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public List<Package> Package { get; set; }
        }

        [XmlRoot(ElementName = "UpdateOSOutput", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class UpdateOSOutput
        {
            [XmlElement(ElementName = "Description", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Description { get; set; }
            [XmlElement(ElementName = "OverallResult", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string OverallResult { get; set; }
            [XmlElement(ElementName = "UpdateState", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string UpdateState { get; set; }
            [XmlElement(ElementName = "Packages", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public Packages Packages { get; set; }
        }

        [XmlRoot(ElementName = "UpdateEvent", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class UpdateEvent
        {
            [XmlElement(ElementName = "Sequence", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string Sequence { get; set; }
            [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public string DateTime { get; set; }
            [XmlElement(ElementName = "UpdateOSOutput", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public UpdateOSOutput UpdateOSOutput { get; set; }
        }

        [XmlRoot(ElementName = "UpdateEvents", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class UpdateEvents
        {
            [XmlElement(ElementName = "UpdateEvent", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public List<UpdateEvent> UpdateEvent { get; set; }
        }

        [XmlRoot(ElementName = "UpdateHistory", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
        public class UpdateHistory
        {
            [XmlElement(ElementName = "UpdateEvents", Namespace = "http://schemas.microsoft.com/embedded/2004/10/ImageUpdate")]
            public UpdateEvents UpdateEvents { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }
    }
}
