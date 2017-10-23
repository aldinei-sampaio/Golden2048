using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golden2048.Wpf.Util
{
    public class GameSettings
    {
        public List<GameRecord> Records { get; set; }
    }

    public class GameRecord
    {
        public DateTime Date { get; set; }
        public int Points { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
