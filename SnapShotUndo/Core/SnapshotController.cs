using SnapShotUndo.Strategy.BaseStaretegt;

namespace SnapShotUndo.Core
{
    /// <summary>
    /// ISnapshotStrategy を使い、スナップショットの操作を統一的に管理するコンテキストクラス。
    /// Strategy Pattern の「Context」に相当する。
    /// </summary>
    public class SnapshotController
    {

        private ISnapshotStrategy? _strategy;


        private List<ISnapshotStrategy> _stratesyList = new();


        /// <summary>
        /// 利用する戦略を設定する。
        /// TextBox用やDataGrid用など、状況に応じて差し替え可能。
        /// </summary>
        public void SetStrategy(ISnapshotStrategy strategy)
        {
            _stratesyList = new List<ISnapshotStrategy> { strategy };
        }

        public void SetStrategy(List<ISnapshotStrategy> strategyList)
        {
            _stratesyList.Clear();
            _stratesyList.AddRange(strategyList);
        }

        /// <summary>
        /// 現在の状態をスナップショットとして保存。
        /// </summary>
        public void IndividualCapture()
        {
            foreach (var s in _stratesyList)
                s.CaptureSnapshot();
        }



        /// <summary>
        /// Undo操作を実行。
        /// </summary>
        public void Undo()
        {
            foreach (var s in _stratesyList)
                s.Undo();
        }

        /// <summary>
        /// Redo操作を実行。
        /// </summary>
        public void Redo()
        {
            foreach (var s in _stratesyList)
                s.Redo();
        }

        internal void CompositeCapture()
        {

            foreach (var s in _stratesyList)
                s.CaptureSnapshot();
        }
    }
}