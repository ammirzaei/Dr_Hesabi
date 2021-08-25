using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Hesabi.Classes.Class
{
    public static class CheckFormat
    {
        public static bool CheckFormatImage(string ImageName)
        {
            string format = Path.GetExtension(ImageName).ToLower();

            switch (format)
            {
                case ".jpg":
                case ".png":
                case ".jpeg":
                case ".bmp":
                case ".pjpeg":
                case ".svg":
                case ".gif":
                    {
                        return true;
                    }
            }
            return false;
        }
        public static bool CheckFormatVideo(string VideoName)
        {
            string format = Path.GetExtension(VideoName).ToLower();

            switch (format)
            {
                case ".mp4":
                case ".amv":
                case ".avi":
                case ".mkv":
                case ".webm":
                case ".ogg":
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckFormatAudio(string AudioName)
        {
            string format = Path.GetExtension(AudioName).ToLower();

            switch (format)
            {
                case ".mp3":
                case ".m4a":
                case ".mpc":
                {
                    return true;
                }
            }
            return false;
        }
    }
}
