using System;
using System.Diagnostics;

namespace NVP.App
{
    /// <summary>
    /// Watch used to refresh periodicaly the screen
    /// </summary>
    internal sealed class Watch : IDisposable
    {
        #region Properties

        /// <summary>
        /// The timer used to draw frames à 25fps
        /// </summary>
        private readonly Timer _frameTimer;

        /// <summary>
        /// Stopwatch used to calculate frame rate
        /// </summary>
        private readonly Stopwatch _fpsTimer;

        /// <summary>
        /// Requested frame rate
        /// </summary>
        private readonly Int32 _requestedFrameRate;

        /// <summary>
        /// Current number of already drawn frames
        /// </summary>
        private Int32 _currentFramesCount;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of a Watch 
        /// </summary>
        /// <param name="requestedFrameRate">Frame rate requested that the watch should achieve</param>
        public Watch(Int32 requestedFrameRate)
        {
            this._requestedFrameRate = requestedFrameRate;
            this._currentFramesCount = 0;
            this._frameTimer = InitializeFrameTimer(requestedFrameRate);
            this._frameTimer.Start();
            this._fpsTimer = Stopwatch.StartNew();
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised on each tick
        /// </summary>
        public event EventHandler<TickEventArgs> Tick;

        #endregion

        #region Methods

        /// <summary>
        /// Creates and configures a new Timer instance, used to draw frames
        /// </summary>
        private Timer InitializeFrameTimer(Int32 frameRate)
        {
            Int32 msInterval = 1000 / frameRate;

            Timer res = new Timer(msInterval);
            res.Tick += OnTimer_Tick;

            return res;
        }

        /// <summary>
        /// EventHandler called on each timer's tick
        /// </summary>
        private void OnTimer_Tick(Object sender, EventArgs e)
        {
            Double elapsedPerFrame = (1.0D * this._fpsTimer.ElapsedMilliseconds) / this._currentFramesCount;
            Int32 currentRate = (Int32)(1000D / elapsedPerFrame);

            if (this._currentFramesCount == this._requestedFrameRate)
            {
                this._currentFramesCount = 0;
                this._fpsTimer.Restart();
            }
            else
                this._fpsTimer.Start();

            RaiseTickEvent(currentRate);    // performs the FrameEnumerator.MoveNext();

            this._currentFramesCount++;
        }

        /// <summary>
        /// Raise the watch's tick event
        /// </summary>
        /// <param name="currentFrameRate">Current frame rate</param>
        private void RaiseTickEvent(Int32 currentFrameRate)
        {
            EventHandler<TickEventArgs> handler = this.Tick;
            if (handler != null)
            {
                handler(this, new TickEventArgs(this._requestedFrameRate, currentFrameRate));
            }
        }

        #region IDisposable implementation

        private Boolean disposedValue = false; // To detect redundant calls

        private void Dispose(Boolean disposing)
        {
            if (!disposedValue)
            {
                this._frameTimer.Dispose();
                disposedValue = true;
            }
        }

        ~Watch()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #endregion
    }
}
