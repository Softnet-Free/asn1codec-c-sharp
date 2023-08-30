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
    class Int32Decoder
    {
        public static int Decode(byte[] buffer, int offset, int length)
        {
            if (length < 1)
                throw new FormatAsnException();

            int A = buffer[offset];
            if (length == 1)
            {
                if (A <= 127)
                {                    
                    return A;
                }
                else
                {
                    return (-256 /* 0xFFFFFF00 */) | A;
                }
            }

            int B = buffer[offset + 1];
            if (length == 2)
            {
                if (A <= 127)
                {
                    return (A << 8) | B;
                }
                else
                {
                    return (-65536 /* 0xFFFF0000 */) | (A << 8) | B;
                }
            }

            int C = buffer[offset + 2];
            if (length == 3)
            {
                if (A <= 127)
                {
                    return (A << 16) | (B << 8) | C;
                }
                else
                {
                    return (-16777216 /* 0xFF000000 */) | (A << 16) | (B << 8) | C;
                }
            }

            if (length == 4)
            {
                int D = buffer[offset + 3];
                return (A << 24) | (B << 16) | (C << 8) | D;
            }

            // length >= 5
            throw new OverflowAsnException("The input number cannot be represented as a 32-bit signed integer.");
        }
    }
}
