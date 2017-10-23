using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Golden2048.Wpf
{
    public class StoryboardController
    {
        private int _runningCount;
        public int RunningCount => _runningCount;

        public void Begin(Storyboard sb)
        {
            sb.Completed += (sender, e) => Interlocked.Decrement(ref _runningCount);
            Interlocked.Increment(ref _runningCount);
            sb.Begin();
        }
    }
}
