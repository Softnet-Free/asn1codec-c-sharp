﻿/*
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
    public class ASNEncoder
    {
        SequenceEncoderImp m_Sequence;

        public ASNEncoder()
        {
            m_Sequence = new SequenceEncoderImp();
        }

        public SequenceEncoder Sequence
        {
            get { return m_Sequence; }
        }

        public byte[] GetEncoding()
        {
            int estimatedSize = m_Sequence.EstimateSize();
            BinaryStack binStack = new BinaryStack();
            binStack.Allocate(estimatedSize);
            m_Sequence.EncodeTLV(binStack);
            return binStack.Buffer;
        }

        public byte[] GetEncoding(int headerSize)
        {
            if (headerSize < 0)
                throw new ArgumentException("The value of 'headerSize' must not be negative.");

            int estimatedlength = m_Sequence.EstimateSize();
            BinaryStack binStack = new BinaryStack();
            binStack.Allocate(headerSize + estimatedlength);
            m_Sequence.EncodeTLV(binStack);
            return binStack.Buffer;
        }	
    }
}
