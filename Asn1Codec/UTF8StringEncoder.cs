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

namespace Softnet.Asn
{
    class UTF8StringEncoder : ElementEncoder
    {
        byte[] m_ValueBytes;

        private UTF8StringEncoder(byte[] valueBytes)
        {
            m_ValueBytes = valueBytes;
        }

        public static UTF8StringEncoder Create(string value)
        {
            byte[] valueBytes = UTF8Encoding.UTF8.GetBytes(value);
            return new UTF8StringEncoder(valueBytes);
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
            binStack.Stack(UniversalTag.UTF8String);
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
