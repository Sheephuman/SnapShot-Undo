using SnapShotUndo.Model;
using SnapShotUndo.Strategy.BaseStaretegt;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;


namespace SnapShotUndo.Strategy
{
    /// <summary>
    /// DataGridにバインドされたObservableCollectionを対象にしたスナップショット戦略。
    /// Undo/Redo時にリスト全体の状態を復元する。
    /// </summary>

    /// <summary>
    /// DataGrid と ObservableCollection に対して Undo/Redo スナップショットを提供する戦略クラス。
    /// </summary>
    internal class DataGridSnapshotStrategy : SnapshotStrategyBase<ObservableCollection<Person>>, ISnapshotStrategy
    {
        private readonly DataGrid _target;
        private readonly ObservableCollection<Person> _source;

        public DataGridSnapshotStrategy(DataGrid target, ObservableCollection<Person> source)
        {
            _target = target;
            _source = source;
            _undoStack = _undoStack.Push(CloneCollection(_source));
        }



        private static ObservableCollection<Person> CloneCollection(ObservableCollection<Person> source)
            => new ObservableCollection<Person>(source.Select(p => (Person)p.Clone()));

        public override void Undo()
            => UndoCore(state => ReplaceCollection(_source, state));

        public override void Redo()
            => RedoCore(state => ReplaceCollection(_source, state));

        private static void ReplaceCollection(ObservableCollection<Person> target, ObservableCollection<Person> source)
        {
            target.Clear();
            foreach (var item in source)
                target.Add(item);
        }

        public override void CaptureSnapshot()
        {

            var current = CloneCollection(_source);

            // 中身が同じならスナップショットを積まない
            if (_undoStack.Head is not null && _undoStack.Head.SequenceEqual(current))
                return;
            else
                _undoStack = _undoStack.Push(current);
            Debug.WriteLine(this.GetType().Name + _undoStack.Count);
            _redoStack.Clear();
        }
        public ObservableCollection<Person> GetCurrentValue()
        {
            return new ObservableCollection<Person>(
                _source.Select(p => new Person
                {
                    Name = p.Name,
                    Age = p.Age,
                    City = p.City,
                })
            );
        }

        internal void Apply(ObservableCollection<Person> personValue)
        {
            if (personValue == null) return;

            // 既存コレクションをクリアして差し替える（バインディング維持）
            var current = _target.ItemsSource as ObservableCollection<Person>;
            if (current == null)
            {
                _target.ItemsSource = personValue;
                return;
            }

            current.Clear();
            foreach (var p in personValue)
                current.Add(p);
        }
    }

}
