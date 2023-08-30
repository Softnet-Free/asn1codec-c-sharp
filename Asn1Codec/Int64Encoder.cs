/*
*	Copyright 2023 Robert Koifman
*
*   Licensed under the Apache License, Version 2.0 (the "License");
*   you may not use this file except in compliance with the License.
*   You may obtain a copy of the License at
*
*   http://www.apache.org/licenses/LICENSE-2.0
*
*   Unless required by applicable law or agreed to in writing, software
*   distributed under the License is distributed on an "AS IS" BASIS,
*   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*   See the License for the specific language governing permissions and
*   limitations under the License.
*/

using System;

namespace Softnet.Asn
{
    class Int64Encoder : ElementEncoder
    {
        long m_Value;

        public Int64Encoder(long value)
        {
            m_Value = value;
        }

        public bool IsConstructed()
        {
            return false;
        }

        public int EstimateSize()
        {
            if (m_Value > -128L)
            {
                if (m_Value <= 127L)
                {
                    return 3;
                }
                else if (m_Value <= 32767L)
                {
                    return 4;
                }
                else if (m_Value <= 8388607L)
                {
                    return 5;
                }
                else if (m_Value <= 2147483647L)
                {
                    return 6;
                }
                else if (m_Value <= 549755813887L)
                {
                    return 7;
                }
                else if (m_Value <= 140737488355327L)
                {
                    return 8;
                }
                else if (m_Value <= 36028797018963967L)
                {
                    return 9;
                }
                else
                    return 10;
            }
            else if (m_Value >= -32768)
            {
                return 4;
            }
            else if (m_Value >= -8388608)
            {
                return 5;
            }
            else if (m_Value >= -2147483648L)
            {
                return 6;
            }
            else if (m_Value >= -549755813888L)
            {
                return 7;
            }
            else if (m_Value >= -140737488355328L)
            {
                return 8;
            }
            else if (m_Value >= -36028797018963967L)
            {
                return 9;
            }
            else
                return 10;
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            int LV_length = EncodeLV(binStack);
            binStack.Stack(UniversalTag.Integer);
            return 1 + LV_length;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            byte b0 = (byte)(m_Value & 0x00000000000000ffL);
            byte b1 = (byte)((m_Value >> 8) & 0x00000000000000ffL);
            byte b2 = (byte)((m_Value >> 16) & 0x00000000000000ffL);
            byte b3 = (byte)((m_Value >> 24) & 0x00000000000000ffL);
            byte b4 = (byte)((m_Value >> 32) & 0x00000000000000ffL);
            byte b5 = (byte)((m_Value >> 40) & 0x00000000000000ffL);
            byte b6 = (byte)((m_Value >> 48) & 0x00000000000000ffL);
            byte b7 = (byte)((m_Value >> 56) & 0x00000000000000ffL);

            if (b7 == 0)
            {
                if (b6 == 0)
                {
                    if (b5 == 0)
                    {
                        if (b4 == 0)
                        {
                            if (b3 == 0)
                            {
                                if (b2 == 0)
                                {
                                    if (b1 == 0)
                                    {
                                        if (b0 == 0)
                                        {
                                            binStack.Stack((byte)0);
                                            binStack.Stack((byte)1);
                                            return 2;
                                        }
                                        else if (b0 <= 127)
                                        {
                                            binStack.Stack(b0);
                                            binStack.Stack((byte)1);
                                            return 2;
                                        }
                                        else
                                        {
                                            binStack.Stack(b0);
                                            binStack.Stack((byte)0);
                                            binStack.Stack((byte)2);
                                            return 3;
                                        }
                                    }
                                    else if (b1 <= 127)
                                    {
                                        binStack.Stack(b0);
                                        binStack.Stack(b1);
                                        binStack.Stack((byte)2);
                                        return 3;
                                    }
                                    else
                                    {
                                        binStack.Stack(b0);
                                        binStack.Stack(b1);
                                        binStack.Stack((byte)0);
                                        binStack.Stack((byte)3);
                                        return 4;
                                    }
                                }
                                else if (b2 <= 127)
                                {
                                    binStack.Stack(b0);
                                    binStack.Stack(b1);
                                    binStack.Stack(b2);
                                    binStack.Stack((byte)3);
                                    return 4;
                                }
                                else
                                {
                                    binStack.Stack(b0);
                                    binStack.Stack(b1);
                                    binStack.Stack(b2);
                                    binStack.Stack((byte)0);
                                    binStack.Stack((byte)4);
                                    return 5;
                                }
                            }
                            else if (b3 <= 127)
                            {
                                binStack.Stack(b0);
                                binStack.Stack(b1);
                                binStack.Stack(b2);
                                binStack.Stack(b3);
                                binStack.Stack((byte)4);
                                return 5;
                            }
                            else
                            {
                                binStack.Stack(b0);
                                binStack.Stack(b1);
                                binStack.Stack(b2);
                                binStack.Stack(b3);
                                binStack.Stack((byte)0);
                                binStack.Stack((byte)5);
                                return 6;
                            }
                        }
                        else if (b4 <= 127)
                        {
                            binStack.Stack(b0);
                            binStack.Stack(b1);
                            binStack.Stack(b2);
                            binStack.Stack(b3);
                            binStack.Stack(b4);
                            binStack.Stack((byte)5);
                            return 6;
                        }
                        else
                        {
                            binStack.Stack(b0);
                            binStack.Stack(b1);
                            binStack.Stack(b2);
                            binStack.Stack(b3);
                            binStack.Stack(b4);
                            binStack.Stack((byte)0);
                            binStack.Stack((byte)6);
                            return 7;
                        }
                    }
                    else if (b5 <= 127)
                    {
                        binStack.Stack(b0);
                        binStack.Stack(b1);
                        binStack.Stack(b2);
                        binStack.Stack(b3);
                        binStack.Stack(b4);
                        binStack.Stack(b5);
                        binStack.Stack((byte)6);
                        return 7;
                    }
                    else
                    {
                        binStack.Stack(b0);
                        binStack.Stack(b1);
                        binStack.Stack(b2);
                        binStack.Stack(b3);
                        binStack.Stack(b4);
                        binStack.Stack(b5);
                        binStack.Stack((byte)0);
                        binStack.Stack((byte)7);
                        return 8;
                    }
                }
                else if (b6 <= 127)
                {
                    binStack.Stack(b0);
                    binStack.Stack(b1);
                    binStack.Stack(b2);
                    binStack.Stack(b3);
                    binStack.Stack(b4);
                    binStack.Stack(b5);
                    binStack.Stack(b6);
                    binStack.Stack((byte)7);
                    return 8;
                }
                else
                {
                    binStack.Stack(b0);
                    binStack.Stack(b1);
                    binStack.Stack(b2);
                    binStack.Stack(b3);
                    binStack.Stack(b4);
                    binStack.Stack(b5);
                    binStack.Stack(b6);
                    binStack.Stack((byte)0);
                    binStack.Stack((byte)8);
                    return 9;
                }
            }
            else if (b7 <= 127)
            {
                binStack.Stack(b0);
                binStack.Stack(b1);
                binStack.Stack(b2);
                binStack.Stack(b3);
                binStack.Stack(b4);
                binStack.Stack(b5);
                binStack.Stack(b6);
                binStack.Stack(b7);
                binStack.Stack((byte)8);
                return 9;
            }
            else if (b7 == 255)
            {
                if (b6 == 255)
                {
                    if (b5 == 255)
                    {
                        if (b4 == 255)
                        {
                            if (b3 == 255)
                            {
                                if (b2 == 255)
                                {
                                    if (b1 == 255)
                                    {
                                        if (b0 >= 128)
                                        {
                                            binStack.Stack(b0);
                                            binStack.Stack((byte)1);
                                            return 2;
                                        }
                                        else
                                        {
                                            binStack.Stack(b0);
                                            binStack.Stack((byte)255);
                                            binStack.Stack((byte)2);
                                            return 3;
                                        }
                                    }
                                    else if (b1 >= 128)
                                    {
                                        binStack.Stack(b0);
                                        binStack.Stack(b1);
                                        binStack.Stack((byte)2);
                                        return 3;
                                    }
                                    else
                                    {
                                        binStack.Stack(b0);
                                        binStack.Stack(b1);
                                        binStack.Stack((byte)255);
                                        binStack.Stack((byte)3);
                                        return 4;
                                    }
                                }
                                else if (b2 >= 128)
                                {
                                    binStack.Stack(b0);
                                    binStack.Stack(b1);
                                    binStack.Stack(b2);
                                    binStack.Stack((byte)3);
                                    return 4;
                                }
                                else
                                {
                                    binStack.Stack(b0);
                                    binStack.Stack(b1);
                                    binStack.Stack(b2);
                                    binStack.Stack((byte)255);
                                    binStack.Stack((byte)4);
                                    return 5;
                                }
                            }
                            else if (b3 >= 128)
                            {
                                binStack.Stack(b0);
                                binStack.Stack(b1);
                                binStack.Stack(b2);
                                binStack.Stack(b3);
                                binStack.Stack((byte)4);
                                return 5;
                            }
                            else
                            {
                                binStack.Stack(b0);
                                binStack.Stack(b1);
                                binStack.Stack(b2);
                                binStack.Stack(b3);
                                binStack.Stack((byte)255);
                                binStack.Stack((byte)5);
                                return 6;
                            }
                        }
                        else if (b4 >= 128)
                        {
                            binStack.Stack(b0);
                            binStack.Stack(b1);
                            binStack.Stack(b2);
                            binStack.Stack(b3);
                            binStack.Stack(b4);
                            binStack.Stack((byte)5);
                            return 6;
                        }
                        else
                        {
                            binStack.Stack(b0);
                            binStack.Stack(b1);
                            binStack.Stack(b2);
                            binStack.Stack(b3);
                            binStack.Stack(b4);
                            binStack.Stack((byte)255);
                            binStack.Stack((byte)6);
                            return 7;
                        }
                    }
                    else if (b5 >= 128)
                    {
                        binStack.Stack(b0);
                        binStack.Stack(b1);
                        binStack.Stack(b2);
                        binStack.Stack(b3);
                        binStack.Stack(b4);
                        binStack.Stack(b5);
                        binStack.Stack((byte)6);
                        return 7;
                    }
                    else
                    {
                        binStack.Stack(b0);
                        binStack.Stack(b1);
                        binStack.Stack(b2);
                        binStack.Stack(b3);
                        binStack.Stack(b4);
                        binStack.Stack(b5);
                        binStack.Stack((byte)255);
                        binStack.Stack((byte)7);
                        return 8;
                    }
                }
                else if (b6 >= 128)
                {
                    binStack.Stack(b0);
                    binStack.Stack(b1);
                    binStack.Stack(b2);
                    binStack.Stack(b3);
                    binStack.Stack(b4);
                    binStack.Stack(b5);
                    binStack.Stack(b6);
                    binStack.Stack((byte)7);
                    return 8;
                }
                else
                {
                    binStack.Stack(b0);
                    binStack.Stack(b1);
                    binStack.Stack(b2);
                    binStack.Stack(b3);
                    binStack.Stack(b4);
                    binStack.Stack(b5);
                    binStack.Stack(b6);
                    binStack.Stack((byte)255);
                    binStack.Stack((byte)8);
                    return 9;
                }
            }
            else
            {
                binStack.Stack(b0);
                binStack.Stack(b1);
                binStack.Stack(b2);
                binStack.Stack(b3);
                binStack.Stack(b4);
                binStack.Stack(b5);
                binStack.Stack(b6);
                binStack.Stack(b7);
                binStack.Stack((byte)8);
                return 9;
            }
        }
    }
}
