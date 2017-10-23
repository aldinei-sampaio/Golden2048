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
        private StoryboardController controller = new StoryboardController();
        private DateTime startTime;
        private System.Timers.Timer timer;

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
            startTime = DateTime.Now;
            timer?.Dispose();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) =>
            {
                var elapsed = DateTime.Now - startTime;
                Application.Current.Dispatcher.BeginInvoke(new Action(() => Time.Text = elapsed.ToString(@"hh\:mm\:ss")));
            };
            timer.Start();

            items = new List<CellController>();
            board = new Board();

            board.CellCreated += (sender, e) =>
            {
                var img = new Image();
                img.Stretch = System.Windows.Media.Stretch.Fill;
                this.BoardSpace.Children.Add(img);
                var cell = new CellController(controller, img);
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
            Refresh();
        }

        public string GetDebugMessage()
        {
            return String.Join(", ", items.Select(i => $"({i.Left}, {i.Top}, {i.Value})"));
        }

        private void GoLeft()
        {
            if (controller.RunningCount > 0) return;
            if (!board.CanPullLeft()) return;
            board.PullLeft();
            board.PutRandomValue();
            Refresh();
        }

        private void GoUp()
        {
            if (controller.RunningCount > 0) return;
            if (!board.CanPullUp()) return;
            board.PullUp();
            board.PutRandomValue();
            Refresh();
        }

        private void GoRight()
        {
            if (controller.RunningCount > 0) return;
            if (!board.CanPullRight()) return;
            board.PullRight();
            board.PutRandomValue();
            Refresh();
        }

        private void GoDown()
        {
            if (controller.RunningCount > 0) return;
            if (!board.CanPullDown()) return;
            board.PullDown();
            board.PutRandomValue();
            Refresh();
        }

        private void Reset()
        {
            this.BoardSpace.Children.Clear();
            InitializeBoard();
        }

        private void Restart_Click(object sender, RoutedEventArgs e) => Reset();
        private void Up_Click(object sender, RoutedEventArgs e) => GoUp();
        private void Left_Click(object sender, RoutedEventArgs e) => GoLeft();
        private void Down_Click(object sender, RoutedEventArgs e) => GoDown();
        private void Right_Click(object sender, RoutedEventArgs e) => GoRight();

        private void Refresh()
        {
            Up.IsEnabled = board.CanPullUp();
            Left.IsEnabled = board.CanPullLeft();
            Down.IsEnabled = board.CanPullDown();
            Right.IsEnabled = board.CanPullRight();
            Points.Text = board.Points.ToString("N0");

            if (board.Combo > 1)
            {
                Combo.Text = $"Combo {board.Combo}!";
            }
            else
            {
                Combo.Text = string.Empty;
            }
        }

        private void Boom_Click(object sender, RoutedEventArgs e) => GoBoom();

        private void GoBoom()
        {
            var fireworks = new Fireworks(150.0);
            i.ItemsSource = fireworks.Items;
        }
    }
}
