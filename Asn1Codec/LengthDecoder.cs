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
    class LengthDecoder
    {
        public static int Decode(byte[] buffer, int offset, out int L_length)
        {
            try
            {
                int L1_Byte = buffer[offset];

                if (L1_Byte <= 127)
                {
                    L_length = 1;
                    return L1_Byte;
                }

                if (L1_Byte == 128)
                    throw new FormatAsnException("The ASN.1 codec does not support the indefinite length form.");

                int L_Size = L1_Byte & 0x7F;
                offset++;

                if (L_Size == 1)
                {
                    int length = buffer[offset];
                    L_length = 2;
                    return length;
                }
                else if (L_Size == 2)
                {
                    int b1 = buffer[offset];
                    int b0 = buffer[offset + 1];
                    int length = (b1 << 8) | b0;
                    L_length = 3;
                    return length;
                }
                else if (L_Size == 3)
                {
                    int b2 = buffer[offset];
                    int b1 = buffer[offset + 1];
                    int b0 = buffer[offset + 2];
                    int length = (b2 << 16) | (b1 << 8) | b0;
                    L_length = 4;
                    return length;
                }
                else if (L_Size == 4)
                {
                    int b3 = buffer[offset];
                    if (b3 >= 128)
                        throw new FormatAsnException("The ASN.1 codec does not support the length of content more than 2GB.");

                    int b2 = buffer[offset + 1];
                    int b1 = buffer[offset + 2];
                    int b0 = buffer[offset + 3];
                    int length = (b3 << 24) | (b2 << 16) | (b1 << 8) | b0;
                    L_length = 5;
                    return length;
                }

                throw new FormatAsnException("The ASN.1 codec does not support the length of content more than 2GB.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatAsnException("The size of the input buffer is not enough to contain all the ASN.1 data.");
            }
        }
    }
}
