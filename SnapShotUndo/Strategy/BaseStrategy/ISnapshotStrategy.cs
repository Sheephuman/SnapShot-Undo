namespace SnapShotUndo.Strategy.BaseStaretegt
{

    public interface ISnapshotStrategy
    {

        public abstract void CaptureSnapshot();

        public virtual void Undo() { }
        public virtual void Redo() { }


        bool CanUndo { get; }

        bool CanRedo { get; }

    }

}

