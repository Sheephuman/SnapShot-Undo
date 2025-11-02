using System.ComponentModel;

namespace SnapShotUndo.ViewModel
{
    /// <summary>
    /// 単一値に対して Undo/Redo をサポートする ViewModel。
    /// WPF バインディングを想定。
    /// </summary>
    public class UndoableValueViewModel<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
