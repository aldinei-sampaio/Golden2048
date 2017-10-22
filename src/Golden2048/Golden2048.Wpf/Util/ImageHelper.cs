using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Golden2048.Wpf
{
    public class ImageHelper
    {
        private static Dictionary<int, BitmapImage> images;
        
        static ImageHelper()
        {
            images = new Dictionary<int, BitmapImage>
            {
                { 2, LoadImage(2) },
                { 4, LoadImage(4) },
                { 8, LoadImage(8) },
                { 16, LoadImage(16) },
                { 32, LoadImage(32) },
                { 64, LoadImage(64) },
                { 128, LoadImage(128) },
                { 256, LoadImage(256) },
                { 512, LoadImage(512) },
                { 1024, LoadImage(1024) },
                { 2048, LoadImage(2048) },
                { 4096, LoadImage(4096) },
                { 8192, LoadImage(8192) },
                { 16384, LoadImage(16384) },
                { 32768, LoadImage(32768) },
                { 65536, LoadImage(65536) },
            };
        }

        private static BitmapImage LoadImage(int value)
        {
            var src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri($"pack://application:,,,/Golden2048.Wpf;component/Images/{value}.png");
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            return src;
        }

        public static BitmapImage GetImage(int value)
        {
            return images[value];
        }
    }
}
