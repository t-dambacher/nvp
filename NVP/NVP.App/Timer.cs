using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NVP.App
{
    /// <summary>
    /// Relatively precise timer (more than the one in System.Windows.Forms.Timer)
    /// </summary>
    public sealed class Timer : IDisposable
    {
        #region Properties

        /// <summary>
        /// Interval between two ticks, in milliseconds
        /// </summary>
        private readonly Int32 _interval;

        /// <summary>
        /// Handler called on the system timer's tick
        /// </summary>
        private readonly TimerEventHandler _handler;

        /// <summary>
        /// Timer ID
        /// </summary>
        private Int32 _timerId;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of the Timer
        /// </summary>
        /// <param name="intervalMS">Interval between two ticks, in milliseconds</param>
        public Timer(Int32 intervalMS)
        {
            this._interval = intervalMS;
            this._handler = new TimerEventHandler(Handler);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised on a timer's tick
        /// </summary>
        public event EventHandler Tick;

        #endregion

        #region Methods

        /// <summary>
        /// EventHandler raised on a system timer's tick
        /// </summary>
        private void Handler(Int32 id, Int32 msg, IntPtr user, Int32 dw1, Int32 dw2)
        {
            RaiseTickEvent();
        }

        /// <summary>
        /// Raises the Tick event
        /// </summary>
        private void RaiseTickEvent()
        {
            EventHandler handler = this.Tick;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            timeBeginPeriod(1);
            this._timerId = timeSetEvent(this._interval, 0, this._handler, IntPtr.Zero, EVENT_TYPE);
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            if (this._timerId != 0)
            {
                Int32 error = timeKillEvent(this._timerId);
                timeEndPeriod(1);
                this._timerId = 0;
            }
        }

        #endregion

        #region IDisposable implementation

        private Boolean disposedValue = false; // To detect redundant calls

        void Dispose(Boolean disposing)
        {
            if (!disposedValue)
            {
                this.Stop();
                disposedValue = true;
            }
        }

        ~Timer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region P/Invoke declarations

        private delegate void TimerEventHandler(Int32 id, Int32 msg, IntPtr user, Int32 dw1, Int32 dw2);

        private const Int32 TIME_PERIODIC = 1;
        private const Int32 EVENT_TYPE = TIME_PERIODIC;// + 0x100;  // TIME_KILL_SYNCHRONOUS causes a hang ?!

        [DllImport("winmm.dll")]
        private static extern Int32 timeSetEvent(Int32 delay, Int32 resolution, TimerEventHandler handler, IntPtr user, Int32 eventType);

        [DllImport("winmm.dll")]
        private static extern Int32 timeKillEvent(Int32 id);

        [DllImport("winmm.dll")]
        private static extern Int32 timeBeginPeriod(Int32 msec);

        [DllImport("winmm.dll")]
        private static extern Int32 timeEndPeriod(Int32 msec);

        #endregion
    }
}
