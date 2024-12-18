using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace VendingMachine.WPF
{
    public class FillCupProgressBar: ProgressBar
    {
        #region RoutedEvent Definition
        public static readonly RoutedEvent FillEmptyCupEvent = EventManager.RegisterRoutedEvent("FillEmptyCup", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(FillCupProgressBar));

        public event RoutedEventHandler FillEmptyCup
        {
            add { AddHandler(FillEmptyCupEvent, value); }
            remove { RemoveHandler(FillEmptyCupEvent, value); }
        }

        private void RaiseFillEmptyCupEvent()
        {
            RaiseEvent(new RoutedEventArgs(FillEmptyCupEvent, this));
        }
        #endregion

        public void FillEmptyCupBegin()
        {
            RaiseFillEmptyCupEvent();
        }

        public void Reset()
        {
            BeginAnimation(ValueProperty, new DoubleAnimation(0, new Duration(new TimeSpan(1))), HandoffBehavior.SnapshotAndReplace);
        }
    }
}
