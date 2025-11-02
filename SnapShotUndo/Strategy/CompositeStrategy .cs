using SnapShotUndo.Model;
using SnapShotUndo.Strategy.BaseStaretegt;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SnapShotUndo.Strategy
{
    internal class CompositeStrategy : SnapshotStrategyBase<CompositeSnapshot>
    {
        private readonly TextBoxSnapshotStrategy _textBoxStrategy;
        private readonly RadioGroupStrategy _radioStrategy;
        private readonly CheckBoxStrategy _checkBoxStrategy;
        private readonly SliderStrategy _sliderStrategy;
        private readonly DataGridSnapshotStrategy _dataGridStrategy;


        public CompositeStrategy(
          TextBoxSnapshotStrategy textBox,
          RadioGroupStrategy radio,
          CheckBoxStrategy check,
          SliderStrategy slider,
          DataGridSnapshotStrategy dataGrid)
        {
            _textBoxStrategy = textBox;
            _radioStrategy = radio;
            _checkBoxStrategy = check;
            _sliderStrategy = slider;
            _dataGridStrategy = dataGrid;

            // 初期スナップショット登録
            _undoStack = _undoStack.Push(CreateSnapshot());
        }


        public override void CaptureSnapshot()
        {
            var snapshot = CreateSnapshot();
            if (_undoStack.IsEmpty || !snapshot.Equals(_undoStack.Head))
            {
                _undoStack = _undoStack.Push(snapshot);

                Debug.WriteLine(this.GetType().Name + _undoStack.Count);

                _redoStack.Clear();
            }
        }

        private CompositeSnapshot CreateSnapshot() => new CompositeSnapshot
        {
            TextBoxValue = _textBoxStrategy.GetCurrentValue(),
            RadioNames = _radioStrategy.GetCurrentValue(),
            CheckBoxValue = _checkBoxStrategy.GetCurrentValue().ToList(),
            SliderValue = _sliderStrategy.GetCurrentValue().ToList(),
            PersonValue = new ObservableCollection<Person>(_dataGridStrategy.GetCurrentValue())
        };

        public override void Undo() => base.UndoCore(ApplySnapshot);
        public override void Redo() => base.RedoCore(ApplySnapshot);

        private void ApplySnapshot(CompositeSnapshot snapshot)
        {
            _textBoxStrategy.Apply(snapshot.TextBoxValue);
            _radioStrategy.Apply(snapshot.RadioNames);
            _checkBoxStrategy.Apply(snapshot.CheckBoxValue);
            _sliderStrategy.Apply(snapshot.SliderValue);
            _dataGridStrategy.Apply(snapshot.PersonValue);
        }
    }

}
