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
    public interface SequenceDecoder
    {
        bool Exists(UType type);
        bool Exists(int tag);
        bool HasNext();

        void End();
        int Count();

        bool IsNull();
        SequenceDecoder Sequence();
        int Int32();
        int Int32(int minValue, int maxValue);
        long Int64();
        bool Boolean();
        string UTF8String();
        string UTF8String(int requiredLength);
        string UTF8String(int minLength, int maxLength);
        DateTime GndTime(); 
        string BMPString();
        string BMPString(int requiredLength);
        string BMPString(int minLength, int maxLength);
        string IA5String();
        string IA5String(int requiredLength);
        string IA5String(int minLength, int maxLength);
        string PrintableString();
        string PrintableString(int requiredLength);
        string PrintableString(int minLength, int maxLength);
        byte[] OctetString();
        byte[] OctetString(int requiredLength);
        byte[] OctetString(int minLength, int maxLength);
    }
}
