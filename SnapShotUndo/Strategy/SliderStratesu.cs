using SnapShotUndo.Strategy.BaseStaretegt;
using System.Diagnostics;
using System.Windows.Controls;

namespace SnapShotUndo.Strategy
{
    internal class SliderStrategy : SnapshotStrategyBase<IEnumerable<object>>
    {
        List<Slider> _Sliders;


        public SliderStrategy(List<Slider> sliders)
        {
            _Sliders = sliders;

            foreach (Slider slider in _Sliders)
                slider.Value = 255;


            // 初期スナップショット
            _undoStack = _undoStack.Push(GetCurrentState());

        }
        private IList<object> GetCurrentState()
        {
            var state = new List<object>();
            foreach (var cb in _Sliders)
                state.Add(cb.Value);
            return state;
        }

        public override void CaptureSnapshot()
        {

            var current = GetCurrentState().ToList();

            if (_undoStack.IsEmpty)
            {
                _undoStack = _undoStack.Push(current);
                _redoStack.Clear();
                return;
            }


            // 直前と同じなら無視
            if (_undoStack.Head.SequenceEqual(current))
                return;
            else
                _undoStack = _undoStack.Push(current);


            Debug.WriteLine(this.GetType().Name + _undoStack.Count);

            _redoStack.Clear();
        }

        public override void Redo() => base.RedoCore(values =>
        {
            foreach (var (tokenSlider, tokenValue) in _Sliders.Zip(values))
                tokenSlider.Value = (double)tokenValue;
        });

        public override void Undo() => base.UndoCore(values =>
        {
            foreach (var (tokenSlider, tokenValue) in _Sliders.Zip(values))
                tokenSlider.Value = (double)tokenValue;
        });

        internal IEnumerable<double> GetCurrentValue()
        {
            return _Sliders.Select(s => s.Value);
        }

        internal void Apply(IEnumerable<double> sliderValue)
        {
            if (sliderValue is null)
                return;


            ////すべての CheckBox の状態が value の最後の要素として反映される
            //foreach (var token in _Sliders)
            //    foreach(var coslide in sliderValue)
            //    token.Value = coslide;


            foreach (var (cb, value) in _Sliders.Zip(sliderValue))
                cb.Value = value;

        }
    }
}
