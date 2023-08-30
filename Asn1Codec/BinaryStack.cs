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

namespace Softnet.Asn
{
    class BinaryStack
    {
        byte[] m_buffer = null;
        int m_position = 0;

        public byte[] Buffer
        {
            get { return m_buffer; }
        }

        public int Position
        {
            get { return m_position; }
        }

        public int Count
        {
            get { return m_buffer.Length - m_position; }
        }

        public void Allocate(int memorySize)
        {
            m_buffer = new byte[memorySize];
            m_position = memorySize;
        }

        public void Stack(byte value)
        {
            m_position -= 1;
            m_buffer[m_position] = value;
        }

        public void Stack(int byteValue)
        {
            m_position -= 1;
            m_buffer[m_position] = (byte)byteValue;
        }

        public void Stack(byte[] data)
        {
            m_position -= data.Length;
            System.Buffer.BlockCopy(data, 0, m_buffer, m_position, data.Length);
        }

        public void Stack(byte[] data, int offset, int size)
        {
            m_position -= size;
            System.Buffer.BlockCopy(data, offset, m_buffer, m_position, size);
        }        
    }
}
