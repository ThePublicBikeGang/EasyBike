using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PublicBikes.WinPhone.Helpers
{
    public static class VisualStatesHelper
    {
        public class VisualStateEventArgs : EventArgs
        {
            public VisualStateEventArgs(DependencyPropertyChangedEventArgs e)
            {
                EventArgs = e;
            }

            public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
        }

        public static event EventHandler<VisualStateEventArgs> VisualStateChanged;

        static readonly DependencyProperty CurrentVisualStateProperty = DependencyProperty.RegisterAttached("CurrentVisualState", typeof(String), typeof(VisualStatesHelper), new PropertyMetadata(null, TransitionToMasterState));
        public static string GetCurrentVisualState(DependencyObject obj)
        {
            return (string)obj.GetValue(CurrentVisualStateProperty);
        }

        public static void SetCurrentVisualState(DependencyObject obj, string value)
        {
            obj.SetValue(CurrentVisualStateProperty, value);
        }

        private static void TransitionToMasterState(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (_previousState != (string)args.NewValue)
            {
                if (VisualStateChanged != null)
                    VisualStateChanged(args.NewValue, new VisualStateEventArgs(args));
                _previousState = (string)args.NewValue;
            }
            var c = sender as Control;
            if (c != null)
                VisualStateManager.GoToState(c, (string)args.NewValue, true);
            else
                throw new ArgumentException("CurrentState is only supported on the Control type");
        }

        private static string _previousState;
    }
}
