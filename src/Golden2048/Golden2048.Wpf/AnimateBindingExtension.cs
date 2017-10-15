using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Golden2048.Wpf
{
    public class AnimateBindingExtension : MarkupExtension
    {
        static DependencyPropertyDescriptor dpd =
            DependencyPropertyDescriptor.FromProperty(DoubleAnimation.ToProperty,
                typeof(DoubleAnimation));

        public AnimateBindingExtension(PropertyPath path)
        {
            Path = path;
        }

        public bool ValidatesOnExceptions { get; set; }
        public IValueConverter Converter { get; set; }
        public object ConverterParamter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object Source { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }
        public object TargetNullValue { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (valueProvider == null)
            {
                throw new Exception("could not get IProviderValueTarget service.");
            }

            var bindingTarget = valueProvider.TargetObject as FrameworkElement;
            var bindingProperty = valueProvider.TargetProperty as DependencyProperty;

            if (bindingProperty == null || bindingTarget == null)
            {
                throw new Exception();
            }

            var binding = new Binding
            {
                Path = Path,
                Converter = Converter,
                ConverterParameter = ConverterParamter,
                ValidatesOnDataErrors = ValidatesOnDataErrors,
                ValidatesOnExceptions = ValidatesOnExceptions,
                TargetNullValue = TargetNullValue
            };

            if (ElementName != null) binding.ElementName = ElementName;
            else if (RelativeSource != null) binding.RelativeSource = RelativeSource;
            else if (Source != null) binding.Source = Source;

            // you can add a Duration property to this class and use it here
            var anim = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };
            // this can be a new subclass of DoubleAnimation that 
            // overrides ToProperty metadata and add a property 
            // change callback
            dpd.AddValueChanged(anim, (s, e) => bindingTarget.BeginAnimation(bindingProperty, anim));

            BindingOperations.SetBinding(anim, DoubleAnimation.ToProperty, binding);
            // this is because we need to catch the DataContext so add animation object 
            // to the visual tree by adding it to target object's resources.
            bindingTarget.Resources[bindingProperty.Name] = anim;
            // animation will set the value
            return DependencyProperty.UnsetValue;
        }
    }


    public class ColorAnimationBehavior : TriggerAction<UIElement>
    {
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register("FillColor", typeof(Color), typeof(ColorAnimationBehavior), null);

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(ColorAnimationBehavior), null);

        protected override void Invoke(object parameter)
        {
            var storyboard = new Storyboard();
            storyboard.Children.Add(CreateColorAnimation(this.AssociatedObject, this.Duration, this.FillColor));
            storyboard.Begin();
        }

        private static ColorAnimationUsingKeyFrames CreateColorAnimation(UIElement element, Duration duration, Color color)
        {
            var animation = new ColorAnimationUsingKeyFrames();
            animation.KeyFrames.Add(new SplineColorKeyFrame() { KeyTime = duration.TimeSpan, Value = color });
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Shape.Fill).(SolidColorBrush.Color)"));
            Storyboard.SetTarget(animation, element);
            return animation;
        }
    }

    public class LeftAnimationBehavior : TriggerAction<UIElement>
    {
        public double Left
        {
            get => (double)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(LeftAnimationBehavior), null);

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(LeftAnimationBehavior), null);

        protected override void Invoke(object parameter)
        {
            var storyboard = new Storyboard();
            storyboard.Children.Add(CreateColorAnimation(this.AssociatedObject, this.Duration, this.Left));
            storyboard.Begin();
        }

        private static DoubleAnimation CreateColorAnimation(UIElement element, Duration duration, double left)
        {
            var anim = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8,
                To = left
            };
            Storyboard.SetTargetProperty(anim, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(anim, element);
            return anim;
        }
    }

}

