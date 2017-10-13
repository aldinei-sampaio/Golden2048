using Golden2048.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Golden2048.Wpf
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class MainModel
    {
        public bool CanPullLeft { get; set; }
        public bool CanPullTop { get; set; }
        public bool CanPullRight { get; set; }
        public bool CanPullBottom { get; set; }
        public List<int> Values { get; set; }

        public DelegateCommand PullLeftCommand { get; }
        public DelegateCommand PullTopCommand { get; }
        public DelegateCommand PullRightCommand { get; }
        public DelegateCommand PullBottomCommand { get; }

        private readonly Board board;

        public MainModel()
        {
            board = new Board();

            PullLeftCommand = new DelegateCommand(
                i =>
                {
                    board.PullLeft();
                    Refresh();
                },
                i => board.CanPullLeft()
            );
            PullTopCommand = new DelegateCommand(
                i =>
                {
                    board.PullTop();
                    Refresh();
                },
                i => board.CanPullTop()
            );
            PullRightCommand = new DelegateCommand(
                i =>
                {
                    board.PullRight();
                    Refresh();
                },
                i => board.CanPullRight()
            );
            PullBottomCommand = new DelegateCommand(
                i =>
                {
                    board.PullBottom();
                    Refresh();
                },
                i => board.CanPullBottom()
            );

            Refresh();
        }

        private void Refresh()
        {
            board.PutRandomValue();
            Values = board.Select(i => i.Value).ToList();
            PullLeftCommand.RaiseCanExecuteChanged();
            PullTopCommand.RaiseCanExecuteChanged();
            PullRightCommand.RaiseCanExecuteChanged();
            PullBottomCommand.RaiseCanExecuteChanged();
        }
    }
}
