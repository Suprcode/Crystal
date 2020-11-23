using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;

namespace Client.Utils
{
    public static class SharpDXHelper
    {
        public static RawColorBGRA ToRawColorBGRA(this Color color)
        {
            return new RawColorBGRA(color.B, color.G, color.R, color.A);
        }
    }
}
