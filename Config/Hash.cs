using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domotica_API.Config
{
    public class Hash
    {
        //The sizes are given in Bits.
        public const int PASSWORD_HASH_SIZE = 512;
        public const int PASSWORD_HASH_SALT_SIZE = 256;

        public const int PASSWORD_HASH_ITERATIONS = 16000;
    }
}
