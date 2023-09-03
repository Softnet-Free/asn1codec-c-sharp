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
    class TimpEncoder : ElementEncoder
    {
        private const int C_ContextSpecific_Constructed = 0xA0;
        private const int C_Application_Constructed = 0x60;
        private const int C_Private_Constructed = 0xE0;
        private const int C_ContextSpecific_Primitive = 0x80;
        private const int C_Application_Primitive = 0x40;
        private const int C_Private_Primitive = 0xC0;

        TagClass m_TagClass;
        int m_Tag;
        ElementEncoder m_ElementEncoder;

        public TimpEncoder(int tag, TagClass tc, ElementEncoder elementEncoder)
        {
            if (tag < 0 || tag > 30)
                throw new ArgumentException("The tag value must be in the range between 0 and 30.");
            m_Tag = tag;
            m_TagClass = tc;
            m_ElementEncoder = elementEncoder;
        }

        public bool IsConstructed()
        {
            return m_ElementEncoder.IsConstructed();
        }

        public int EstimateSize()
        {
            return m_ElementEncoder.EstimateSize();
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            int LV_length = m_ElementEncoder.EncodeLV(binStack);

            if (m_ElementEncoder.IsConstructed() == false)
            {
                if (m_TagClass == TagClass.ContextSpecific)
                    binStack.Stack(C_ContextSpecific_Primitive | m_Tag);
                else if (m_TagClass == TagClass.Application)
                    binStack.Stack(C_Application_Primitive | m_Tag);
                else // m_TagClass == TagClass.Private
                    binStack.Stack(C_Private_Primitive | m_Tag);
            }
            else
            {
                if (m_TagClass == TagClass.ContextSpecific)
                    binStack.Stack(C_ContextSpecific_Constructed | m_Tag);
                else if (m_TagClass == TagClass.Application)
                    binStack.Stack(C_Application_Constructed | m_Tag);
                else // m_TagClass == TagClass.Private
                    binStack.Stack(C_Private_Constructed | m_Tag);
            }
            return 1 + LV_length;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            return m_ElementEncoder.EncodeLV(binStack);
        }
    }
}
