using SnapShotUndo.Strategy.BaseStaretegt;
using System.Diagnostics;
using System.Windows.Controls;


namespace SnapShotUndo.Strategy
{
    public class TextBoxSnapshotStrategy : SnapshotStrategyBase<string>
    {
        private readonly TextBox _target;


        public string GetCurrentValue()
        => _target.Text;


        public TextBoxSnapshotStrategy(TextBox target)
        {
            _target = target;

            _undoStack = _undoStack.Push(target.Text);
        }

        public void Apply(string text)
        {
            _target.Text = text;
        }

        public override void CaptureSnapshot()
        {
            var current = _target.Text;

            if (_undoStack.IsEmpty)
            {
                _undoStack = _undoStack.Push(current);
                _redoStack.Clear();
                return;
            }


            if (current == _undoStack.Head)
                return;
            else
                // 差分キャプチャ: 「前の状態」をUndoに積む
                _undoStack = _undoStack.Push(current);

            Debug.WriteLine(this.GetType().Name + _undoStack.Count);

            _redoStack.Clear();
        }

        public override void Undo() => base.UndoCore(v => _target.Text = v);
        public override void Redo() => base.RedoCore(v => _target.Text = v);
    }


}
