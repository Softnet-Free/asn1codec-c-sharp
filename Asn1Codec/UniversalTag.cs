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
    class UniversalTag
    {
        public const byte Boolean = 1;
	    public const byte Integer = 2;
	    public const byte OctetString = 4;
        public const byte Null = 5;
	    public const byte UTF8String = 12;           
	    public const byte Sequence = 16;
	    public const byte PrintableString = 19; // A-Z a-z 0-9 space ' ( ) + , - . / : = ?
        public const byte IA5String = 22;		// ASCII [0 - 127]
	    public const byte GeneralizedTime = 24;
        public const byte BMPString = 30;		// Unicode (Big-Endian UTF-16)
    }
}
