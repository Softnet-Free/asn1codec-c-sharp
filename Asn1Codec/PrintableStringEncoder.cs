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
using System.Text;
using System.Text.RegularExpressions;

namespace Softnet.Asn
{
    class PrintableStringEncoder : ElementEncoder
    {
        private byte[] m_ValueBytes;

        private PrintableStringEncoder(byte[] valueBytes)
        {
            m_ValueBytes = valueBytes;
        }

        public static PrintableStringEncoder Create(string value)
        {
            if (Regex.IsMatch(value, @"[^\u0020-\u007E]", RegexOptions.None))
                throw new ArgumentException(string.Format("The string '{0}' contains characters that are not allowed in 'Asn1 PrintableString'.", value));

            byte[] valueBytes = Encoding.ASCII.GetBytes(value);
            return new PrintableStringEncoder(valueBytes);
        }

        public bool IsConstructed()
        {
            return false;
        }

        public int EstimateSize()
        {
            return 1 + LengthEncoder.EstimateSize(m_ValueBytes.Length) + m_ValueBytes.Length;
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            binStack.Stack(m_ValueBytes);
            int L_length = LengthEncoder.Encode(m_ValueBytes.Length, binStack);
            binStack.Stack(UniversalTag.PrintableString);
            return 1 + L_length + m_ValueBytes.Length;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            binStack.Stack(m_ValueBytes);
            int L_length = LengthEncoder.Encode(m_ValueBytes.Length, binStack);
            return L_length + m_ValueBytes.Length;
        }
    }
}
