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
    class GndTimeDecoder
    {
        public static DateTime Decode(byte[] buffer, int offset, int V_length)
        {
            if (V_length < 15 || V_length > 19)
                throw new FormatAsnException();

            int upper_index = offset + V_length - 1;       
     
            int digit = buffer[offset] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            int year = digit * 1000;

            digit = buffer[offset + 1] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            year = year + digit * 100;

            digit = buffer[offset + 2] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            year = year + digit * 10;

            digit = buffer[offset + 3] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            year = year + digit;

            digit = buffer[offset + 4] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            int month = digit * 10;

            digit = buffer[offset + 5] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            month = month + digit;

            digit = buffer[offset + 6] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            int day = digit * 10;

            digit = buffer[offset + 7] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            day = day + digit;

            digit = buffer[offset + 8] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            int hour = digit * 10;

            digit = buffer[offset + 9] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            hour = hour + digit;

            digit = buffer[offset + 10] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            int minute = digit * 10;

            digit = buffer[offset + 11] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            minute = minute + digit;

            digit = buffer[offset + 12] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            int second = digit * 10;

            digit = buffer[offset + 13] - 48;
            if (digit < 0 || digit > 9)
                throw new FormatAsnException();
            second = second + digit;

            int millisecond = 0;

            offset = offset + 14;
            if (buffer[offset] == 46) // '.'
            {
                offset++;
                if(offset >= upper_index)
                    throw new FormatAsnException();

                digit = buffer[offset] - 48;
                if (digit < 0 || digit > 9)
                    throw new FormatAsnException();
                int d1 = digit;

                offset++;
                if (offset < upper_index)
                {
                    digit = buffer[offset] - 48;
                    if (digit < 0 || digit > 9)
                        throw new FormatAsnException();
                    int d2 = digit;

                    offset++;
                    if (offset < upper_index)
                    {
                        digit = buffer[offset] - 48;
                        if (digit < 0 || digit > 9)
                            throw new FormatAsnException();
                        int d3 = digit;
                        offset++;

                        millisecond = d1 * 100 + d2 * 10 + d3;
                    }
                    else
                    {
                        millisecond = d1 * 100 + d2 * 10;
                    }
                }
                else
                {
                    millisecond = d1 * 100;
                }
            }

            if (offset != upper_index)
                throw new FormatAsnException();

            if (buffer[offset] != 90) // 'Z'
                throw new FormatAsnException();

            return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);
        }
    }
}
