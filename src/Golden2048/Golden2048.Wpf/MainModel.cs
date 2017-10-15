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
        public bool IsStopped { get; set; } = true;
        public bool IsGameOver { get; set; } = false;
        public bool IsVictory { get; set; } = false;
        public int Points { get; set; } = 0;
        
        public DelegateCommand PullLeftCommand { get; }
        public DelegateCommand PullTopCommand { get; }
        public DelegateCommand PullRightCommand { get; }
        public DelegateCommand PullBottomCommand { get; }
        public DelegateCommand UndoCommand { get; }
        public DelegateCommand StartCommand { get; }
        public DelegateCommand ContinueCommand { get; }

        private readonly Board board;
        private bool beaten = false;

        public MainModel()
        {
            board = new Board();

            PullLeftCommand = new DelegateCommand(
                i =>
                {
                    board.PullLeft();
                    board.PutRandomValue();
                    Refresh();
                },
                i => board.CanPullLeft()
            );
            PullTopCommand = new DelegateCommand(
                i =>
                {
                    board.PullTop();
                    board.PutRandomValue();
                    Refresh();
                },
                i => board.CanPullTop()
            );
            PullRightCommand = new DelegateCommand(
                i =>
                {
                    board.PullRight();
                    board.PutRandomValue();
                    Refresh();
                },
                i => board.CanPullRight()
            );
            PullBottomCommand = new DelegateCommand(
                i =>
                {
                    board.PullBottom();
                    board.PutRandomValue();
                    Refresh();
                },
                i => board.CanPullBottom()
            );
            UndoCommand = new DelegateCommand(
                i =>
                {
                    board.Undo();
                    Refresh();
                }
            );
            StartCommand = new DelegateCommand(
                i =>
                {
                    board.Reset();
                    IsStopped = false;
                    IsGameOver = false;
                    IsVictory = false;
                    beaten = false;
                    board.PutRandomValue();
                    Refresh();
                }
            );
            ContinueCommand = new DelegateCommand(
                i => IsVictory = false
            );

            board.Initialize(
                2, 4, 8, 16,
                32, 64, 128, 256,
                512, 1024, 2048, 4096,
                8192, 16384, 32768, 65536
            );

            Refresh();
        }

        private void Refresh()
        {
            Values = board.Select(i => i.Value).ToList();
            Points = Values.Sum();
            PullLeftCommand.RaiseCanExecuteChanged();
            PullTopCommand.RaiseCanExecuteChanged();
            PullRightCommand.RaiseCanExecuteChanged();
            PullBottomCommand.RaiseCanExecuteChanged();
            if (!IsStopped)
            {
                if (!beaten && Values.Contains(2048))
                {
                    beaten = true;
                    IsVictory = true;
                }
                else
                {
                    if (!(board.CanPullLeft() || board.CanPullTop() || board.CanPullRight() || board.CanPullBottom())) IsGameOver = true;
                }
            }
        }
    }
}
