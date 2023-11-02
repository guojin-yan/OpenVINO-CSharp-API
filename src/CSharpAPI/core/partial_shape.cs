using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ov_partial_shape = OpenVinoSharp.Ov.ov_partial_shape;

namespace OpenVinoSharp
{
    /// <summary>
    /// Class representing a shape that may be partially or totally dynamic.
    /// </summary>
    /// <remarks>
    /// <para>Dynamic rank. (Informal notation: `?`)</para>
    /// <para>Static rank, but dynamic dimensions on some or all axes.
    /// (Informal notation examples: `{1,2,?,4}`, `{?,?,?}`)</para>
    /// <para> Static rank, and static dimensions on all axes.
    /// (Informal notation examples: `{1,2,3,4}`, `{6}`, `{}`)</para>
    /// </remarks>
    public class PartialShape
    {
        /// <summary>
        /// PartialShape rank.
        /// </summary>
        private Dimension rank;
        
        /// <summary>
        /// PartialShape dimensions.
        /// </summary>
        private Dimension[] dimensions;
        /// <summary>
        /// Constructing partial shape by ov_partial_shape.
        /// </summary>
        /// <param name="shape">ov_partial_shape struct.</param>
        public PartialShape(Ov.ov_partial_shape shape) {
            partial_shape_convert(shape);
        }
        /// <summary>
        /// Constructing partial shape by dimensions.
        /// </summary>
        /// <param name="dimensions">The partial shape dimensions array.</param>
        public PartialShape(Dimension[] dimensions) 
        { 
            this.dimensions = dimensions;
            rank = new Dimension(dimensions.Length, dimensions.Length);
        }
        /// <summary>
        /// Constructing partial shape by dimensions.
        /// </summary>
        /// <param name="dimensions">The partial shape dimensions list.</param>
        public PartialShape(List<Dimension> dimensions) : this(dimensions.ToArray())
        { 
        }

        /// <summary>
        /// Constructing dynamic partial shape by dimensions.
        /// </summary>
        /// <param name="rank">The partial shape rank.</param>
        /// <param name="dimensions">The partial shape dimensions array.</param>
        public PartialShape(Dimension rank, Dimension[] dimensions)
        {
            this.dimensions = dimensions;
            this.rank = rank;
        }

        /// <summary>
        /// Constructing dynamic partial shape by dimensions.
        /// </summary>
        /// <param name="rank">The partial shape rank.</param>
        /// <param name="dimensions">The partial shape dimensions list.</param>
        public PartialShape(Dimension rank, List<Dimension> dimensions) : this(rank, dimensions.ToArray())
        {
        }
        /// <summary>
        /// Constructing static partial shape by dimensions.
        /// </summary>
        /// <param name="rank">The partial shape rank.</param>
        /// <param name="dimensions">The partial shape dimensions array.</param>
        public PartialShape(long rank, long[] dimensions)
        {
            this.rank = new Dimension(rank);
            for (int i = 0; i < dimensions.Length; ++i)
            {
                this.dimensions[i] = new Dimension(dimensions[i]);
            }
        }
        /// <summary>
        /// Constructing static partial shape by dimensions.
        /// </summary>
        /// <param name="rank">The partial shape rank.</param>
        /// <param name="dimensions">The partial shape dimensions list.</param>
        public PartialShape(long rank, List<long> dimensions) : this(rank, dimensions.ToArray())
        {
        }

        /// <summary>
        /// Constructing static partial shape by shape.
        /// </summary>
        /// <param name="shape">The shape</param>
        public PartialShape(Shape shape) 
        {
            Ov.ov_partial_shape partial_shape = new ov_partial_shape();
            HandleException.handler(
                NativeMethods.ov_shape_to_partial_shape(shape.shape, ref partial_shape));
            partial_shape_convert(partial_shape);
        }

        /// <summary>
        /// Default deconstruction.
        /// </summary>
        ~PartialShape()
        {
        }

        /// <summary>
        /// Convert partial shape to PartialShape class.
        /// </summary>
        /// <param name="shape">ov_partial_shape struct</param>
        private void partial_shape_convert(Ov.ov_partial_shape shape)
        {
            rank = new Dimension(shape.rank);
            long[] data = new long[rank.get_max() * 2];
            dimensions = new Dimension[rank.get_max()];
            Marshal.Copy(shape.dims, data, 0, (int)rank.get_max() * 2);
            for (int i = 0; i < rank.get_max(); ++i)
            {
                dimensions[i] = new Dimension(data[2 * i], data[2 * i + 1]);
            }
        }
        /// <summary>
        /// Get ov_partial_shape
        /// </summary>
        /// <returns>return ov_partial_shape.</returns>
        public ov_partial_shape get_partial_shape() 
        {
            Ov.ov_partial_shape shape_arr = new Ov.ov_partial_shape();
            shape_arr.rank = rank.get_dimension();
            List<Ov.ov_dimension> ov_dims = new List<Ov.ov_dimension>();
            for (int i = 0; i < shape_arr.rank.max; ++i)
            {
                ov_dims.Add(dimensions[i].get_dimension());
            }
            Ov.ov_dimension[] ds = ov_dims.ToArray();
            shape_arr.dims = Marshal.UnsafeAddrOfPinnedArrayElement(ds, 0);
            return shape_arr;
        }
        /// <summary>
        /// Get rank.
        /// </summary>
        /// <returns></returns>
        public Dimension get_rank()
        {
            return rank;
        }
        /// <summary>
        /// Get dimensions.
        /// </summary>
        /// <returns>Dimension[</returns>
        public Dimension[] get_dimensions() { 
            return dimensions;
        }

        /// <summary>
        /// Convert partial shape without dynamic data to a static shape.
        /// </summary>
        /// <returns>The shape.</returns>
        public Shape to_shape()
        {
            IntPtr shape_ptr = IntPtr.Zero;
            HandleException.handler(
                NativeMethods.ov_partial_shape_to_shape(get_partial_shape(), shape_ptr));     
            return new Shape(shape_ptr);
        }

        /// <summary>
        /// Check if this shape is static.
        /// </summary>
        /// <remarks>A shape is considered static if it has static rank, and all dimensions of the shape
        /// are static.</remarks>
        /// <returns>`true` if this shape is static, else `false`.</returns>
        public bool is_static() {
            return !is_dynamic();
        }

        /// <summary>
        /// Check if this shape is dynamic.
        /// </summary>
        /// <remarks>A shape is considered static if it has static rank, and all dimensions of the shape
        /// are static.</remarks>
        /// <returns>`false` if this shape is static, else `true`.</returns>
        public bool is_dynamic() {
            return rank.is_dynamic();
        }

        /// <summary>
        /// Get partial shape string.
        /// </summary>
        /// <returns></returns>
        public string to_string() 
        {
            string s = "Shape : {";
            if (rank.is_dynamic())
            {
                s += "?";
            }
            else 
            {
                for (int i = 0; i < rank.get_max(); ++i) 
                {
                    if (dimensions[i].is_dynamic())
                    {
                        s += "?,";
                    }
                    else
                    {
                        s += dimensions[i].get_dimension().max.ToString() + ",";
                    }
                }
            }
            s = s.Substring(0, s.Length - 1);
            s += "}";
            return s;
        }
    }
}
