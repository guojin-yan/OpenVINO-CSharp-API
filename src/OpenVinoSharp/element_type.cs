using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVinoSharp
{
    namespace element
    {  
        /// <summary>
        /// Enum to define possible element types
        /// </summary>
        /// <ingroup>ov_element_c#_api</ingroup>
        public enum Type_t
        {
            undefined,  //!< Undefined element type
            dynamic,    //!< Dynamic element type
            boolean,    //!< boolean element type
            bf16,       //!< bf16 element type
            f16,        //!< f16 element type
            f32,        //!< f32 element type
            f64,        //!< f64 element type
            i4,         //!< i4 element type
            i8,         //!< i8 element type
            i16,        //!< i16 element type
            i32,        //!< i32 element type
            i64,        //!< i64 element type
            u1,         //!< binary element type
            u4,         //!< u4 element type
            u8,         //!< u8 element type
            u16,        //!< u16 element type
            u32,        //!< u32 element type
            u64         //!< u64 element type
        };

        struct TypeInfo
        {
            public ulong m_bitwidth;
            public bool m_is_real;
            public bool m_is_signed;
            public bool m_is_quantized;
            public string m_cname;
            public string m_type_name;
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
        /// </summary>
        /// <ingroup>ov_element_c#_api</ingroup>
        public class Type {
            private Type_t m_type = Type_t.undefined ;
            public Type(Type_t t) { m_type = t; }
            public Type(Type t) {
                m_type = t.m_type;
            } 
            public Type(string type) {
                new Type(type_from_string(type));
            }


            public string c_type_string()
            {
                return get_type_info(m_type).m_cname;
            }

            public ulong size()
            {
                return (bitwidth() + 7) >> 3;
            }

            public ulong hash()
            {
                return (ulong)(m_type);
            }

            public string get_type_name()
            {
                return to_string();
            }

            public bool is_integral()
            {
                return !is_real();
            }

            public string to_string()
            {
                return get_type_info(m_type).m_type_name;
            }

            public bool is_static()
            {
                return get_type_info(m_type).m_bitwidth != 0;
            }

            public bool is_real()
            {
                return get_type_info(m_type).m_is_real;
            }

            public bool is_integral_number()
            {
                return is_integral() && (m_type != Type_t.boolean);
            }

            public bool is_signed()
            {
                return get_type_info(m_type).m_is_signed;
            }

            public bool is_quantized()
            {
                return get_type_info(m_type).m_is_quantized;
            }

            public ulong bitwidth() {
            return get_type_info(m_type).m_bitwidth;
            }


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

      

    internal class element_type
    {
    }
}
