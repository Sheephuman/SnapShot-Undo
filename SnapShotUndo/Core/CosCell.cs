using System.Collections;

namespace SnapShotUndo
{
    public class ConsCell<T> : IEnumerable<T>
    {
        private readonly T? head;
        private readonly ConsCell<T>? tail;
        public readonly bool isTerminal;

        /// <summary>
        /// 空リスト（終端セル）を作成する
        /// </summary>
        public ConsCell()
        {
            this.isTerminal = true;
        }

        /// <summary>
        /// 値と次のセルを指定して新しい ConsCell を作成する
        /// </summary>
        public ConsCell(T value, ConsCell<T> tail)
        {
            this.head = value;
            this.tail = tail;
        }

        /// <summary>
        /// IEnumerable から ConsCell を構築する
        /// </summary>
        public ConsCell(IEnumerable<T> source)
            : this(EnsureNotNull(source).GetEnumerator())
        {
        }

        private static IEnumerable<T> EnsureNotNull(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return source;
        }

        private ConsCell(IEnumerator<T> itor)
        {
            if (itor.MoveNext())
            {
                this.head = itor.Current;
                this.tail = new ConsCell<T>(itor);
            }
            else
            {
                this.isTerminal = true;
            }
        }

        /// <summary>
        /// 空リストかどうかを判定
        /// </summary>
        public bool IsEmpty => this.isTerminal;

        /// <summary>先頭要素を安全に取得（空なら default）</summary>
        public ConsCell<T> SafeCell => isTerminal ? new ConsCell<T>() : this;

        public T TryGetHead(out T? value)
        {
            if (isTerminal)
            {
                value = new ConsCell<T>().head;
                if (value != null)
                    return value;
            }

            value = head;
            if (value != null)
                return value;

            return value!;
        }


        /// <summary>
        /// 先頭要素を取得（空リストなら例外）
        /// </summary>
        public T Head
        {
            get
            {
                ErrorIfEmpty();
                if (head is null)

                    throw new ArgumentNullException(nameof(head));
                return this.head;
            }
        }

        /// <summary>
        /// 残りのリストを取得（空リストなら例外）
        /// </summary>
        public ConsCell<T> Tail
        {
            get
            {
                ErrorIfEmpty();
                return this.tail!;
            }
        }
        /// <summary>
        /// 要素数を返す
        /// </summary>
        public int Count
        {
            get
            {
                int c = 0;
                for (ConsCell<T> p = this; !p.isTerminal; p = p.tail!)
                {
                    c++;
                }

                return c;
            }
            set
            {
                value = Count;

            }

        }


        private void ErrorIfEmpty()
        {
            if (this.isTerminal)
                //MessageBox.Show("this is empty.");
                throw new InvalidOperationException("this is empty.");
        }

        /// <summary>
        /// 新しい要素を先頭に追加し、新しい ConsCell を返す
        /// </summary>
        public ConsCell<T> Push(T head) => new ConsCell<T>(head, this);



        /// <summary>
        /// このリストの末尾に別のリストを連結する
        /// </summary>
        public ConsCell<T> Concat(ConsCell<T> second)
        {
            if (this.isTerminal || head is null)
                return second;
            return this.tail!.Concat(second).Push(this.head);
        }

        /// <summary>
        /// 要素を含むかどうか
        /// </summary>
        public bool Contains(T item)
        {
            for (ConsCell<T> p = this; !p.isTerminal; p = p.tail!)
            {
                if (p.head == null && item == null) return true;
                if (p.head != null && p.head.Equals(item)) return true;
            }
            return false;
        }

        internal ConsCell<T> Clear()
        {
            return new ConsCell<T>();
        }

        internal ConsCell<T> Next()
        {
            return isTerminal || tail == null
                ? new ConsCell<T>() // 終端の場合は空のセルを返す
                : tail;             // 次のセルを返す
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
