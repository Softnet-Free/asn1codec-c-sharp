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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Softnet.Asn
{
    public class ASNDecoder
    {
        const int C_Mask_Class = 0xC0;
        const int C_Mask_Tag = 0x1F;
        const int C_Universal_Class = 0;
        const int C_Constructed_Flag = 0x20;

        private ASNDecoder() { }

        public static SequenceDecoder Sequence(byte[] buffer)
        {
            return Sequence(buffer, 0); 
        }

        public static SequenceDecoder Sequence(byte[] buffer, int offset)
        {
            try
            {
                int T = buffer[offset];

                if ((T & C_Constructed_Flag) == 0)
                    throw new FormatAsnException();

                if ((T & C_Mask_Class) != C_Universal_Class)
                    throw new FormatAsnException();

                if ((T & C_Mask_Tag) != UniversalTag.Sequence)
                    throw new FormatAsnException();

                offset++;
                int L_length;
                int V_length = LengthDecoder.Decode(buffer, offset, out L_length);
                offset += L_length;

                if (offset + V_length > buffer.Length)
                    throw new FormatAsnException("The size of the input buffer is not enough to contain all the ASN.1 data.");

                return new SequenceDecoderImp(buffer, offset, V_length); 
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatAsnException("The size of the input buffer is not enough to contain all the ASN.1 data.");
            }
        }
    }
}
