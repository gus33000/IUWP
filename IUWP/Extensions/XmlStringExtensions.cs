using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace IUWP
{
    public static class XmlStringExtensions
    {
        public static string XmlSerializeToString(this object objectInstance)
        {
            XmlSerializer serializer = new(objectInstance.GetType());
            StringBuilder sb = new();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            XmlSerializer serializer = new(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
