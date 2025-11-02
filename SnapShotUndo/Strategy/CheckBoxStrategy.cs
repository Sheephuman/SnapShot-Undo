using SnapShotUndo.Strategy.BaseStaretegt;
using System.Diagnostics;
using System.Windows.Controls;


namespace SnapShotUndo.Strategy
{
    internal class CheckBoxStrategy : SnapshotStrategyBase<IEnumerable<bool>>, ISnapshotStrategy
    {
        private readonly IEnumerable<CheckBox> _checkBoxes;



        public CheckBoxStrategy(IEnumerable<CheckBox> checkBoxes)
        {
            _checkBoxes = checkBoxes;

            // 初期スナップショット
            _undoStack = _undoStack.Push(GetCurrentState());
        }

        private List<bool> GetCurrentState()
        {
            var state = new List<bool>();
            foreach (var cb in _checkBoxes)
                state.Add(cb.IsChecked == true);
            return state;
        }

        public override void CaptureSnapshot()
        {
            var current = GetCurrentState();

            // 直前と同じなら無視（必要なら）
            if (_undoStack.Head.SequenceEqual(current))
                return;
            else
                _undoStack = _undoStack.Push(current);

            Debug.WriteLine(this.GetType().Name + _undoStack.Count);
            _redoStack.Clear();
        }



        public override void Undo() => base.UndoCore(values =>
        {
            foreach (var (checkBox, isChecked) in _checkBoxes.Zip(values))
                checkBox.IsChecked = isChecked;
        });

        public override void Redo() => base.RedoCore(values =>
        {
            foreach (var (checkBox, isChecked) in _checkBoxes.Zip(values))
                checkBox.IsChecked = isChecked;
        });

        public IEnumerable<bool> GetCurrentValue()
     => _checkBoxes.Select(c => c.IsChecked == true);

        public void Apply(IEnumerable<bool>? value)
        {
            if (value is null)
                return;
            foreach (var (cb, v) in _checkBoxes.Zip(value))
                cb.IsChecked = v;
            ///CheckBox の順番通りに状態を復元

            ////Enumerable.Zip は 2 つのシーケンスを同時に走査するメソッド。

            ///要素数が異なる場合は 短い方に合わせて自動で終了 します。

            //これにより「CheckBox の順番通りに状態を復元」できます。
        }
    }

}
