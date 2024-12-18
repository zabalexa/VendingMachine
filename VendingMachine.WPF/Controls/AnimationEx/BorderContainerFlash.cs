using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace VendingMachine.WPF
{
    public class BorderContainer : Border
    {
        #region VisibilityBool Property
        public bool VisibilityBool
        {
            get { return (bool) GetValue(VisibilityBoolProperty); }
            set { SetValue(VisibilityBoolProperty, value); }
        }

        public static readonly DependencyProperty VisibilityBoolProperty = DependencyProperty.Register("VisibilityBool", typeof (bool), typeof (BorderContainer), new UIPropertyMetadata(false, new PropertyChangedCallback(VisibilityChanged)), new ValidateValueCallback(ValidateVisibility));

        public static bool ValidateVisibility(object value)
        {
            bool res;
            return bool.TryParse($"{value}", out res);
        }

        private static void VisibilityChanged(DependencyObject control, DependencyPropertyChangedEventArgs property)
        {
            ((BorderContainer)control).Visibility = ((bool)property.NewValue) ? Visibility.Visible : Visibility.Hidden;
        }
        #endregion

        public void Initialize(bool initValue)
        {
            Reset(!initValue);
            VisibilityBool = initValue;
        }

        public void Reset(bool visible = false)
        {
            if (VisibilityBool != visible)
            {
                BooleanKeyFrameCollection bools = new BooleanKeyFrameCollection();
                bools.Add(new DiscreteBooleanKeyFrame(visible));
                BeginAnimation(VisibilityBoolProperty, new BooleanAnimationUsingKeyFrames() { KeyFrames = bools, BeginTime = new TimeSpan(), Duration = new Duration(new TimeSpan(1)) }, HandoffBehavior.SnapshotAndReplace);
            }
        }
    }

    public class BorderContainerFlash : BorderContainer
    {
        #region RoutedEvent Definition
        public static readonly RoutedEvent FlashAndFillCupEvent = EventManager.RegisterRoutedEvent("FlashAndFillCup", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(BorderContainerFlash));

        public event RoutedEventHandler FlashAndFillCup
        {
            add { AddHandler(FlashAndFillCupEvent, value); }
            remove { RemoveHandler(FlashAndFillCupEvent, value); }
        }

        private void RaiseFlashAndFillCupEvent()
        {
            RaiseEvent(new RoutedEventArgs(FlashAndFillCupEvent, this));
        }
        #endregion

        public void FlashAndFillCupBegin()
        {
            RaiseFlashAndFillCupEvent();
        }
    }
}
