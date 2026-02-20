// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

namespace OpenVinoSharp
{
    /// <summary>
    /// OpenVINO元素类型枚举 / OpenVINO element type enumeration
    /// <para>此枚举包含元素类型的代码。/ This enum contains codes for element types.</para>
    /// </summary>
    public enum ElementType : uint
    {
        /// <summary>
        /// 未定义元素类型 / Undefined element type
        /// </summary>
        UNDEFINED = 0U,
        
        /// <summary>
        /// 动态元素类型 / Dynamic element type
        /// </summary>
        DYNAMIC = UNDEFINED,
        
        /// <summary>
        /// 布尔元素类型 / Boolean element type
        /// </summary>
        BOOLEAN,
        
        /// <summary>
        /// BF16（脑浮点16位）元素类型 / BF16 (brain floating-point 16-bit) element type
        /// </summary>
        BF16,
        
        /// <summary>
        /// F16（半精度浮点）元素类型 / F16 (half-precision floating-point) element type
        /// </summary>
        F16,
        
        /// <summary>
        /// F32（单精度浮点）元素类型 / F32 (single-precision floating-point) element type
        /// </summary>
        F32,
        
        /// <summary>
        /// F64（双精度浮点）元素类型 / F64 (double-precision floating-point) element type
        /// </summary>
        F64,
        
        /// <summary>
        /// I4（4位有符号整数）元素类型 / I4 (4-bit signed integer) element type
        /// </summary>
        I4,
        
        /// <summary>
        /// I8（8位有符号整数）元素类型 / I8 (8-bit signed integer) element type
        /// </summary>
        I8,
        
        /// <summary>
        /// I16（16位有符号整数）元素类型 / I16 (16-bit signed integer) element type
        /// </summary>
        I16,
        
        /// <summary>
        /// I32（32位有符号整数）元素类型 / I32 (32-bit signed integer) element type
        /// </summary>
        I32,
        
        /// <summary>
        /// I64（64位有符号整数）元素类型 / I64 (64-bit signed integer) element type
        /// </summary>
        I64,
        
        /// <summary>
        /// U1（1位无符号整数）元素类型 / U1 (1-bit unsigned integer) element type
        /// </summary>
        U1,
        
        /// <summary>
        /// U2（2位无符号整数）元素类型 / U2 (2-bit unsigned integer) element type
        /// </summary>
        U2,
        
        /// <summary>
        /// U3（3位无符号整数）元素类型 / U3 (3-bit unsigned integer) element type
        /// </summary>
        U3,
        
        /// <summary>
        /// U4（4位无符号整数）元素类型 / U4 (4-bit unsigned integer) element type
        /// </summary>
        U4,
        
        /// <summary>
        /// U6（6位无符号整数）元素类型 / U6 (6-bit unsigned integer) element type
        /// </summary>
        U6,
        
        /// <summary>
        /// U8（8位无符号整数）元素类型 / U8 (8-bit unsigned integer) element type
        /// </summary>
        U8,
        
        /// <summary>
        /// U16（16位无符号整数）元素类型 / U16 (16-bit unsigned integer) element type
        /// </summary>
        U16,
        
        /// <summary>
        /// U32（32位无符号整数）元素类型 / U32 (32-bit unsigned integer) element type
        /// </summary>
        U32,
        
        /// <summary>
        /// U64（64位无符号整数）元素类型 / U64 (64-bit unsigned integer) element type
        /// </summary>
        U64,
        
        /// <summary>
        /// NF4（4位归一化浮点）元素类型 / NF4 (4-bit normalized floating-point) element type
        /// </summary>
        NF4,
        
        /// <summary>
        /// F8E4M3（8位浮点E4M3）元素类型 / F8E4M3 (8-bit floating-point E4M3) element type
        /// </summary>
        F8E4M3,
        
        /// <summary>
        /// F8E5M3（8位浮点E5M3）元素类型 / F8E5M3 (8-bit floating-point E5M3) element type
        /// </summary>
        F8E5M3,
        
        /// <summary>
        /// 字符串元素类型 / String element type
        /// </summary>
        STRING,
        
        /// <summary>
        /// F4E2M1（4位浮点E2M1）元素类型 / F4E2M1 (4-bit floating-point E2M1) element type
        /// </summary>
        F4E2M1,
        
        /// <summary>
        /// F8E8M0（8位浮点E8M0）元素类型 / F8E8M0 (8-bit floating-point E8M0) element type
        /// </summary>
        F8E8M0,
    }
}
