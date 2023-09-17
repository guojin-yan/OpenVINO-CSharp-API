using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVinoSharp
{
    /// <summary>
    /// The class of data type, mainly used for model data types. 
    /// </summary>
    /// <remarks>
    /// OvType inherits from element. Type
    /// </remarks>
    public class OvType : element.Type
    {
        /// <summary>
        /// OvType constructor, initializing element. Type
        /// </summary>
        /// <param name="t">ElementType data</param>
        public OvType(ElementType t) : base(t) { }
        /// <summary>
        /// OvType copy constructor, initializing element. Type
        /// </summary>
        /// <param name="t">OvType data</param>
        public OvType(OvType t) : base(t.m_type) { }
        /// <summary>
        /// OvType constructor, initializing element.Type through data type string
        /// </summary>
        /// <param name="type">data type string</param>
        public OvType(string type) : base (type) { }

    };
    namespace element
    {

        /// <summary>
        /// Enum to define possible element types
        /// <ingroup>ov_element_c#_api</ingroup>
        /// </summary>
        public enum Type_t
        {
            /// <summary>
            /// Undefined element type
            /// </summary>
            undefined,
            /// <summary>
            /// Dynamic element type
            /// </summary>
            dynamic,
            /// <summary>
            ///  boolean element type
            /// </summary>
            boolean,
            /// <summary>
            ///  bf16 element type
            /// </summary>
            bf16,
            /// <summary>
            /// f16 element type
            /// </summary>
            f16, 
            /// <summary>
            /// f32 element type
            /// </summary>
            f32,
            /// <summary>
            /// f64 element type
            /// </summary>
            f64,
            /// <summary>
            /// i4 element type
            /// </summary>
            i4,
            /// <summary>
            /// i8 element type
            /// </summary>
            i8,
            /// <summary>
            /// i16 element type
            /// </summary>
            i16,
            /// <summary>
            /// i32 element type
            /// </summary>
            i32,
            /// <summary>
            ///  i64 element type
            /// </summary>
            i64,
            /// <summary>
            ///  binary element type
            /// </summary>
            u1,
            /// <summary>
            /// u4 element type
            /// </summary>
            u4,
            /// <summary>
            ///  u8 element type
            /// </summary>
            u8,
            /// <summary>
            /// u16 element type
            /// </summary>
            u16,
            /// <summary>
            /// u32 element type
            /// </summary>
            u32,
            /// <summary>
            /// u64 element type
            /// </summary>
            u64
        };

        /// <summary>
        /// [struct] Type information storage struct.
        /// </summary>
        struct TypeInfo
        {
            /// <summary>
            /// data length.
            /// </summary>
            public ulong m_bitwidth;
            /// <summary>
            /// real number flag
            /// </summary>
            public bool m_is_real;
            /// <summary>
            /// signed number flag
            /// </summary>
            public bool m_is_signed;
            /// <summary>
            /// quantize number flag
            /// </summary>
            public bool m_is_quantized;
            /// <summary>
            /// type name full name string 
            /// </summary>
            public string m_cname;
            /// <summary>
            /// type name abbreviation string 
            /// </summary>
            public string m_type_name;
            /// <summary>
            /// Structure constructor
            /// </summary>
            /// <param name="bitwidth">data length.</param>
            /// <param name="is_real">real number flag</param>
            /// <param name="is_signed">signed number flag</param>
            /// <param name="is_quantized">quantize number flag</param>
            /// <param name="cname"> type name full name string</param>
            /// <param name="type_name">type name abbreviation string</param>
            public TypeInfo(ulong bitwidth, bool is_real, bool is_signed, bool is_quantized, string cname, string type_name) 
            { 
                m_bitwidth = bitwidth;
                m_is_real = is_real;
                m_is_signed = is_signed;
                m_is_quantized = is_quantized;
                m_cname = cname;
                m_type_name = type_name;
            }
        }
        /// <summary>
        /// Base class to define element type
        /// <ingroup>ov_element_c#_api</ingroup>
        /// </summary>
        public class Type {
            /// <summary>
            /// data type, defined based on Type_t.
            /// </summary>
            protected Type_t m_type = Type_t.undefined ;
            /// <summary>
            /// OvType constructor, by Type_t initialize the Type class
            /// </summary>
            /// <param name="t">Type_t data</param>
            public Type(Type_t t) { m_type = t; }
            /// <summary>
            /// OvType constructor, by ElementType initialize the Type class
            /// </summary>
            /// <param name="t">ElementType data</param>
            public Type(ElementType t) { m_type = (Type_t)t; }
            /// <summary>
            /// OvType copy constructor, by Type initialize the Type class
            /// </summary>
            /// <param name="t">Type data</param>
            public Type(Type t) {
                m_type = t.m_type;
            }
            /// <summary>
            /// OvType constructor, initializing element.Type through data type string
            /// </summary>
            /// <param name="type">data type string</param>
            public Type(string type) {
                new Type(type_from_string(type));
            }
            /// <summary>
            /// Get data type.
            /// </summary>
            /// <returns>ElementType type</returns>
            public ElementType get_type() {
                return (ElementType)m_type;
            }
            /// <summary>
            /// Get type full name string.
            /// </summary>
            /// <returns>full name string</returns>
            public string c_type_string()
            {
                return get_type_info(m_type).m_cname;
            }
            /// <summary>
            /// Get data type length.
            /// </summary>
            /// <returns>type length</returns>
            public ulong size()
            {
                return (bitwidth() + 7) >> 3;
            }
            /// <summary>
            /// Get type number.
            /// </summary>
            /// <returns>type number</returns>
            public ulong hash()
            {
                return (ulong)(m_type);
            }
            /// <summary>
            /// Get abbreviated name.
            /// </summary>
            /// <returns>abbreviated name</returns>
            public string get_type_name()
            {
                return to_string();
            }
            /// <summary>
            /// Determine whether it is a real number
            /// </summary>
            /// <returns>true: is real; false: not real</returns>
            public bool is_integral()
            {
                return !is_real();
            }
            /// <summary>
            /// Convert data type to string
            /// </summary>
            /// <returns> data type string</returns>
            public string to_string()
            {
                return get_type_info(m_type).m_type_name;
            }
            /// <summary>
            /// Determine whether the current data type is static.
            /// </summary>
            /// <returns>true : is static; false : not static</returns>
            public bool is_static()
            {
                return get_type_info(m_type).m_bitwidth != 0;
            }
            /// <summary>
            /// Determine whether the current data type is real.
            /// </summary>
            /// <returns>true : is real; false : not real</returns>
            public bool is_real()
            {
                return get_type_info(m_type).m_is_real;
            }
            /// <summary>
            /// Determine whether the current data type is integral number.
            /// </summary>
            /// <returns>true : is integral number; false : not integral number</returns>
            public bool is_integral_number()
            {
                return is_integral() && (m_type != Type_t.boolean);
            }
            /// <summary>
            /// Determine whether the current data type is signed.
            /// </summary>
            /// <returns>true : is signed; false : not signed</returns>
            public bool is_signed()
            {
                return get_type_info(m_type).m_is_signed;
            }
            /// <summary>
            /// Determine whether the current data is of quantum type
            /// </summary>
            /// <returns>true : is quantized; false : not quantized</returns>
            public bool is_quantized()
            {
                return get_type_info(m_type).m_is_quantized;
            }
            /// <summary>
            /// Obtain the size of the current data type
            /// </summary>
            /// <returns>the size of the current data type</returns>
            public ulong bitwidth()
            {
                return get_type_info(m_type).m_bitwidth;
            }

            /// <summary>
            /// Get the current type of the Type_ Info
            /// </summary>
            /// <param name="type">Type_t</param>
            /// <returns>TypeInfo data</returns>
            TypeInfo get_type_info(element.Type_t type)
            {
                switch (type)
                {
                    case element.Type_t.undefined:
                        return new TypeInfo(10000, false, false, false, "undefined", "undefined");
                    case element.Type_t.dynamic:
                        return new TypeInfo(0, false, false, false, "dynamic", "dynamic");
                    case element.Type_t.boolean:
                        return new TypeInfo(8, false, true, false, "char", "boolean");
                    case element.Type_t.bf16:
                        return new TypeInfo(16, true, true, false, "bfloat16", "bf16");
                    case element.Type_t.f16:
                        return new TypeInfo(16, true, true, false, "float16", "f16");
                    case element.Type_t.f32:
                        return new TypeInfo(32, true, true, false, "float", "f32");
                    case element.Type_t.f64:
                        return new TypeInfo(64, true, true, false, "double", "f64");
                    case element.Type_t.i4:
                        return new TypeInfo(4, false, true, true, "int4_t", "i4");
                    case element.Type_t.i8:
                        return new TypeInfo(8, false, true, true, "int8_t", "i8");
                    case element.Type_t.i16:
                        return new TypeInfo(16, false, true, false, "int16_t", "i16");
                    case element.Type_t.i32:
                        return new TypeInfo(32, false, true, true, "int32_t", "i32");
                    case element.Type_t.i64:
                        return new TypeInfo(64, false, true, false, "int64_t", "i64");
                    case element.Type_t.u1:
                        return new TypeInfo(1, false, false, false, "uint1_t", "u1");
                    case element.Type_t.u4:
                        return new TypeInfo(4, false, false, false, "uint4_t", "u4");
                    case element.Type_t.u8:
                        return new TypeInfo(8, false, false, true, "uint8_t", "u8");
                    case element.Type_t.u16:
                        return new TypeInfo(16, false, false, false, "uint16_t", "u16");
                    case element.Type_t.u32:
                        return new TypeInfo(32, false, false, false, "uint32_t", "u32");
                    case element.Type_t.u64:
                        return new TypeInfo(64, false, false, false, "uint64_t", "u64");
                    default:
                        return new TypeInfo(100000, false, false, false, "default", "default");
                }
            }
            /// <summary>
            /// Convert type string to Type class
            /// </summary>
            /// <param name="type">type string</param>
            /// <returns>Type class</returns>
            Type type_from_string(string type)
            {
                if (type == "f16" || type == "FP16")
                {
                    return new Type(Type_t.f16);
                }
                else if (type == "f32" || type == "FP32")
                {
                    return new Type(Type_t.f32);
                }
                else if (type == "bf16" || type == "BF16")
                {
                    return new Type(Type_t.bf16);
                }
                else if (type == "f64" || type == "FP64")
                {
                    return new Type(Type_t.f64);
                }
                else if (type == "i4" || type == "I4")
                {
                    return new Type(Type_t.i4);
                }
                else if (type == "i8" || type == "I8")
                {
                    return new Type(Type_t.i8);
                }
                else if (type == "i16" || type == "I16")
                {
                    return new Type(Type_t.i16);
                }
                else if (type == "i32" || type == "I32")
                {
                    return new Type(Type_t.i32);
                }
                else if (type == "i64" || type == "I64")
                {
                    return new Type(Type_t.i64);
                }
                else if (type == "u1" || type == "U1" || type == "BIN" || type == "bin")
                {
                    return new Type(Type_t.u1);
                }
                else if (type == "u4" || type == "U4")
                {
                    return new Type(Type_t.u4);
                }
                else if (type == "u8" || type == "U8")
                {
                    return new Type(Type_t.u8);
                }
                else if (type == "u16" || type == "U16")
                {
                    return new Type(Type_t.u16);
                }
                else if (type == "u32" || type == "U32")
                {
                    return new Type(Type_t.u32);
                }
                else if (type == "u64" || type == "U64")
                {
                    return new Type(Type_t.u64);
                }
                else if (type == "boolean" || type == "BOOL")
                {
                    return new Type(Type_t.boolean);
                }
                else if (type == "undefined" || type == "UNSPECIFIED")
                {
                    return new Type(Type_t.undefined);
                }
                else if (type == "dynamic")
                {
                    return new Type(Type_t.dynamic);
                }
                else
                {
                    return new Type(Type_t.undefined);
                }
            }    
        };

       
    }
}
