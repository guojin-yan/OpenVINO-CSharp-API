using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ov_dimension = OpenVinoSharp.Ov.ov_dimension;

namespace OpenVinoSharp
{
    /// <summary>
    ///  Class representing a dimension, which may be dynamic (undetermined until runtime),
    ///  in a shape or shape-like object.
    /// </summary>
    /// <remarks>Static dimensions may be implicitly converted from value_type. 
    /// A dynamic dimension is constructed with Dimension() or Dimension::dynamic().</remarks>
    public class Dimension
    {
        /// <summary>
        /// The ov_dimension struct.
        /// </summary>
        ov_dimension m_dimension;
        /// <summary>
        ///  Construct a static dimension.
        /// </summary>
        /// <param name="dimension">Value of the dimension.</param>
        public Dimension(long dimension)
        {
            m_dimension.min = dimension;
            m_dimension.max = dimension;
        }

        /// <summary>
        /// Construct a dynamic dimension with bounded range
        /// </summary>
        /// <param name="min_dimension">The lower inclusive limit for the dimension</param>
        /// <param name="max_dimension">The upper inclusive limit for the dimension</param>
        public Dimension(long min_dimension, long max_dimension) 
        {
            m_dimension.min = min_dimension;
            m_dimension.max = max_dimension;
        }
        /// <summary>
        /// Get ov_dimension struct.
        /// </summary>
        /// <returns>Return ov_dimension struct.</returns>
        ov_dimension get_dimension() 
        {
            return m_dimension;
        }
    }
}
