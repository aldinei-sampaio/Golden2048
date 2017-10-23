using System.Collections.Generic;
using System.Timers;

namespace Golden2048.Wpf
{
    using System;
    using System.ComponentModel;

    public class Fireworks
    {
        private readonly Timer timer = new Timer(50);

        public List<FireworkItem> Items { get; }

        public Fireworks(double center)
        {
            var r = new Random();
            Func<double> n = () => 2 * (Math.Sqrt(-2 * Math.Log(r.NextDouble())) * Math.Sin(6 * r.NextDouble()));
            Items = new List<FireworkItem>();
            for (var a = 0; a++ < 300;)
            {
                Items.Add(new FireworkItem { X = center, Y = center, f = n(), g = n() });
            }
            var counter = 0;
            timer.Elapsed += (sender, e) =>
            {
                counter++;
                if (counter >= 100)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                foreach (var x in Items)
                {
                    x.X += x.f;
                    x.Y += x.g += .2;
                    x.O = Math.Max(1 - Math.Sqrt(Math.Pow(center - x.X, 2) + Math.Pow(center - x.Y, 2)) / center, 0);
                }
            };
            timer.Start();
        }
    }

    public class FireworkItem : System.Windows.ContentElement, INotifyPropertyChanged
    {
        public double y, f, g;
        public double X { get; set; }
        public double O { get; set; }
        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
