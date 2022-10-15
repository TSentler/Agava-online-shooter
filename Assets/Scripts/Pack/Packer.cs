using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pack
{
    public class Packer
    {
        public static object ByteArrayToObject(byte[] bytes)
        {
            if(bytes.Length == 0)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return bf.Deserialize(ms);
            }
        }
        
        public static byte[] ObjectToByteArray(object obj)
        {
            if(obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
