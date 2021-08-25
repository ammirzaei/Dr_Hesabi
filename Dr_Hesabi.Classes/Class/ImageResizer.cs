﻿using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Dr_Hesabi.Classes.Class
{
    public class ImageResizer
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }
        public bool TrimImage { get; set; }
        public ImageFormat SaveFormat { get; set; }
        public ImageResizer(int Size)
        {
            MaxY = MaxX = Size;
            TrimImage = false;
            SaveFormat = ImageFormat.Jpeg;
        }
        public bool Resize(string source, string target)
        {
            using (Image src = Image.FromFile(source, true))
            {
                // Check that we have an image
                if (src != null)
                {
                    int origX, origY, newX, newY;
                    int trimX = 0, trimY = 0;

                    // Default to size of source image
                    newX = origX = src.Width;
                    newY = origY = src.Height;

                    // Does image exceed maximum dimensions?
                    if (origX > MaxX || origY > MaxY)
                    {
                        // Need to resize image
                        if (TrimImage)
                        {
                            // Trim to exactly fit maximum dimensions
                            double factor = Math.Max((double)MaxX / (double)origX,
                                (double)MaxY / (double)origY);
                            newX = (int)Math.Ceiling((double)origX * factor);
                            newY = (int)Math.Ceiling((double)origY * factor);
                            trimX = newX - MaxX;
                            trimY = newY - MaxY;
                        }
                        else
                        {
                            // Resize (no trim) to keep within maximum dimensions
                            double factor = Math.Min((double)MaxX / (double)origX,
                                (double)MaxY / (double)origY);
                            newX = (int)Math.Ceiling((double)origX * factor);
                            newY = (int)Math.Ceiling((double)origY * factor);
                        }
                    }

                    // Create destination image
                    using (Image dest = new Bitmap(newX - trimX, newY - trimY))
                    {
                        Graphics graph = Graphics.FromImage(dest);
                        graph.InterpolationMode =
                            System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graph.DrawImage(src, -(trimX / 2), -(trimY / 2), newX, newY);
                        dest.Save(target, SaveFormat);
                        // Indicate success
                        return true;
                    }
                }
            }
            // Indicate failure
            return false;
        }
    }
}
