using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skullquestia
{
    class BotInfo
    {
            // Bot info for all configs
        public const char command_prefix = '$';
#if DEBUG   // Bot info for Debug config
        public const string bot_token = "MzcyNzQ4NTY3ODAwMDUzNzcz.DNeaSg.KbEHlNmcs3FQuoHNGjD9i0lz8OI";
#else       // Bot info for Release config
        public const string bot_token = "MzcyNzQ4NTY3ODAwMDUzNzcz.DNeaSg.KbEHlNmcs3FQuoHNGjD9i0lz8OI";
#endif
    }
}
