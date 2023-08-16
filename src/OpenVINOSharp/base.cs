using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// OpenVINO wrapper for .NET. 
/// This is the basic namespace of OpenVINO in Cshrp, 
/// and all classes and methods are within this method.
/// </summary>
namespace OpenVINOSharp
{
    /// <summary>
    /// OpenVINO wrapper for .NET. 
    /// Define elements in OpenVINO.
    /// </summary>
    namespace element { }

    /// <summary>
    /// Mainly defined the data processing methods in OpenVINO.
    /// </summary>
    namespace preprocess { }

    /// <summary>
    /// Processing methods for main common models.
    /// </summary>
    namespace model {
        /// <summary>
        /// The processing methods of the main Yolov8 model.
        /// </summary>
        namespace Yolov8 { }
    }
}
