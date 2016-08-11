using System;
using System.Drawing;
using System.IO;
using System.Linq;

using TagFile = TagLib.File;
using TagMediaTypes = TagLib.MediaTypes;
using TagProperties = TagLib.Properties;
using ITagCodec = TagLib.ICodec;
using ITagVideoCodec = TagLib.IVideoCodec;

namespace NVP
{
    /// <summary>
    /// A video metadata collection
    /// </summary>
    public class Metadata
    {
        #region Properties

        const Int32 FRAMES_PER_SECONDS = 30;    // todo: should be dynamic

        /// <summary>
        /// Video's frame rate
        /// </summary>
        public Int32 FrameRate
        {
            get { return FRAMES_PER_SECONDS; }
        }

        /// <summary>
        /// Video frame dimensions
        /// </summary>
        public Size FrameSize
        {
            get; private set;
        }

        /// <summary>
        /// Video duration
        /// </summary>
        public TimeSpan Duration
        {
            get; private set;
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of a video metadata collection
        /// </summary>
        /// <param name="frameSize">The frame size of the video</param>
        internal Metadata(Size frameSize)
        {
            this.FrameSize = frameSize;
        }

        /// <summary>
        /// Creates a new instance of a video metadata collection from the given tags
        /// </summary>
        /// <param name="tag">Tags</param>
        internal Metadata(TagProperties tag)
        {
            this.FrameSize = new Size(tag.VideoWidth, tag.VideoHeight);
            this.Duration = tag.Duration;
        }

        #endregion

        #region Static

        /// <summary>
        /// Reads the metadata from the given video file path
        /// </summary>
        /// <param name="filepath">Video file path</param>
        /// <returns>The video's metadata</returns>
        public static Metadata FromFile(String filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException(ResourceMessages.FileNotFound, filepath);

            // Reading the file's taf
            TagFile tag = ReadTagsOrThrowException(filepath);

            // We currently only support mp4 + acc, so if we don't have audio + video, we throw
            if ((tag.Properties.MediaTypes & TagMediaTypes.Video) != TagMediaTypes.Video || ((tag.Properties.MediaTypes & TagMediaTypes.Audio) != TagMediaTypes.Audio))
                throw new UnsupportedFileFormatException();

            return new Metadata(tag.Properties);
        }

        /// <summary>
        /// Reads the file tags or throws an exception if it can't
        /// </summary>
        /// <param name="filepath">Video file path to read the tags from</param>
        /// <returns>TagFile</returns>
        private static TagFile ReadTagsOrThrowException(String filepath)
        {
            try
            {
                TagFile infos = TagFile.Create(filepath);
                if (infos != null)
                    return infos;

            }
            catch (Exception ex)
            {
                throw new UnsupportedFileFormatException(ex);
            }

            throw new UnsupportedFileFormatException();
        }

        #endregion
    }
}
