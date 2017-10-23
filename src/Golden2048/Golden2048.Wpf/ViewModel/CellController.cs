using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Golden2048.Wpf
{
    public class CellController
    {
        private readonly StoryboardController controller;
        public Image Component { get; }

        public CellController(StoryboardController controller, Image img) => (this.Component, this.controller) = (img, controller);

        private const int scale = 100;

        private int _left;
        public int Left
        {
            get => _left;
            set
            {
                _left = value;
                Component.SetValue(Canvas.LeftProperty, (double)value * scale);
            }
        }

        private int _top;
        public int Top
        {
            get => _top;
            set
            {
                _top = value;
                Component.SetValue(Canvas.TopProperty, (double)value * scale);
            }
        }

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                Component.Source = ImageHelper.GetImage(value);
            }
        }

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                Component.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Show()
        {
            IsVisible = true;
            var animation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            Component.BeginAnimation(Image.OpacityProperty, animation);
        }

        private Storyboard CreateMoveStoryboard(int x, int y)
        {
            var sb = new Storyboard();

            if (x != Left)
            {
                var leftAnimation = new DoubleAnimation(Left * scale, x * scale, TimeSpan.FromMilliseconds(200));
                Storyboard.SetTarget(leftAnimation, Component);
                Storyboard.SetTargetProperty(leftAnimation, new PropertyPath(Canvas.LeftProperty));
                sb.Children.Add(leftAnimation);
                Left = x;
            }

            if (y != Top)
            {
                var topAnimation = new DoubleAnimation(Top * scale, y * scale, TimeSpan.FromMilliseconds(200));
                Storyboard.SetTarget(topAnimation, Component);
                Storyboard.SetTargetProperty(topAnimation, new PropertyPath(Canvas.TopProperty));
                sb.Children.Add(topAnimation);
                Top = y;
            }

            return sb;
        }

        public void Hide(int x, int y, Action onCompleted)
        {
            var sb = CreateMoveStoryboard(x, y);

            var animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
            animation.Completed += (sender, e) => IsVisible = false;
            Storyboard.SetTarget(animation, Component);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Image.OpacityProperty));
            sb.Children.Add(animation);

            sb.Completed += (sender, e) =>
            {
                IsVisible = false;
                onCompleted();
            };

            sb.Begin();
        }

        public void Move(int x, int y)
        {
            var sb = CreateMoveStoryboard(x, y);
            controller.Begin(sb);
        }

        public void MoveAndFlipHorizontally(int x, int y, int value)
        {
            var sb = CreateMoveStoryboard(x, y);

            var leftOriginal = x * scale;
            var leftChanged = leftOriginal + 50;
            var image = ImageHelper.GetImage(value);
            var duration = TimeSpan.FromMilliseconds(100);
            var startTime = TimeSpan.FromMilliseconds(sb.Children.Any() ? 200 : 0);
            var startTime2 = startTime + duration;

            var animWidth1 = new DoubleAnimation(100, 0, duration);
            animWidth1.BeginTime = startTime;
            Storyboard.SetTarget(animWidth1, Component);
            Storyboard.SetTargetProperty(animWidth1, new PropertyPath(Image.WidthProperty));
            sb.Children.Add(animWidth1);

            var animLeft1 = new DoubleAnimation(leftOriginal, leftChanged, duration);
            animLeft1.BeginTime = startTime;
            Storyboard.SetTarget(animLeft1, Component);
            Storyboard.SetTargetProperty(animLeft1, new PropertyPath(Canvas.LeftProperty));
            sb.Children.Add(animLeft1);

            var changeImage = new ObjectAnimationUsingKeyFrames();
            changeImage.BeginTime = startTime2;
            changeImage.Duration = TimeSpan.Zero;
            Storyboard.SetTarget(changeImage, Component);
            Storyboard.SetTargetProperty(changeImage, new PropertyPath(Image.SourceProperty));
            changeImage.KeyFrames.Add(new DiscreteObjectKeyFrame(image));
            changeImage.Completed += (sender, e) => Value = value;
            sb.Children.Add(changeImage);

            var animWidth2 = new DoubleAnimation(0, 100, duration);
            animWidth2.BeginTime = startTime2;
            Storyboard.SetTarget(animWidth2, Component);
            Storyboard.SetTargetProperty(animWidth2, new PropertyPath(Image.WidthProperty));
            sb.Children.Add(animWidth2);

            var animLeft2 = new DoubleAnimation(leftChanged, leftOriginal, duration);
            animLeft2.BeginTime = startTime2;
            Storyboard.SetTarget(animLeft2, Component);
            Storyboard.SetTargetProperty(animLeft2, new PropertyPath(Canvas.LeftProperty));
            sb.Children.Add(animLeft2);

            controller.Begin(sb);
        }

        public void MoveAndFlipVertically(int x, int y, int value)
        {
            var sb = CreateMoveStoryboard(x, y);

            var originalPos = y * scale;
            var changedPos = originalPos + 50;
            var image = ImageHelper.GetImage(value);
            var duration = TimeSpan.FromMilliseconds(100);
            var startTime = TimeSpan.FromMilliseconds(sb.Children.Any() ? 200 : 0);
            var startTime2 = startTime + duration;

            var animWidth1 = new DoubleAnimation(100, 0, duration);
            animWidth1.BeginTime = startTime;
            Storyboard.SetTarget(animWidth1, Component);
            Storyboard.SetTargetProperty(animWidth1, new PropertyPath(Image.HeightProperty));
            sb.Children.Add(animWidth1);

            var animLeft1 = new DoubleAnimation(originalPos, changedPos, duration);
            animLeft1.BeginTime = startTime;
            Storyboard.SetTarget(animLeft1, Component);
            Storyboard.SetTargetProperty(animLeft1, new PropertyPath(Canvas.TopProperty));
            sb.Children.Add(animLeft1);

            var changeImage = new ObjectAnimationUsingKeyFrames();
            changeImage.BeginTime = startTime2;
            changeImage.Duration = TimeSpan.Zero;
            Storyboard.SetTarget(changeImage, Component);
            Storyboard.SetTargetProperty(changeImage, new PropertyPath(Image.SourceProperty));
            changeImage.KeyFrames.Add(new DiscreteObjectKeyFrame(image));
            changeImage.Completed += (sender, e) => Value = value;
            sb.Children.Add(changeImage);

            var animWidth2 = new DoubleAnimation(0, 100, duration);
            animWidth2.BeginTime = startTime2;
            Storyboard.SetTarget(animWidth2, Component);
            Storyboard.SetTargetProperty(animWidth2, new PropertyPath(Image.HeightProperty));
            sb.Children.Add(animWidth2);

            var animLeft2 = new DoubleAnimation(changedPos, originalPos, duration);
            animLeft2.BeginTime = startTime2;
            Storyboard.SetTarget(animLeft2, Component);
            Storyboard.SetTargetProperty(animLeft2, new PropertyPath(Canvas.TopProperty));
            sb.Children.Add(animLeft2);

            controller.Begin(sb);
        }

    }
}
