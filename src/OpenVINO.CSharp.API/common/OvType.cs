// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;

namespace OpenVinoSharp
{
    namespace element
    {
        /// <summary>
        /// OpenVINO元素类型类 / OpenVINO element type class
        /// </summary>
        public class Type
        {
            private ElementType _type;

            /// <summary>
            /// 从ElementType构造Type / Constructs Type from ElementType
            /// </summary>
            /// <param name="type">元素类型 / Element type</param>
            public Type(ElementType type)
            {
                _type = type;
            }

            /// <summary>
            /// 获取ElementType / Get the ElementType
            /// </summary>
            /// <returns>元素类型 / Element type</returns>
            public ElementType get_type()
            {
                return _type;
            }

            /// <summary>
            /// 获取类型的字节大小 / Get the size of the type in bytes
            /// </summary>
            /// <returns>字节大小 / Size in bytes</returns>
            public uint get_size()
            {
                switch (_type)
                {
                    case ElementType.BOOLEAN:
                    case ElementType.U1:
                    case ElementType.U2:
                    case ElementType.U3:
                    case ElementType.U4:
                    case ElementType.U6:
                    case ElementType.U8:
                    case ElementType.I4:
                    case ElementType.I8:
                        return 1;
                    case ElementType.BF16:
                    case ElementType.F16:
                    case ElementType.U16:
                    case ElementType.I16:
                    case ElementType.NF4:
                    case ElementType.F8E4M3:
                    case ElementType.F8E5M3:
                    case ElementType.F4E2M1:
                    case ElementType.F8E8M0:
                        return 2;
                    case ElementType.F32:
                    case ElementType.U32:
                    case ElementType.I32:
                        return 4;
                    case ElementType.F64:
                    case ElementType.U64:
                    case ElementType.I64:
                        return 8;
                    default:
                        return 0;
                }
            }
        }
    }

    /// <summary>
    /// OpenVINO类型包装器 / OpenVINO Type wrapper
    /// </summary>
    public struct OvType
    {
        private ElementType _type;

        /// <summary>
        /// 从ElementType构造OvType / Constructs OvType from ElementType
        /// </summary>
        /// <param name="type">元素类型 / Element type</param>
        public OvType(ElementType type)
        {
            _type = type;
        }

        /// <summary>
        /// 获取ElementType / Get the ElementType
        /// </summary>
        /// <returns>元素类型 / Element type</returns>
        public ElementType get_type()
        {
            return _type;
        }

        /// <summary>
        /// 获取类型的字节大小 / Get the size of the type in bytes
        /// </summary>
        /// <returns>字节大小 / Size in bytes</returns>
        public uint get_size()
        {
            switch (_type)
            {
                case ElementType.BOOLEAN:
                case ElementType.U1:
                case ElementType.U2:
                case ElementType.U3:
                case ElementType.U4:
                case ElementType.U6:
                case ElementType.U8:
                case ElementType.I4:
                case ElementType.I8:
                    return 1;
                case ElementType.BF16:
                case ElementType.F16:
                case ElementType.U16:
                case ElementType.I16:
                case ElementType.NF4:
                case ElementType.F8E4M3:
                case ElementType.F8E5M3:
                case ElementType.F4E2M1:
                case ElementType.F8E8M0:
                    return 2;
                case ElementType.F32:
                case ElementType.U32:
                case ElementType.I32:
                    return 4;
                case ElementType.F64:
                case ElementType.U64:
                case ElementType.I64:
                    return 8;
                default:
                    return 0;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _type.ToString();
        }
    }
}
