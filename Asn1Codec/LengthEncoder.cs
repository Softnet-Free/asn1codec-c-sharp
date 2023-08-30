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
    static class LengthEncoder
    {
        public static int EstimateSize(int length)
        {
            if (length <= 0x0000007f)
            {
                return 1;
            }
            else if (length <= 0x000000ff)
            {
                return 2;
            }
            else if (length <= 0x0000ffff)
            {
                return 3;
            }
            else if (length <= 0x00ffffff)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }

        public static int Encode(int length, BinaryStack binStack)
        {
            if (length <= 0x0000007f)
            {
                binStack.Stack((byte)length);
                return 1;
            }
            else if (length <= 0x000000ff)
            {
                binStack.Stack((byte)length);
                binStack.Stack((byte)(129));

                return 2;
            }
            else if (length <= 0x0000ffff)
            {
                byte b0 = (byte)(length & 0x000000ff);
                byte b1 = (byte)((length >> 8) & 0x000000ff);

                binStack.Stack(b0);
                binStack.Stack(b1);
                binStack.Stack((byte)(130));

                return 3;
            }
            else if (length <= 0x00ffffff)
            {
                byte b0 = (byte)(length & 0x000000ff);
                byte b1 = (byte)((length >> 8) & 0x000000ff);
                byte b2 = (byte)((length >> 16) & 0x000000ff);

                binStack.Stack(b0);
                binStack.Stack(b1);
                binStack.Stack(b2);
                binStack.Stack((byte)(131));

                return 4;
            }
            else
            {
                byte b0 = (byte)(length & 0x000000ff);
                byte b1 = (byte)((length >> 8) & 0x000000ff);
                byte b2 = (byte)((length >> 16) & 0x000000ff);
                byte b3 = (byte)((length >> 24) & 0x000000ff);

                binStack.Stack(b0);
                binStack.Stack(b1);
                binStack.Stack(b2);
                binStack.Stack(b3);
                binStack.Stack((byte)(132));

                return 5;
            }
        }
    }
}
