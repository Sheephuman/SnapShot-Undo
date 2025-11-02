using SnapShotUndo.Strategy.BaseStaretegt;
using System.Diagnostics;
using System.Windows.Controls;

namespace SnapShotUndo.Strategy
{
    internal class RadioGroupStrategy : SnapshotStrategyBase<string>
    {


        private readonly List<RadioButton> _radioButtons;


        internal string GetCurrentValue()
        {
            return _radioButtons.FirstOrDefault(r => r.IsChecked == true)?.Name ?? string.Empty;
        }

        public RadioGroupStrategy(List<RadioButton> buttons)
        {

            _radioButtons = buttons;
            _undoStack = _undoStack.Push(GetCheckedName());
        }

        private string GetCheckedName()
            => _radioButtons.FirstOrDefault(b => b.IsChecked == true)?.Name ?? string.Empty;

        public override void CaptureSnapshot()
        {
            var current = GetCheckedName();

            if (_undoStack.IsEmpty)
            {
                _undoStack = _undoStack.Push(current);
                _redoStack.Clear();
                return;
            }


            if (current == _undoStack.Head)
                return;
            else
                _undoStack = _undoStack.Push(current);


            Debug.WriteLine(this.GetType().Name + _undoStack.Count);
            _redoStack = new ConsCell<string>();
        }

        public override void Undo()
            => base.UndoCore(name => SetChecked(name));

        public override void Redo()
            => base.RedoCore(name => SetChecked(name));

        private void SetChecked(string name)
        {
            foreach (var btn in _radioButtons)
                btn.IsChecked = (btn.Name == name);
        }

        internal void Apply(string checkedName)
        {
            foreach (var radio in _radioButtons)
            {
                radio.IsChecked = (radio.Name == checkedName);
            }
        }
    }
}


