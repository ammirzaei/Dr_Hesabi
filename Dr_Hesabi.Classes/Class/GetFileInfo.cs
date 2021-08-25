using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dr_Hesabi.Classes.Class
{
    public static class GetFileInfo
    {
        public static string GetSize(string url)
        {
            FileInfo file = new FileInfo(url);
            var size = Double.Parse(file.Length.ToString()) / 1024 / 1024;
            return size.ToString("N2") + " مگابایت ";
        }
    }
}
