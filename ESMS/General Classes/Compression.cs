using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace ESMS.General_Classes
{
    public class Compression
    {
        public static void Compres(string path, IFormFile file, FType fType)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

            using (var ms = new MemoryStream())
            {
                using (var archive =
                    new System.IO.Compression.ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    file.OpenReadStream().CopyTo(memoryStream);
                    var strearFile = file.OpenReadStream();
                    byte[] fileByte = memoryStream.ToArray();

                    var zipEntry = archive.CreateEntry(file.FileName,
                        CompressionLevel.Optimal);
                    using (var zipStream = zipEntry.Open())
                    {
                        zipStream.Write(fileByte, 0, fileByte.Length);
                        zipStream.Close();
                        fs.Write(ms.ToArray());
                        fs.Close();
                    }

                    //var zipEntry2 = archive.CreateEntry("image2.png",
                    //    CompressionLevel.Fastest);
                    //using (var zipStream = zipEntry2.Open())
                    //{
                    //    zipStream.Write(bytes2, 0, bytes2.Length);
                    //}
                }
            }
        }

        public static byte[] Decompres(string path)
        {
            return null;
        }


        public enum FType
        {
            ContractFile = 1,
            GeneralFile = 2
        }

    }
}
