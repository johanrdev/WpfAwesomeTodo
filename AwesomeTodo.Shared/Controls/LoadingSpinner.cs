using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AwesomeTodo.Shared.Controls
{
    public class LoadingSpinner : Control
    {
        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingSpinner), new PropertyMetadata(false));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Diameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register("Diameter", typeof(double), typeof(LoadingSpinner), new PropertyMetadata(45.0));

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(double), typeof(LoadingSpinner), new PropertyMetadata(5.0));

        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(LoadingSpinner), new PropertyMetadata(Brushes.LightGray));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        static LoadingSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingSpinner), new FrameworkPropertyMetadata(typeof(LoadingSpinner)));
        }
    }
}
