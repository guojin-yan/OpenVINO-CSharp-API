﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        /// It represents a shape that may be partially or totally dynamic.
        /// </summary>
        /// <remarks>
        /// <para>Dynamic rank. (Informal notation: `?`)</para>
        /// <para>Static rank, but dynamic dimensions on some or all axes.
        /// (Informal notation examples: `{1,2,?,4}`, `{?,?,?}`)</para>
        /// <para> Static rank, and static dimensions on all axes.
        /// (Informal notation examples: `{1,2,3,4}`, `{6}`, `{}`)</para>
        /// </remarks>
        public struct ov_partial_shape
        {
            /// <summary>
            /// The rank
            /// </summary>
            public NativeMethods.ov_rank rank;

#if NET7_0_OR_GREATER || NET6_0_OR_GREATER
            /// <summary>
            /// The dimension
            /// </summary>
            public IntPtr dims = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.ov_dimension)));
            /// <summary>
            /// Default Constructor
            /// </summary>
            public ov_partial_shape()
            {
                rank = new NativeMethods.ov_rank();
                dims = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.ov_dimension)));
            }
#else
            public IntPtr dims;
#endif
        }

        /// <summary>
        /// [private]Core class pointer.
        /// </summary>
        private IntPtr m_ptr = IntPtr.Zero;
        /// <summary>
        /// [public]Core class pointer.
        /// </summary>
        public IntPtr Ptr { get { return m_ptr; } set { m_ptr = value; } }

        public ov_partial_shape shape;

        public PartialShape(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("Shape init error : ptr is null!");
                return;
            }
            this.m_ptr = ptr;
            var temp = Marshal.PtrToStructure(ptr, typeof(ov_partial_shape));
            shape = (ov_partial_shape)temp;
            //~~~~~~~~~~~~~~~~~~
        }
    }
}