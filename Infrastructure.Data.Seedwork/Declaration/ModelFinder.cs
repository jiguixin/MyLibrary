namespace Infrastructure.Data.Seedwork.Declaration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class ModelFinder
    {
        private static Dictionary<int, object> _cache = new Dictionary<int, object>();
        private static OpCode[] _multiByteOpCodes;
        private static OpCode[] _singleByteOpCodes;

        private static Type GetDerivedModeleType<T>(ConstructorInfo vmCi)
        {
            MethodBody methodBody = null;
            byte[] il = null;
            int position = 0;
            ushort index = 0;
            int metadataToken = 0;
            Type mDerivedType = null;
            methodBody = vmCi.GetMethodBody();
            if (methodBody != null)
            {
                il = methodBody.GetILAsByteArray();
                while (position < il.Length)
                {
                    OpCode nop = OpCodes.Nop;
                    index = il[position++];
                    if (index != 0xfe)
                    {
                        nop = _singleByteOpCodes[index];
                    }
                    else
                    {
                        index = il[position++];
                        nop = _multiByteOpCodes[index];
                        index = (ushort) (index | 0xfe00);
                    }
                    metadataToken = 0;
                    if ((nop.OperandType == OperandType.InlineMethod) && (nop == OpCodes.Newobj))
                    {
                        metadataToken = ReadInt32(il, ref position);
                        try
                        {
                            mDerivedType = vmCi.Module.ResolveMethod(metadataToken).DeclaringType;
                            if ((mDerivedType.IsClass && !mDerivedType.IsAbstract) && IsUnderlineType<T>(mDerivedType))
                            {
                                return mDerivedType;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return null;
        }

        internal static Type GetDerivedModeleType<T>(Type vmType)
        {
            Type derivedModeleType = null;
            ConstructorInfo[] constructors = vmType.GetConstructors();
            foreach (ConstructorInfo info in constructors)
            {
                derivedModeleType = GetDerivedModeleType<T>(info);
                if (derivedModeleType != null)
                {
                    return derivedModeleType;
                }
            }
            return null;
        }

        public static bool HasCode(MethodInfo mi)
        {
            MethodBody methodBody = null;
            byte[] il = null;
            int position = 0;
            ushort index = 0;
            int num3 = 0;
            methodBody = mi.GetMethodBody();
            if (methodBody != null)
            {
                il = methodBody.GetILAsByteArray();
                while (position < il.Length)
                {
                    OpCode nop = OpCodes.Nop;
                    index = il[position++];
                    if (index != 0xfe)
                    {
                        nop = _singleByteOpCodes[index];
                    }
                    else
                    {
                        index = il[position++];
                        nop = _multiByteOpCodes[index];
                        index = (ushort) (index | 0xfe00);
                    }
                    if (nop.OperandType != OperandType.InlineNone)
                    {
                        num3 = 0;
                        switch (nop.OperandType)
                        {
                            case OperandType.InlineBrTarget:
                                num3 = ReadInt32(il, ref position) + position;
                                break;

                            case OperandType.InlineField:
                                num3 = ReadInt32(il, ref position);
                                break;

                            case OperandType.InlineI:
                            case OperandType.InlineI8:
                            case OperandType.InlineNone:
                            case OperandType.InlineR:
                            case OperandType.InlineVar:
                            case OperandType.ShortInlineI:
                            case OperandType.ShortInlineR:
                            case OperandType.ShortInlineVar:
                                break;

                            case OperandType.InlineMethod:
                                num3 = ReadInt32(il, ref position);
                                try
                                {
                                }
                                catch
                                {
                                }
                                break;

                            case OperandType.InlineSig:
                                num3 = ReadInt32(il, ref position);
                                break;

                            case OperandType.InlineString:
                                num3 = ReadInt32(il, ref position);
                                break;

                            case OperandType.InlineSwitch:
                            {
                                int num4 = ReadInt32(il, ref position);
                                int[] numArray = new int[num4];
                                int num5 = 0;
                                while (num5 < num4)
                                {
                                    numArray[num5] = ReadInt32(il, ref position);
                                    num5++;
                                }
                                int[] numArray2 = new int[num4];
                                for (num5 = 0; num5 < num4; num5++)
                                {
                                    numArray2[num5] = position + numArray[num5];
                                }
                                break;
                            }
                            case OperandType.InlineTok:
                                num3 = ReadInt32(il, ref position);
                                try
                                {
                                }
                                catch
                                {
                                }
                                break;

                            case OperandType.InlineType:
                                num3 = ReadInt32(il, ref position);
                                break;

                            case OperandType.ShortInlineBrTarget:
                            {
                                object obj2 = ReadSByte(il, ref position) + position;
                                break;
                            }
                            default:
                                throw new Exception("Unknown operand type.");
                        }
                    }
                }
            }
            return false;
        }

        public static void Initialize()
        {
            _singleByteOpCodes = new OpCode[0x100];
            _multiByteOpCodes = new OpCode[0x100];
            foreach (FieldInfo info in typeof(OpCodes).GetFields())
            {
                if (info.FieldType == typeof(OpCode))
                {
                    OpCode code = (OpCode) info.GetValue(null);
                    ushort index = (ushort) code.Value;
                    if (index < 0x100)
                    {
                        _singleByteOpCodes[index] = code;
                    }
                    else
                    {
                        if ((index & 0xff00) != 0xfe00)
                        {
                            throw new Exception("Invalid OpCode.");
                        }
                        _multiByteOpCodes[index & 0xff] = code;
                    }
                }
            }
        }

        private static bool IsUnderlineType<T>(Type mDerivedType)
        {
            return (((mDerivedType.GetType() == typeof(T)) || (mDerivedType.BaseType == typeof(T))) || ((mDerivedType.BaseType != null) && IsUnderlineType<T>(mDerivedType.BaseType)));
        }

        private static int ReadInt32(byte[] il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        }

        private static sbyte ReadSByte(byte[] il, ref int position)
        {
            return (sbyte) il[position++];
        }

        public static void UnLoad()
        {
            _cache.Clear();
        }
    }
}

