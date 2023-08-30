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
using System.Text;
using System.Text.RegularExpressions;

namespace Softnet.Asn
{
    class SequenceEncoderImp : SequenceEncoder, ElementEncoder
    {
        private const byte C_Constructed_Flag = 0x20;
        private List<ElementEncoder> m_ChildNodes;

        public SequenceEncoderImp()
        {
            m_ChildNodes = new List<ElementEncoder>();
        }

        public int Count
        {
            get { return m_ChildNodes.Count; }
        }

        public bool IsConstructed()
        {
            return true;
        }

        public int EstimateSize()
        {
            int V_Length = 0;
            foreach(ElementEncoder encoder in m_ChildNodes)
                V_Length += encoder.EstimateSize();
            return 1 + LengthEncoder.EstimateSize(V_Length) + V_Length;
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            int V_Length = 0;
            for (int i = m_ChildNodes.Count - 1; i >= 0; i--)
                V_Length += m_ChildNodes[i].EncodeTLV(binStack);
            int L_length = LengthEncoder.Encode(V_Length, binStack);
            binStack.Stack((byte)(C_Constructed_Flag | UniversalTag.Sequence));

            return 1 + L_length + V_Length;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            int V_Length = 0;
            for (int i = m_ChildNodes.Count - 1; i >= 0; i--)
                V_Length += m_ChildNodes[i].EncodeTLV(binStack);
            int L_length = LengthEncoder.Encode(V_Length, binStack);

            return L_length + V_Length;
        }

        public SequenceEncoder Sequence()
        {
            SequenceEncoderImp encoder = new SequenceEncoderImp();
            m_ChildNodes.Add(encoder);
            return encoder;
        }

        public void Int32(int value)
        {
            m_ChildNodes.Add(new Int32Encoder(value));
        }

        public void Int64(long value)
        {
            m_ChildNodes.Add(new Int64Encoder(value));
        }

        public void Boolean(bool value)
	    {
		    m_ChildNodes.Add(new BooleanEncoder(value));
	    }

        public void GndTime(DateTime value)
        {            
            if(value == null)
                throw new ArgumentException("The 'value' argument must not be null");
            m_ChildNodes.Add(GndTimeEncoder.Create(value));
        }

        public void OctetString(byte[] buffer)
        {
            m_ChildNodes.Add(new OctetStringEncoder(buffer, 0, buffer.Length));
        }

        public void OctetString(byte[] buffer, int offset, int length)
        {
            m_ChildNodes.Add(new OctetStringEncoder(buffer, offset, length));
        }

        public void UTF8String(string value)
        {
            m_ChildNodes.Add(UTF8StringEncoder.Create(value));
        }

        public void BMPString(string value)
	    {
            m_ChildNodes.Add(BMPStringEncoder.Create(value));		
	    }

	    public void IA5String(string value)
	    {
            m_ChildNodes.Add(IA5StringEncoder.Create(value));	
	    }
	
	    public void PrintableString(string value)
	    {
            m_ChildNodes.Add(PrintableStringEncoder.Create(value));	
	    }

        public SequenceEncoder Sequence(int tag)
        {
            SequenceEncoderImp encoder = new SequenceEncoderImp();
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
            return encoder;
        }

        public void Null()
        {
            m_ChildNodes.Add(new NullEncoder());
        }

        public void Int32(int tag, int value)
        {
            Int32Encoder encoder = new Int32Encoder(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
        }

        public void Int64(int tag, long value)
        {
            Int64Encoder encoder = new Int64Encoder(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
        }

        public void Boolean(int tag, bool value)
        {
            BooleanEncoder encoder = new BooleanEncoder(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
        }
	
       	public void GndTime(int tag, DateTime value)
	    {
		    if(value == null)
                throw new ArgumentException("The 'value' argument must not be null.");

		    GndTimeEncoder encoder = GndTimeEncoder.Create(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
	    }

        public void OctetString(int tag, byte[] buffer)
        {
            OctetStringEncoder encoder = new OctetStringEncoder(buffer, 0, buffer.Length);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
        }

        public void OctetString(int tag, byte[] buffer, int offset, int length)
        {
            OctetStringEncoder encoder = new OctetStringEncoder(buffer, offset, length);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
        }

        public void UTF8String(int tag, string value)
        {
            UTF8StringEncoder encoder = UTF8StringEncoder.Create(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
        }

        public void BMPString(int tag, string value)
	    {
		    BMPStringEncoder encoder = BMPStringEncoder.Create(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));	
	    }

	    public void IA5String(int tag, string value)
	    {
		    if (Regex.IsMatch(value, @"[^\u0000-\u007F]", RegexOptions.None))
                throw new ArgumentException(string.Format("The argument '{0}' contains characters that are not allowed in 'Asn1 IA5String'.", value));
		
		    IA5StringEncoder encoder = IA5StringEncoder.Create(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
	    }
	
	    public void PrintableString(int tag, string value) 
	    {
            if (Regex.IsMatch(value, @"[^\u0020-\u007E]", RegexOptions.None))
                throw new ArgumentException(string.Format("The argument '{0}' contains characters that are not allowed in Asn1 PrintableString.", value));
		
		    PrintableStringEncoder encoder = PrintableStringEncoder.Create(value);
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, encoder));
	    }

        public void Null(int tag)
        {
            m_ChildNodes.Add(new TimpEncoder(tag, TagClass.ContextSpecific, new NullEncoder()));
        }
    }
}
