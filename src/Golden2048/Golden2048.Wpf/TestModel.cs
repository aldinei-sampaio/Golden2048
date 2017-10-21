using Golden2048.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golden2048.Wpf
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GameCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Value { get; set; }
        public bool IsVisible { get; set; }
        public int Transition { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TestModel
    {
        public ObservableCollection<GameCell> Items { get; set; }

        public DelegateCommand PullLeftCommand { get; }
        public DelegateCommand PullTopCommand { get; }
        public DelegateCommand PullRightCommand { get; }
        public DelegateCommand PullBottomCommand { get; }
        public DelegateCommand ChangeValueCommand { get; }

        private Board board;

        public TestModel()
        {
            Items = new ObservableCollection<GameCell>()
            {
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0},
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0},
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 },
                new GameCell { IsVisible = false, Value = 0, Left = 0, Top = 0 }
            };

            board = new Board();

            var testCell = Items[0];

            PullLeftCommand = new DelegateCommand(
                i =>
                {
                    if (testCell.X == 0)
                    {
                        testCell.X = 3;
                        testCell.Transition = 403;
                    }
                    else
                    {
                        testCell.X--;
                        testCell.Transition = 400 + testCell.X;
                    }

                    //board.PullLeft();
                    //board.PutValue(2, 2, 2);
                    //board.PutRandomValue();
                    //board.Compare(GetValues());
                }//,
                //i => board.CanPullLeft()
            );
            PullTopCommand = new DelegateCommand(
                i =>
                {
                    if (testCell.Transition == 10) testCell.Transition = 106;
                    else if ((testCell.Transition - 100) / 4 == 0) testCell.Transition += 12;
                    else testCell.Transition-=4;
                    //board.PullTop();
                    //board.PutValue(2, 2, 2);
                    //board.PutRandomValue();
                    //board.Compare(GetValues());
                }//,
                //i => board.CanPullTop()
            );
            PullRightCommand = new DelegateCommand(
                i =>
                {
                    if (testCell.Transition == 10) testCell.Transition = 111;
                    else if ((testCell.Transition - 100) % 4 == 3) testCell.Transition -= 4;
                    else testCell.Transition++;
                    //board.PullRight();
                    //board.PutValue(2, 2, 2);
                    //board.PutRandomValue();
                    //board.Compare(GetValues());
                }//,
                //i => board.CanPullRight()
            );
            PullBottomCommand = new DelegateCommand(
                i =>
                {
                    if (testCell.Transition == 10) testCell.Transition = 114;
                    else if ((testCell.Transition - 100) / 4 == 3) testCell.Transition -= 12;
                    else testCell.Transition+=4;
                    //board.PullBottom();
                    //board.PutValue(2, 2, 2);
                    //board.PutRandomValue();
                    //board.Compare(GetValues());
                }//,
                //i => board.CanPullBottom()
            );
            ChangeValueCommand = new DelegateCommand(
                i =>
                {
                    var cell = Items[0];
                    switch (cell.Transition)
                    {
                        case 10:
                        case 110:
                            cell.Transition = 108;
                            break;
                        case 108:
                            cell.Transition = 100;
                            break;
                        case 100:
                            cell.Transition = 102;
                            break;
                        case 102:
                            cell.Transition = 110;
                            break;
                    } 
                }
            );

            board.CellCreated += (sender, e) =>
            {
                var cell = Items.FirstOrDefault(i => !i.IsVisible);
                cell.Value = e.Created.Value;
                cell.IsVisible = true;
                cell.X = e.Created.X;
                cell.Y = e.Created.Y;
                cell.Transition = e.Created.Index;
            };

            board.CellMerged += (sender, e) =>
            {
                var destroyed = Items.First(i => i.IsVisible && i.X == e.Destroyed.X && i.Y == e.Destroyed.Y);
                destroyed.Transition = 200 + e.Merged.Index;
                destroyed.IsVisible = false;

                var merged = Items.First(i => i.IsVisible && i.X == e.Merged.X && i.Y == e.Merged.Y);
                merged.Value = e.Merged.Value;
                merged.Transition = (merged.Transition == 300) ? 301 : 300;
                merged.X = e.Merged.X;
                merged.Y = e.Merged.Y;
            };

            board.CellMoved += (sender, e) =>
            {
                var cell = Items.First(i => i.IsVisible && i.X == e.From.X && i.Y == e.From.Y);
                cell.X = e.To.X;
                cell.Y = e.To.Y;
                cell.Transition = 100 + e.To.Index;
            };

            //board.PutValue(2, 2, 2);
            //board.PutRandomValue();
            testCell.Value = 2048;
            testCell.X = 2;
            testCell.Y = 2;
            testCell.Transition = 402;
        }

        private int[] GetValues()
        {
            var result = new int[16];
            var n = 0;
            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    var cell = Items.FirstOrDefault(i => i.IsVisible && i.X == x && i.Y == y);
                    result[n] = (cell == null) ? 0 : cell.Value;
                    n++;
                }
            }
            return result;
        }
    }
}
