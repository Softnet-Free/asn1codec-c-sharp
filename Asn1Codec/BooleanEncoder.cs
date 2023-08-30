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
    class BooleanEncoder : ElementEncoder
    {
        bool m_Value;

        public BooleanEncoder(bool value)
        {
            m_Value = value;
        }

        public bool IsConstructed()
        {
            return false;
        }

        public int EstimateSize()
        {
            return 3;
        }

        public int EncodeTLV(BinaryStack binStack)
        {
            if (m_Value)
            {
                binStack.Stack((byte)0xFF);
                binStack.Stack((byte)1);
                binStack.Stack(UniversalTag.Boolean);
            }
            else
            {
                binStack.Stack((byte)0);
                binStack.Stack((byte)1);
                binStack.Stack(UniversalTag.Boolean);
            }
            return 3;
        }

        public int EncodeLV(BinaryStack binStack)
        {
            if (m_Value)
            {
                binStack.Stack((byte)0xFF);
                binStack.Stack((byte)1);
            }
            else
            {
                binStack.Stack((byte)0);
                binStack.Stack((byte)1);
            }
            return 2;
        }
    }
}
