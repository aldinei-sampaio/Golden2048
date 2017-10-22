using Golden2048.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Golden2048.Wpf
{
    /// <summary>
    /// Lógica interna para Test2Window.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Board board;
        private List<CellController> items;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            InitializeBoard();

            this.KeyUp += (sender, e1) =>
            {
                switch(e1.Key)
                {
                    case Key.Left: GoLeft(); break;
                    case Key.Right: GoRight(); break;
                    case Key.Up: GoUp(); break;
                    case Key.Down: GoDown(); break;
                    case Key.R:
                        if (e1.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e1.KeyboardDevice.IsKeyDown(Key.RightCtrl)) Reset();
                        break;
                }
            };
        }

        private void InitializeBoard()
        {
            items = new List<CellController>();
            board = new Board();

            board.CellCreated += (sender, e) =>
            {
                var img = new Image();
                img.Stretch = System.Windows.Media.Stretch.Fill;
                this.BoardSpace.Children.Add(img);
                var cell = new CellController(img);
                items.Add(cell);
                cell.Value = e.Created.Value;
                cell.Left = e.Created.X;
                cell.Top = e.Created.Y;
                cell.Show();
            };

            board.CellMoved += (sender, e) =>
            {
                var cell = items.First(i => i.IsVisible && i.Left == e.From.X && i.Top == e.From.Y);

                if (e.Merged.HasValue)
                {
                    var destroyed = items.First(i => i.IsVisible && i.Left == e.Merged.Value.X && i.Top == e.Merged.Value.Y);

                    if (destroyed.Left != e.To.X) cell.MoveAndFlipHorizontally(e.To.X, e.To.Y, e.To.Value);
                    else cell.MoveAndFlipVertically(e.To.X, e.To.Y, e.To.Value);

                    destroyed.Hide(e.To.X, e.To.Y, () => {
                        this.BoardSpace.Children.Remove(destroyed.Component);
                        items.Remove(destroyed);
                    });
                }
                else
                {
                    cell.Move(e.To.X, e.To.Y);
                }
            };

            board.PutRandomValue();
        }

        public string GetDebugMessage()
        {
            return String.Join(", ", items.Select(i => $"({i.Left}, {i.Top}, {i.Value})"));
        }

        private void GoLeft()
        {
            if (!board.CanPullLeft()) return;
            board.PullLeft();
            board.PutRandomValue();
        }

        private void GoUp()
        {
            if (!board.CanPullUp()) return;
            board.PullUp();
            board.PutRandomValue();
        }

        private void GoRight()
        {
            if (!board.CanPullRight()) return;
            board.PullRight();
            board.PutRandomValue();
        }

        private void GoDown()
        {
            if (!board.CanPullDown()) return;
            board.PullDown();
            board.PutRandomValue();
        }

        private void Reset()
        {
            this.BoardSpace.Children.Clear();
            InitializeBoard();
        }
    }
}
