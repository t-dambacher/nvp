using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVP
{
    /// <summary>
    /// Exception thrown when a file type is not supported or recognized
    /// </summary>
    public class UnsupportedFileFormatException : ApplicationException
    {
        /// <summary>
        /// Creates a new instance of a UnsupportedFileFormationException
        /// </summary>
        public UnsupportedFileFormatException()
            : this(ResourceMessages.SelectedFileIsNotValid)
        { }

        /// <summary>
        /// Creates a new instance of a UnsupportedFileFormationException with the given error message
        /// </summary>
        /// <param name="message">Error message</param>
        public UnsupportedFileFormatException(String message)
            : base(message)
        { }

        /// <summary>
        /// Creates a new instance of a UnsupportedFileFormationException
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        public UnsupportedFileFormatException(Exception innerException)
            : base(ResourceMessages.UnsupportedVideoFormat, innerException)
        { }

        /// <summary>
        /// Creates a new instance of a UnsupportedFileFormationException with the given error message
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public UnsupportedFileFormatException(String message, Exception innerException)
            : base(message, innerException)
        { }

        // todo: add serialization ctor
    }
}
