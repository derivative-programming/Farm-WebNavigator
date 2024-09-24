using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace FS.Common.Serialize
{
    public class Functions
    {

        public static byte[] SerializeHashTable(System.Collections.Hashtable value)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, value);
                    return memoryStream.ToArray();
                }
            }
        public static System.Collections.Hashtable DeSerializeHashTable(byte [] value)
            { 
            
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(value, 0, value.Length);
                BinaryFormatter bf = new BinaryFormatter();
                memoryStream.Position = 0;
                return (System.Collections.Hashtable)bf.Deserialize(memoryStream);
            }


        public static void SerializeBinary(object data, string file)
        {
            System.IO.Stream stream = System.IO.File.Open(file, System.IO.FileMode.Create);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bformatter.Serialize(stream, data);
            stream.Close();
        } 
        public static T DeSerializeBinary<T>(string file)
        {
            System.IO.Stream stream = System.IO.File.Open(file, System.IO.FileMode.Open);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            T data = (T)bformatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        public static T DeSerializeFromString<T>(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (System.IO.StringReader reader = new System.IO.StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        public static string SerializeToString(object obj)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());

            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }
    }
}
