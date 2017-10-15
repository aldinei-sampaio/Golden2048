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
    public class CellValueConverter : IValueConverter
    {
        private static Dictionary<int, BitmapImage> images;
        
        static CellValueConverter()
        {
            images = new Dictionary<int, BitmapImage>
            {
                { 2, GetImage(2) },
                { 4, GetImage(4) },
                { 8, GetImage(8) },
                { 16, GetImage(16) },
                { 32, GetImage(32) },
                { 64, GetImage(64) },
                { 128, GetImage(128) },
                { 256, GetImage(256) },
                { 512, GetImage(512) },
                { 1024, GetImage(1024) },
                { 2048, GetImage(2048) },
                { 4096, GetImage(4096) },
                { 8192, GetImage(8192) },
                { 16384, GetImage(16384) },
                { 32768, GetImage(32768) },
                { 65536, GetImage(65536) },
            };
        }

        private static BitmapImage GetImage(int value)
        {
            var src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri($"pack://application:,,,/Golden2048.Wpf;component/Images/{value}.png");
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            return src;
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as int?;
            if (v.HasValue && v.Value > 0) return images[v.Value];
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
