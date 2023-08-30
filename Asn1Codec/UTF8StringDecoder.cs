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
using System.Text;

namespace Softnet.Asn
{
    class UTF8StringDecoder
    {
        public static string Decode(byte[] buffer, int offset, int V_length)
        {
            try
            {
                if (V_length == 0)
                    return string.Empty;
                return UTF8Encoding.UTF8.GetString(buffer, offset, V_length);
            }
            catch (ArgumentException ex)
            {
                throw new FormatAsnException(ex.Message);
            }
        }
    }
}
