using System;
using System.IO;
using System.Text;
using static System.String;
using System.Collections.Generic;

namespace Utilities
{
    public class LogHelper
    {
        private static readonly string DefaultFilePath = "D:/UserLog/";
        private static readonly Dictionary<string, string> HashFiles = new Dictionary<string, string>();

        public static bool UserLog(string content, string title = null, string fileName = null, string filePath = null)
        {
            var defaultFfileName = DateTime.Today.ToString("yyyyMMdd") + ".txt";
            var name = fileName ?? defaultFfileName;
            if (!HashFiles.ContainsKey(name))
                HashFiles.Add(name, name.ToMd5());

            var str = new StringBuilder();
            str.AppendLine("-----------------------------------------------------------------------");
            str.AppendLine("时间:" + DateTime.Now.ToString("O"));
            if (!IsNullOrEmpty(title))
            {
                str.AppendLine("标题:" + title);
            }
            str.AppendLine("内容:" + content);
            str.AppendLine("-----------------------------------------------------------------------\r\n\r\n");

            try
            {
                lock (HashFiles[name])
                {
                    var path = (filePath ?? DefaultFilePath) + name;
                    var directoryName = new FileInfo(path).DirectoryName;
                    if (directoryName != null && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    File.AppendAllText(path, str.ToString(), Encoding.UTF8);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
