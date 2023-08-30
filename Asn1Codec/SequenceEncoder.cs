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
    public interface SequenceEncoder
    {
        int Count { get; }

        SequenceEncoder Sequence();
        SequenceEncoder Sequence(int tag);
        
        void Int32(int value);
        void Int32(int tag, int value);

	    void Int64(long value);
        void Int64(int tag, long value);

        void Boolean(bool value);
        void Boolean(int tag, bool value);

        void GndTime(DateTime value);
        void GndTime(int tag, DateTime value);

        void OctetString(byte[] buffer);
        void OctetString(int tag, byte[] buffer);

        void OctetString(byte[] buffer, int offset, int length);
        void OctetString(int tag, byte[] buffer, int offset, int length);

        void UTF8String(string value);
        void UTF8String(int tag, string value);

        void BMPString(string value);
        void BMPString(int tag, string value);
        
        void IA5String(string value);
        void IA5String(int tag, string value);
        
        void PrintableString(string value);
        void PrintableString(int tag, string value);

        void Null();
        void Null(int tag);
    }
}
