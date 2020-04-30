using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace CEngine
{
    public static class CommonTool
    {
        public static string CalFileMD5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);

                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
