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
    class SequenceDecoderImp : SequenceDecoder
    {
        const int C_Mask_Class = 0xC0;
        const int C_Mask_Tag = 0x1F;
        const int C_ContextSpecific_Class = 0x80;
        const int C_Universal_Class = 0;
        const int C_Constructed_Flag = 0x20;

        byte[] m_buffer;
        int m_offset;
        int m_data_begin;
        int m_data_end;

        public SequenceDecoderImp(byte[] buffer, int offset, int length)
        {
            m_buffer = buffer;
            m_offset = offset;
            m_data_begin = offset;
            m_data_end = offset + length;
        }

        int m_count = -1;
        public int Count()
        {
            if (m_count == -1)
            {
                m_count = 0;
                int offset = m_data_begin;
                while (offset < m_data_end)
                { 
                    int L_Length;
                    int V_Length = LengthDecoder.Decode(m_buffer, offset + 1, out L_Length);
                    offset += 1 + L_Length + V_Length;
                    m_count++;
                }

                if(offset != m_data_end)
                    throw new FormatAsnException();
            }
            return m_count;
        }

        public bool Exists(UType type)
        {
            if (m_offset == m_data_end)
                return false;

            int T = m_buffer[m_offset];
            if ((T & C_Mask_Class) != C_Universal_Class)
                return false;

            if ((T & C_Mask_Tag) == (int)type)
                return true;
            return false;
        }

        public bool Exists(int tag)
        {
            if (m_offset == m_data_end)
        		return false;

            int T = m_buffer[m_offset];
            if ((T & C_Mask_Class) == C_Universal_Class)
        		return false;
        	
        	if((T & C_Mask_Tag) == tag)
        		return true;
        	return false;
        }

        public bool IsNull()
    	{
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];        

            if((T & C_Constructed_Flag) != 0)
                return false;

            if ((T & C_Mask_Class) == C_Universal_Class) 
            {
	    	    if((T & C_Mask_Tag) == UniversalTag.Null)
	    		    return true;
            }
            else 
            {
	    	    if(m_offset + 1 >= m_data_end)
	        	    throw new FormatAsnException();
	        
	            if(m_buffer[m_offset + 1] == 0)
	        	    return true;
            }        
    	    return false;
	    }

        public bool HasNext()
        {
            if (m_offset < m_data_end)
                return true;
            return false;
        }

        public void End()
        {
            if (m_offset < m_data_end)
                throw new EndNotReachedAsnException();
        }

        public SequenceDecoder Sequence()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];

            int tagClass = T & C_Mask_Class;
            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) == 0 || (T & C_Mask_Tag) != UniversalTag.Sequence)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                var decoder = new SequenceDecoderImp(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return decoder;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) == 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                var decoder = new SequenceDecoderImp(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return decoder;
            }
            
            throw new TypeMismatchAsnException();
        }

        public int Int32()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];

            int tagClass = T & C_Mask_Class;
            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.Integer)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                int value = Int32Decoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();
                
                int V_Length = DecodeLength();
                int value = Int32Decoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public int Int32(int minValue, int maxValue)
        {
            int value = Int32();
            if (value < minValue || maxValue < value)
                throw new RestrictionAsnException(string.Format("The value of the input integer must be in the range [{0}, {1}], while the actual value is {2}.", minValue, maxValue, value));
            return value;
        }

        public long Int64()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.Integer)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                long value = Int64Decoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                long value = Int64Decoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public bool Boolean()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];

            int tagClass = T & C_Mask_Class;
            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.Boolean)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                if(V_Length != 1)
                    throw new FormatAsnException();

                int value = m_buffer[m_offset];
                m_offset += V_Length;

                if (value == 0)
                    return false;
                else if(value == 255)
                    return true;

                throw new FormatAsnException();
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                if (V_Length != 1)
                    throw new FormatAsnException();

                int value = m_buffer[m_offset];
                m_offset += V_Length;

                if (value == 0)
                    return false;
                else if (value == 255)
                    return true;

                throw new FormatAsnException();
            }

            throw new TypeMismatchAsnException();
        }

        public string UTF8String()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.UTF8String)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = UTF8StringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = UTF8StringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public string UTF8String(int requiredLength)
        {
            string value = UTF8String();
            if (value.Length != requiredLength)
                throw new RestrictionAsnException(string.Format("The length of the input string must be {0}, while the actual length is {1}.", requiredLength, value.Length));
            return value;        
        }

        public string UTF8String(int minLength, int maxLength)
        {
            string value = UTF8String();
            if (value.Length < minLength || maxLength < value.Length)
                throw new RestrictionAsnException(string.Format("The length of the input string must be in the range [{0}, {1}], while the actual length is {2}.", minLength, maxLength, value.Length));
            return value;        
        }

        public string BMPString()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.BMPString)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = BMPStringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = BMPStringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public string BMPString(int requiredLength)
        {
            string value = BMPString();
            if (value.Length != requiredLength)
                throw new RestrictionAsnException(string.Format("The length of the input string must be {0}, while the actual length is {1}.", requiredLength, value.Length));
            return value;
        }

        public string BMPString(int minLength, int maxLength)
        {
            string value = BMPString();
            if (value.Length < minLength || maxLength < value.Length)
                throw new RestrictionAsnException(string.Format("The length of the input string must be in the range [{0}, {1}], while the actual length is {2}.", minLength, maxLength, value.Length));
            return value;
        }

        public string IA5String()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.IA5String)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = IA5StringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = IA5StringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public string IA5String(int requiredLength)
        {
            string value = IA5String();
            if (value.Length != requiredLength)
                throw new RestrictionAsnException(string.Format("The length of the input string must be {0}, while the actual length is {1}.", requiredLength, value.Length));
            return value;    
        }

        public string IA5String(int minLength, int maxLength)
        {
            string value = IA5String();
            if (value.Length < minLength || maxLength < value.Length)
                throw new RestrictionAsnException(string.Format("The length of the input string must be in the range [{0}, {1}], while the actual length is {2}.", minLength, maxLength, value.Length));
            return value; 
        }

        public string PrintableString()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.PrintableString)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = PrintableStringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                string value = PrintableStringDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public string PrintableString(int requiredLength)
        {
            string value = PrintableString();
            if (value.Length != requiredLength)
                throw new RestrictionAsnException(string.Format("The length of the input string must be {0}, while the actual length is {1}.", requiredLength, value.Length));
            return value;  
        }

        public string PrintableString(int minLength, int maxLength)
        {
            string value = PrintableString();
            if (value.Length < minLength || maxLength < value.Length)
                throw new RestrictionAsnException(string.Format("The length of the input string must be in the range [{0}, {1}], while the actual length is {2}.", minLength, maxLength, value.Length));
            return value; 
        }

        public DateTime GndTime()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.GeneralizedTime)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                DateTime value = GndTimeDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();
                DateTime value = GndTimeDecoder.Decode(m_buffer, m_offset, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public byte[] OctetString()
        {
            if (m_offset == m_data_end)
                throw new EndOfContainerAsnException();

            int T = m_buffer[m_offset];
            int tagClass = T & C_Mask_Class;

            if (tagClass == C_Universal_Class)
            {
                if ((T & C_Constructed_Flag) != 0 || (T & C_Mask_Tag) != UniversalTag.OctetString)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();

                byte[] value = new byte[V_Length];
                Buffer.BlockCopy(m_buffer, m_offset, value, 0, V_Length);
                m_offset += V_Length;

                return value;
            }
            else if (tagClass == C_ContextSpecific_Class)
            {
                if ((T & C_Constructed_Flag) != 0)
                    throw new TypeMismatchAsnException();

                int V_Length = DecodeLength();

                byte[] value = new byte[V_Length];
                Buffer.BlockCopy(m_buffer, m_offset, value, 0, V_Length);
                m_offset += V_Length;

                return value;
            }

            throw new TypeMismatchAsnException();
        }

        public byte[] OctetString(int requiredLength)
        {
            byte[] value = OctetString();
            if (value.Length != requiredLength)
                throw new RestrictionAsnException(string.Format("The length of the input octet string must be {0}, while the actual length is {1}.", requiredLength, value.Length));
            return value;
        }

        public byte[] OctetString(int minLength, int maxLength)
        {
            byte[] value = OctetString();
            if (value.Length < minLength || maxLength < value.Length)
                throw new RestrictionAsnException(string.Format("The length of the input octet string must be in the range [{0}, {1}], while the actual length is {2}.", minLength, maxLength, value.Length));
            return value;        
        }

        private int DecodeLength()
        {
            m_offset++;
            try
            {
                int L1_Byte = m_buffer[m_offset];
                m_offset++;

                if (L1_Byte <= 127)
                {
                    if (m_offset + L1_Byte > m_data_end)
                        throw new FormatAsnException();
                    return L1_Byte;
                }

                if (L1_Byte == 128)
                    throw new FormatAsnException("The ASN1 codec does not support the indefinite length form.");

                int bytes = L1_Byte & 0x7F;

                if (bytes == 1)
                {
                    int length = m_buffer[m_offset];
                    m_offset++;

                    if (m_offset + length > m_data_end)
                        throw new FormatAsnException();

                    return length;
                }
                else if (bytes == 2)
                {
                    int b1 = m_buffer[m_offset];
                    int b0 = m_buffer[m_offset + 1];
                    m_offset += 2;

                    int length = (b1 << 8) | b0;

                    if (m_offset + length > m_data_end)
                        throw new FormatAsnException();

                    return length;
                }
                else if (bytes == 3)
                {
                    int b2 = m_buffer[m_offset];
                    int b1 = m_buffer[m_offset + 1];
                    int b0 = m_buffer[m_offset + 2];
                    m_offset += 3;

                    int length = (b2 << 16) | (b1 << 8) | b0;

                    if (m_offset + length > m_data_end)
                        throw new FormatAsnException();

                    return length;
                }
                else if (bytes == 4)
                {
                    int b3 = m_buffer[m_offset];
                    if (b3 >= 128)
                        throw new FormatAsnException("The ASN1 codec does not support the size of content more than 2GB.");

                    int b2 = m_buffer[m_offset + 1];
                    int b1 = m_buffer[m_offset + 2];
                    int b0 = m_buffer[m_offset + 3];
                    m_offset += 4;

                    int length = (b3 << 24) | (b2 << 16) | (b1 << 8) | b0;

                    if (m_offset + length > m_data_end)
                        throw new FormatAsnException();

                    return length;
                }

                throw new FormatAsnException("The ASN1 codec does not support the size of content more than 2GB.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatAsnException("The size of the input buffer is not enough to contain all the Asn1 data.");
            }
        }
    }
}
