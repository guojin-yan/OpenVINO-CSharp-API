using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{

    public partial class NativeMethods
    {
        /// <summary>
        /// Initialze a partial shape with static rank and dynamic dimension.
        /// </summary>
        /// <param name="rank">support static rank.</param>
        /// <param name="dims">support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The pointer of partial shape</param>
        /// <remarks>
        /// Static rank, but dynamic dimensions on some or all axes.
        ///     Examples: `{1,2,?,4}` or `{?,?,?}` or `{1,2,-1,4}`
        /// Static rank, and static dimensions on all axes.
        ///     Examples: `{ 1,2,3,4}` or `{6}` or `{}`</remarks>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_create",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create(
            long rank, 
            ref Ov.ov_dimension dims,
            out Ov.ov_partial_shape partial_shape_obj);

        /// <summary>
        /// Initialze a partial shape with static rank and dynamic dimension.
        /// </summary>
        /// <param name="rank">support dynamic and static rank.</param>
        /// <param name="dims">support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The pointer of partial shape</param>
        /// <remarks>
        /// Dynamic rank:
        ///    Example: `?`
        /// Static rank, but dynamic dimensions on some or all axes.
        ///     Examples: `{1,2,?,4}` or `{?,?,?}` or `{1,2,-1,4}`
        /// Static rank, and static dimensions on all axes.
        ///     Examples: `{ 1,2,3,4}` or `{6}` or `{}`</remarks>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_create_dynamic",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create_dynamic(
            Ov.ov_dimension rank,
            ref Ov.ov_dimension dims,
            out Ov.ov_partial_shape partial_shape_obj);

        /// <summary>
        /// Initialize a partial shape with static rank and static dimension.
        /// </summary>
        /// <param name="rank">support dynamic and static rank.</param>
        /// <param name="dims">support dynamic and static dimension.</param>
        /// <param name="partial_shape_obj">The pointer of partial shape</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_create_static",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_create_static(
            long rank, 
            ref long dims,
            out Ov.ov_partial_shape partial_shape_obj);

        /// <summary>
        /// Release internal memory allocated in partial shape.
        /// </summary>
        /// <param name="partial_shape">The object's internal memory will be released.</param>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_free",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static void ov_partial_shape_free(ref Ov.ov_partial_shape partial_shape);

        /// <summary>
        /// Convert partial shape without dynamic data to a static shape.
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <param name="shape">The shape pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_to_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_partial_shape_to_shape(
            Ov.ov_partial_shape partial_shape, 
            IntPtr shape);

        /// <summary>
        /// Convert shape to partial shape.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_shape_to_partial_shape",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static ExceptionStatus ov_shape_to_partial_shape(
            Ov.ov_shape shape,
            ref Ov.ov_partial_shape partial_shape);

        /// <summary>
        /// Check this partial_shape whether is dynamic
        /// </summary>
        /// <param name="partial_shape">The partial_shape.</param>
        /// <returns>Status code of the operation: OK(0) for success.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_is_dynamic",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static bool ov_partial_shape_is_dynamic(Ov.ov_partial_shape partial_shape);

        /// <summary>
        /// Helper function, convert a partial shape to readable string.
        /// </summary>
        /// <param name="partial_shape">The partial_shape pointer.</param>
        /// <returns>A string reprensts partial_shape's content.</returns>
        [DllImport(dll_extern, EntryPoint = "ov_partial_shape_to_string",
            CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public extern static string ov_partial_shape_to_string(Ov.ov_partial_shape partial_shape);
    }
}
