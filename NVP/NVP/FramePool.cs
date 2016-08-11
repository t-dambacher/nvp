using System;
using System.Collections.Generic;
using System.Drawing;

namespace NVP
{
    internal sealed class FramePool : IDisposable
    {
        private readonly List<Frame> _inUse;

        private readonly Stack<Frame> _pool;

        public FramePool(Size frameSize, Int32 capacity)
        {
            if (capacity < 0)
                throw new ArgumentException(nameof(capacity));

            this._pool = new Stack<Frame>(capacity);
            for (Int32 i = 0; i < capacity; ++i)
                this._pool.Push(new Frame(frameSize));

            this._inUse = new List<Frame>();
        }

        public Frame Get()
        {
            if (this._pool.Count == 0)
                throw new InvalidOperationException(ResourceMessages.NoMoreSpaceInPool); // todo: dynamic allocation if needed, up to a certain limit

            Frame res = this._pool.Pop();
            this._inUse.Add(res);

            return res;
        }

        public void Restore(Frame t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            this._inUse.Remove(t);
            this._pool.Push(t);
        }

        #region IDisposable

        private Boolean _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;

            if (disposing)
            {
                // disposing managed resources
                for (Int32 i = this._inUse.Count - 1; i >= 0; --i)
                    Restore(this._inUse[i]);

                for (Int32 i = 0; i < this._pool.Count; ++i)
                {
                    Frame t = this._pool.Pop();
                    t.Dispose();
                }
            }

            this._isDisposed = true;
        }

        #endregion
    }
}
