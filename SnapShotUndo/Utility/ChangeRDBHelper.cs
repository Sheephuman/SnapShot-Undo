using SnapShotUndo.Utility.abstractImplement;
using System.Windows.Media;

namespace SnapShotUndo.Utility
{
    internal static class ChangeRDBHelper
    {
        internal static ChangeRDBBase Red => new ChangeRDB_Red();
        internal static ChangeRDBBase Green => new ChangeRDB_Green();
        internal static ChangeRDBBase Blue => new ChangeRDB_Blue();

        private class ChangeRDB_Red : ChangeRDBBase
        {
            protected override Color GetColor(byte value, Color currentColor)
                => Color.FromRgb(value, currentColor.G, currentColor.B);
        }

        private class ChangeRDB_Green : ChangeRDBBase
        {
            protected override Color GetColor(byte value, Color currentColor)
                => Color.FromRgb(currentColor.R, value, currentColor.B);
        }

        private class ChangeRDB_Blue : ChangeRDBBase
        {
            protected override Color GetColor(byte value, Color currentColor)
                => Color.FromRgb(currentColor.R, currentColor.G, value);
        }
    }

}


