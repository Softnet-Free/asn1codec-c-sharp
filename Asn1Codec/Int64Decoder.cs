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
    class Int64Decoder
    {
        public static long Decode(byte[] buffer, int offset, int length)
        {
            if (length < 1)
                throw new FormatAsnException();

            long A = buffer[offset];
            if (length == 1)
            {
                if (A <= 127)
                {
                    return A;
                }
                else
                {
                    return (-256L /* 0xFFFFFFFFFFFFFF00 */) | A;
                }
            }

            long B = buffer[offset + 1];
            if (length == 2)
            {
                if (A <= 127)
                {
                    return (A << 8) | B;
                }
                else
                {
                    return (-65536L /* 0xFFFFFFFFFFFF0000 */) | (A << 8) | B;
                }
            }

            long C = buffer[offset + 2];
            if (length == 3)
            {
                if (A <= 127)
                {
                    return (A << 16) | (B << 8) | C;
                }
                else
                {
                    return (-16777216L /* 0xFFFFFFFFFF000000 */) | (A << 16) | (B << 8) | C;
                }
            }

            long D = buffer[offset + 3];
            if (length == 4)
            {
                if (A <= 127)
                {
                    return (A << 24) | (B << 16) | (C << 8) | D;
                }
                else
                {
                    return (-4294967296L /* 0xFFFFFFFF00000000 */) | (A << 24) | (B << 16) | (C << 8) | D;
                }
            }

            long E = buffer[offset + 4];
            if (length == 5)
            {
                if (A <= 127)
                {
                    return (A << 32) | (B << 24) | (C << 16) | (D << 8) | E;
                }
                else
                {
                    return (-1099511627776L /* 0xFFFFFF0000000000 */) | (A << 32) | (B << 24) | (C << 16) | (D << 8) | E;
                }
            }

            long F = buffer[offset + 5];
            if (length == 6)
            {
                if (A <= 127)
                {
                    return (A << 40) | (B << 32) | (C << 24) | (D << 16) | (E << 8) | F;
                }
                else
                {
                    return (-281474976710656L /* 0xFFFF000000000000 */) | (A << 40) | (B << 32) | (C << 24) | (D << 16) | (E << 8) | F;
                }
            }

            long G = buffer[offset + 6];
            if (length == 7)
            {
                if (A <= 127)
                {
                    return (A << 48) | (B << 40) | (C << 32) | (D << 24) | (E << 16) | (F << 8) | G;
                }
                else
                {
                    return (-72057594037927936L /* 0xFF00000000000000 */) | (A << 48) | (B << 40) | (C << 32) | (D << 24) | (E << 16) | (F << 8) | G;
                }
            }

            if (length == 8)
            {
                long H = buffer[offset + 7];
                return (A << 56) | (B << 48) | (C << 40) | (D << 32) | (E << 24) | (F << 16) | (G << 8) | H;
            }

            // length > 8
            throw new OverflowAsnException("The input number cannot be represented as a 64-bit signed integer.");
        }
    }
}
