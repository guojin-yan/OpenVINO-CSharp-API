using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// The default exception to be thrown by OpenVINO
    /// </summary>
    [Serializable]
    // ReSharper disable once InconsistentNaming
    internal class OVException : Exception
    {
        /// <summary>
        /// The numeric code for error status
        /// </summary>
        public ExceptionStatus status { get; set; }


        /// <summary>
        /// A description of the error
        /// </summary>
        public string err_msg { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="status">The numeric code for error status</param>
        /// <param name="func_name">The source file name where error is encountered</param>
        /// <param name="err_msg">A description of the error</param>
        /// <param name="file_name">The source file name where error is encountered</param>
        /// <param name="line">The line number in the source where error is encountered</param>
        public OVException(ExceptionStatus status, string err_msg)
            : base(err_msg)
        {
            this.status = status;
            this.err_msg = err_msg;
        }

    }
}
