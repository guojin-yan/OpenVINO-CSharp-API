using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO C API Return value anomaly detection handle
    /// </summary>
    static class HandleException
    {
        /// <summary>
        /// Check if there are any abnormalities in the return value, and if so, return the 
        /// corresponding exceptions according to the abnormal value
        /// </summary>
        /// <param name="status"></param>
        public static void handler(ExceptionStatus status) {
            if (ExceptionStatus.OK == status)
            {
                return;
            }
            else if (ExceptionStatus.GENERAL_ERROR == status)
            {
                general_error();
            }
            else if (ExceptionStatus.NOT_IMPLEMENTED == status)
            {
                not_implemented();
            }
            else if (ExceptionStatus.NETWORK_NOT_LOADED == status)
            {
                network_not_loaded();
            }
            else if (ExceptionStatus.PARAMETER_MISMATCH == status)
            {
                parameter_mismatch();
            }
            else if (ExceptionStatus.NOT_FOUND == status)
            {
                not_found();
            }
            else if (ExceptionStatus.OUT_OF_BOUNDS == status)
            {
                out_of_bounds();
            }
            else if (ExceptionStatus.UNEXPECTED == status)
            {
                unexpection();
            }
            else if (ExceptionStatus.REQUEST_BUSY == status)
            {
                request_busy();
            }
            else if (ExceptionStatus.RESULT_NOT_READY == status)
            {
                result_not_ready();
            }
            else if (ExceptionStatus.NOT_ALLOCATED == status)
            {
                not_allocated();
            }
            else if (ExceptionStatus.INFER_NOT_STARTED == status)
            {
                infer_not_started();
            }
            else if (ExceptionStatus.NETWORK_NOT_READ == status)
            {
                netword_not_read();
            }
            else if (ExceptionStatus.INFER_CANCELLED == status)
            {
                infer_cancelled();
            }
            else if (ExceptionStatus.INVALID_C_PARAM == status)
            {
                invalid_c_param();
            }
            else if (ExceptionStatus.UNKNOWN_C_ERROR == status)
            {
                unknown_c_error();
            }
            else if (ExceptionStatus.NOT_IMPLEMENT_C_METHOD == status)
            {
                not_implement_c_method();
            }
            else if (ExceptionStatus.UNKNOW_EXCEPTION == status)
            {
                unknown_exception();
            }
            else if (ExceptionStatus.PTR_NULL == status)
            {
                ptr_null_exception();
            }

        }
        /// <summary>
        /// Throw GENERAL_ERROR OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">general error!</exception>
        private static void general_error() {
            throw new OVException(ExceptionStatus.GENERAL_ERROR, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw NOT_IMPLEMENTED OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">not implemented!</exception>
        private static void not_implemented()
        {
            throw new OVException(ExceptionStatus.NOT_IMPLEMENTED, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }

        /// <summary>
        /// Throw NETWORK_NOT_LOADED OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">network not loaded!</exception>
        private static void network_not_loaded()
        {
            throw new OVException(ExceptionStatus.NETWORK_NOT_LOADED, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }


        /// <summary>
        /// Throw PARAMETER_MISMATCH OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">parameter mismatch!</exception>
        private static void parameter_mismatch()
        {
            throw new OVException(ExceptionStatus.PARAMETER_MISMATCH, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }

        /// <summary>
        /// Throw NOT_FOUND OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">not found!</exception>
        private static void not_found()
        {
            throw new OVException(ExceptionStatus.NOT_FOUND, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }

        /// <summary>
        /// Throw OUT_OF_BOUNDS OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">out of bounds!</exception>
        private static void out_of_bounds()
        {
            throw new OVException(ExceptionStatus.OUT_OF_BOUNDS, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }


        /// <summary>
        /// Throw UNEXPECTED OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">unexpection!</exception>
        private static void unexpection()
        {
            throw new OVException(ExceptionStatus.UNEXPECTED, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }



        /// <summary>
        /// Throw REQUEST_BUSY OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">request busy!</exception>
        private static void request_busy()
        {
            throw new OVException(ExceptionStatus.REQUEST_BUSY, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw RESULT_NOT_READY OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">result not ready!</exception>
        private static void result_not_ready()
        {
            throw new OVException(ExceptionStatus.RESULT_NOT_READY, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">not allocated!</exception>
        private static void not_allocated()
        {
            throw new OVException(ExceptionStatus.NOT_ALLOCATED, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw INFER_NOT_STARTED OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">infer not started!</exception>
        private static void infer_not_started()
        {
            throw new OVException(ExceptionStatus.INFER_NOT_STARTED, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw NETWORK_NOT_READ OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">netword not read!</exception>
        private static void netword_not_read()
        {
            throw new OVException(ExceptionStatus.NETWORK_NOT_READ, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw INFER_CANCELLED OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">infer cancelled!</exception>
        private static void infer_cancelled()
        {
            throw new OVException(ExceptionStatus.INFER_CANCELLED, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw INVALID_C_PARAM OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">invalid c param!</exception>
        private static void invalid_c_param()
        {
            throw new OVException(ExceptionStatus.INVALID_C_PARAM, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw UNKNOWN_C_ERROR OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">unknown c error!</exception>
        private static void unknown_c_error()
        {
            throw new OVException(ExceptionStatus.UNKNOWN_C_ERROR, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw NOT_IMPLEMENT_C_METHOD OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">not implement c method!</exception>
        private static void not_implement_c_method()
        {
            throw new OVException(ExceptionStatus.NOT_IMPLEMENT_C_METHOD, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw UNKNOW_EXCEPTION OpenVINOException.
        /// </summary>
        /// <exception cref="OVException">unknown exception!</exception>
        private static void unknown_exception()
        {
            throw new OVException(ExceptionStatus.UNKNOW_EXCEPTION, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
        /// <summary>
        /// Throw PTR_NULL OpenVINOException.
        /// </summary>
        /// <exception cref="OVException"></exception>
        private static void ptr_null_exception()
        {
            throw new OVException(ExceptionStatus.UNKNOW_EXCEPTION, Marshal.PtrToStringAnsi(NativeMethods.ov_get_last_err_msg()));
        }
    }
}
