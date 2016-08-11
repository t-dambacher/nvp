using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace NVP
{
    /// <summary>
    /// Video player base class
    /// </summary>
    public class VideoDecoder : IEnumerable<Frame>, IDisposable
    {
        #region Properties

        /// <summary>
        /// Videos metadata
        /// </summary>
        public Metadata Metadata
        {
            get; private set;
        }

        /// <summary>
        /// Enumerator to the video's frames
        /// </summary>
        public IEnumerator<Frame> Enumerator
        {
            get { return _enumerator; }
        }
        private readonly IEnumerator<Frame> _enumerator;

        /// <summary>
        /// True if the instance has been disposed
        /// </summary>
        private Boolean _isDisposed;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of a VideoPlayer
        /// </summary>
        /// <param name="metadata">Metadata informations taken from the video header</param>
        private VideoDecoder(Metadata metadata)
        {
            this.Metadata = metadata;
            this._enumerator = new FrameEnumerator(metadata);
        }

        #endregion

        #region IDisposable implementation

        /// <summary>
        /// Dispose the class's resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class's resources
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;

            if (disposing)
            {
                // disposing managed resources
                this.Enumerator.Dispose();
            }

            this._isDisposed = true;
        }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Returns an enumerator to the video's frames
        /// </summary>
        /// <returns>Enumerator to the video's frames</returns>
        public IEnumerator<Frame> GetEnumerator()
        {
            return this.Enumerator;
        }

        /// <summary>
        /// Returns an enumerator to the video's frames
        /// </summary>
        /// <returns>Enumerator to the video's frames</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Creates a new video player able to play the given file 
        /// </summary>
        /// <param name="filepath">Full path to the video file</param>
        /// <returns>A video player able to play the given file</returns>
        public static VideoDecoder FromFile(String filepath)
        {
            Metadata metadata = Metadata.FromFile(filepath);
            if (metadata == null)
                throw new UnsupportedFileFormatException(ResourceMessages.SelectedFileIsNotValid);

            return new VideoDecoder(metadata);
        }
    }
}
