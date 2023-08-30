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
    class GndTimeEncoder : ElementEncoder
    {
        byte[] V_bytes;
        int V_length;

        public GndTimeEncoder()
        {
            V_bytes = new byte[19];
            V_length = 0;
        }

        public static GndTimeEncoder Create(DateTime value)
        {
            GndTimeEncoder encoder = new GndTimeEncoder();
            encoder.encodeV(value);
            return encoder;
        }

        public bool IsConstructed()
        {
            return false;
        }

        public int EstimateSize()
        {
            return 2 + V_length;
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            binStack.Stack(V_bytes, 0, V_length);
            binStack.Stack((byte)V_length);
            binStack.Stack(UniversalTag.GeneralizedTime);
            return 2 + V_length;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            binStack.Stack(V_bytes, 0, V_length);
            binStack.Stack((byte)V_length);
            return 1 + V_length;
        }

        private void encodeV(DateTime _value)
        {
            DateTime value = _value.ToUniversalTime();

            int year = value.Year;
            int month = value.Month;
            int day = value.Day;
            int hour = value.Hour;
            int minute = value.Minute;
            int second = value.Second;
            int millisecond = value.Millisecond;            

            int digit = year / 1000;
            V_bytes[0] = (byte)(48 + digit);
            year = year - digit * 1000;
            digit = year / 100;
            V_bytes[1] = (byte)(48 + digit);
            year = year - digit * 100;
            digit = year / 10;
            V_bytes[2] = (byte)(48 + digit);
            digit = year % 10;
            V_bytes[3] = (byte)(48 + digit);

            V_bytes[4] = (byte)(48 + month / 10);
            V_bytes[5] = (byte)(48 + month % 10);

            V_bytes[6] = (byte)(48 + day / 10);
            V_bytes[7] = (byte)(48 + day % 10);

            V_bytes[8] = (byte)(48 + hour / 10);
            V_bytes[9] = (byte)(48 + hour % 10);

            V_bytes[10] = (byte)(48 + minute / 10);
            V_bytes[11] = (byte)(48 + minute % 10);

            V_bytes[12] = (byte)(48 + second / 10);
            V_bytes[13] = (byte)(48 + second % 10);

            int offset = 14;
            if (millisecond > 0)
            {
                int d1 = millisecond / 100;
                millisecond = millisecond - d1 * 100;
                int d2 = millisecond / 10;
                int d3 = millisecond % 10;

                V_bytes[offset] = (byte)46; // '.'
                offset++;

                if (d3 != 0)
                {
                    V_bytes[offset] = (byte)(48 + d1);
                    offset++;
                    V_bytes[offset] = (byte)(48 + d2);
                    offset++;
                    V_bytes[offset] = (byte)(48 + d3);
                    offset++;
                }
                else if (d2 != 0)
                {
                    V_bytes[offset] = (byte)(48 + d1);
                    offset++;
                    V_bytes[offset] = (byte)(48 + d2);
                    offset++;
                }
                else
                {
                    V_bytes[offset] = (byte)(48 + d1);
                    offset++;
                }
            }

            V_bytes[offset] = (byte)90; // 'Z';
            V_length = offset + 1;
        }
    }
}
