namespace SnapShotUndo.Strategy.BaseStaretegt
{
    public static class CountManage
    {
        public static int UndoCount { get; private set; }

        public static void StackCounter<T>(ConsCell<T> undoStack)
        {
            UndoCount = undoStack.Count;
        }
    }


    public abstract class SnapshotStrategyBase<T> : ISnapshotStrategy
    {
        protected ConsCell<T> _undoStack = new();
        protected ConsCell<T> _redoStack = new();



        public virtual bool CanUndo => !_undoStack.IsEmpty;
        public virtual bool CanRedo => !_redoStack.IsEmpty;

        // 共通のUndoロジック
        protected virtual void UndoCore(Action<T> apply)
        {
            if (CanUndo)
            {
                var current = _undoStack.Head;
                _undoStack = _undoStack.Tail;
                _redoStack = _redoStack.Push(current);
                if (!_undoStack.IsEmpty)
                    apply(_undoStack.Head);
            }


            CountManage.StackCounter(_undoStack);
        }

        // 共通のRedoロジック
        protected virtual void RedoCore(Action<T> apply)
        {
            if (CanRedo)
            {
                var redoValue = _redoStack.Head;
                _redoStack = _redoStack.Tail;
                _undoStack = _undoStack.Push(redoValue);
                apply(redoValue);
            }

        }

        // 派生クラスで具体化
        public abstract void CaptureSnapshot();

        public abstract void Undo();
        public abstract void Redo();


    }


}
