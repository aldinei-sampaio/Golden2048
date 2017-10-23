using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Golden2048.Wpf
{
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public static DependencyProperty HoverImageProperty =
            DependencyProperty.RegisterAttached(
                "HoverImage", 
                typeof(ImageSource), 
                typeof(ImageButton), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public ImageSource HoverImage
        {
            get => (ImageSource)GetValue(HoverImageProperty);
            set => SetValue(HoverImageProperty, value);
        }

        public static DependencyProperty NormalImageProperty =
            DependencyProperty.RegisterAttached(
                "NormalImage",
                typeof(ImageSource),
                typeof(ImageButton),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public ImageSource NormalImage
        {
            get => (ImageSource)GetValue(NormalImageProperty);
            set => SetValue(NormalImageProperty, value);
        }

        public static DependencyProperty PressedImageProperty =
            DependencyProperty.RegisterAttached(
                "PressedImage",
                typeof(ImageSource),
                typeof(ImageButton),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public ImageSource PressedImage
        {
            get => (ImageSource)GetValue(PressedImageProperty);
            set => SetValue(PressedImageProperty, value);
        }

        public static DependencyProperty DisabledImageProperty =
            DependencyProperty.RegisterAttached(
                "DisabledImage",
                typeof(ImageSource),
                typeof(ImageButton),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public ImageSource DisabledImage
        {
            get => (ImageSource)GetValue(DisabledImageProperty);
            set => SetValue(DisabledImageProperty, value);
        }

        public static DependencyProperty ImageRotationProperty =
            DependencyProperty.RegisterAttached(
                "ImageRotation",
                typeof(double),
                typeof(ImageButton),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public double ImageRotation
        {
            get => (double)GetValue(ImageRotationProperty);
            set => SetValue(ImageRotationProperty, value);
        }
    }
}
