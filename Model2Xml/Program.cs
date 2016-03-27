using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model2Xml
{
    public class Program
    {
        public static Version Version { get; private set; }

        static void Main(string[] args)
        {
            Version = new Version(1, 0);
            Model2Xml(@"D:\Bureau\data_bug.model", @"D:\Bureau\data_bug.xml");
        }

        public static void Model2Xml(string modelPath, string xmlPath)
        {
            using (FileStream fileStream = File.OpenRead(modelPath))
            using (GZipStream gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                byte[] buffer = new byte[4];
                gzStream.Read(buffer, 0, 4);

                using (DeflateStream deflateStream = new DeflateStream(gzStream, CompressionMode.Decompress))
                using (StreamReader streamReader = new StreamReader(deflateStream))
                    File.WriteAllText(xmlPath, streamReader.ReadToEnd());
            }
        }
        public static void Xml2Model(string xmlPath, string modelPath)
        {
            using (FileStream fileStream = File.Create(modelPath))
            using (GZipStream gzStream = new GZipStream(fileStream, CompressionMode.Compress))
            {
                gzStream.WriteByte((byte)'M');
                gzStream.WriteByte((byte)'S');
                gzStream.WriteByte((byte)Version.Major);
                gzStream.WriteByte((byte)Version.Minor);

                using (DeflateStream deflateStream = new DeflateStream(gzStream, CompressionMode.Compress))
                using (StreamWriter streamWriter = new StreamWriter(deflateStream))
                    streamWriter.Write(File.ReadAllText(xmlPath));
            }
        }
    }
}
