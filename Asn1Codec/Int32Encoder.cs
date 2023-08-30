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
    class Int32Encoder : ElementEncoder
    {
        int m_Value;

        public Int32Encoder(int value)
        {
            m_Value = value;
        }

        public bool IsConstructed()
        {
            return false;
        }

        public int EstimateSize()
        {
            if (m_Value > -128)
            {
                if (m_Value <= 127)
                {
                    return 3;
                }
                else if (m_Value <= 32767)
                {
                    return 4;
                }
                else if (m_Value <= 8388607)
                {
                    return 5;
                }
                else 
                    return 6;
            }
            else if (m_Value >= -32768)
            {
                return 4;
            }
            else if (m_Value >= -8388608)
            {
                return 5;
            }
            else
                return 6;
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            int LV_length = EncodeLV(binStack);
            binStack.Stack(UniversalTag.Integer);
            return 1 + LV_length;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            byte b0 = (byte)(m_Value & 0x000000ff);
            byte b1 = (byte)((m_Value & 0x0000ff00) >> 8);
            byte b2 = (byte)((m_Value & 0x00ff0000) >> 16);
            byte b3 = (byte)((m_Value & 0xff000000) >> 24);

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
            else if (b3 == 255)
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
            else
            {
                binStack.Stack(b0);
                binStack.Stack(b1);
                binStack.Stack(b2);
                binStack.Stack(b3);
                binStack.Stack((byte)4);
                return 5;
            }
        }
    }
}
