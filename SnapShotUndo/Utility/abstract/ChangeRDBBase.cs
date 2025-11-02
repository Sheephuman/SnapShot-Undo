using System.Windows;
using System.Windows.Media;

namespace SnapShotUndo.Utility.abstractImplement
{
    public abstract class ChangeRDBBase
    {

        public Brush Change(RoutedPropertyChangedEventArgs<double> e, Color currentColor)
        {
            byte value = (byte)e.NewValue;
            var newColor = GetColor(value, currentColor);
            return new SolidColorBrush(newColor);
        }

        protected abstract Color GetColor(byte value, Color currentColor);

    }
}
