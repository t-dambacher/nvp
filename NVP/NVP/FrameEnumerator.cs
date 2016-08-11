using System;
using System.Collections;
using System.Collections.Generic;

namespace NVP
{
    /// <summary>
    /// Enumerator to enumerate a video's frames
    /// </summary>
    internal class FrameEnumerator : IEnumerator<Frame>
    {
        #region Properties

        /// <summary>
        /// True if the instance has been disposed
        /// </summary>
        private Boolean _isDisposed;

        /// <summary>
        /// Current background color ; todo : test only
        /// </summary>
        private Int32 _currentColor;

        /// <summary>
        /// Frame pool
        /// </summary>
        private readonly FramePool _pool;

        /// <summary>
        /// Current frame
        /// </summary>
        private Frame _current;

        #endregion

        #region IEnumerator implementation

        /// <summary>
        /// Get the current frame
        /// </summary>
        public Frame Current
        {
            get { return _current; }
        }

        /// <summary>
        /// Get the current frame
        /// </summary>
        Object IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Move the enumerator to the next item
        /// </summary>
        /// <returns>true if the enumerator could move to the next item, false otherwise</returns>
        public Boolean MoveNext()
        {
            if (this._current != null)
            {
                this._pool.Restore(this._current);
                this._current = null;
            }

            Byte backcolor = (Byte)this._currentColor;
            Frame res = this._pool.Get();

            res.SetColor(backcolor);
            this._currentColor = (this._currentColor + 1) % 256;

            this._current = res;

            // todo: handle EoF
            return true;
        }

        /// <summary>
        /// Reset the enumerator to its initial position
        /// </summary>
        public void Reset()
        {
            // todo ; then add fast forward / backward capabilities
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Dispose the enumerator's resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the enumerator's resources
        /// </summary>
        private void Dispose(Boolean disposing)
        {
            if (this._isDisposed)
                return;

            if (disposing)
            {
                // disposing managed resources
                this._pool.Dispose();
            }

            this._isDisposed = true;
        }

        #endregion

        #region Ctor

        ///<summary>
        /// Creates a new instance of a FrameEnumerator
        /// </summary>
        public FrameEnumerator(Metadata metadata)
        {
            this._pool = new FramePool(metadata.FrameSize, 16);
            this._currentColor = 0;
        }

        #endregion
    }
}
