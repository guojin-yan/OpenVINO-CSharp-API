using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVinoSharp
{
    /// <summary>
    /// This enum contains codes for all possible return values of the interface functions
    /// </summary>
    public enum ExceptionStatus : int
     {
        OK = 0,  //!< SUCCESS!
        // map exception to C++ interface
        GENERAL_ERROR = -1,       //!< GENERAL_ERROR
        NOT_IMPLEMENTED = -2,     //!< NOT_IMPLEMENTED
        NETWORK_NOT_LOADED = -3,  //!< NETWORK_NOT_LOADED
        PARAMETER_MISMATCH = -4,  //!< PARAMETER_MISMATCH
        NOT_FOUND = -5,           //!< NOT_FOUND
        OUT_OF_BOUNDS = -6,       //!< OUT_OF_BOUNDS

        // exception not of std::exception derived type was thrown
        UNEXPECTED = -7,          //!< UNEXPECTED
        REQUEST_BUSY = -8,        //!< REQUEST_BUSY
        RESULT_NOT_READY = -9,    //!< RESULT_NOT_READY
        NOT_ALLOCATED = -10,      //!< NOT_ALLOCATED
        INFER_NOT_STARTED = -11,  //!< INFER_NOT_STARTED
        NETWORK_NOT_READ = -12,   //!< NETWORK_NOT_READ
        INFER_CANCELLED = -13,    //!< INFER_CANCELLED

        // exception in C wrapper
        INVALID_C_PARAM = -14,         //!< INVALID_C_PARAM
        UNKNOWN_C_ERROR = -15,         //!< UNKNOWN_C_ERROR
        NOT_IMPLEMENT_C_METHOD = -16,  //!< NOT_IMPLEMENT_C_METHOD
        UNKNOW_EXCEPTION = -17,        //!< UNKNOW_EXCEPTION
    }

    /// <summary>
    /// This enum contains codes for element type.
    /// </summary>
    public enum ElementType : uint
    {
        UNDEFINED = 0U,  //!< Undefined element type
        DYNAMIC,         //!< Dynamic element type
        BOOLEAN,         //!< boolean element type
        BF16,            //!< bf16 element type
        F16,             //!< f16 element type
        F32,             //!< f32 element type
        F64,             //!< f64 element type
        I4,              //!< i4 element type
        I8,              //!< i8 element type
        I16,             //!< i16 element type
        I32,             //!< i32 element type
        I64,             //!< i64 element type
        U1,              //!< binary element type
        U4,              //!< u4 element type
        U8,              //!< u8 element type
        U16,             //!< u16 element type
        U32,             //!< u32 element type
        U64,             //!< u64 element type
    };

}
