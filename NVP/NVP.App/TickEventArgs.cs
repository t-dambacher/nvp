using System;

namespace NVP.App
{
    /// <summary>
    /// EventArgs used on a watch's tick
    /// </summary>
    internal class TickEventArgs : EventArgs
    {
        /// <summary>
        /// Requested frame rate
        /// </summary>
        public Int32 RequestedFrameRate { get; private set; }

        /// <summary>
        /// Actual frame rate
        /// </summary>
        public Int32 ActualFrameRate { get; private set; }

        /// <summary>
        /// Constructs a new instance of a TickEventArgs
        /// </summary>
        /// <param name="requestedFrameRate">Requested frame rate</param>
        /// <param name="actualFrameRate">Actual frame rate</param>
        public TickEventArgs(Int32 requestedFrameRate, Int32 actualFrameRate)
        {
            this.ActualFrameRate = actualFrameRate;
            this.RequestedFrameRate = requestedFrameRate;
        }
    }
}
