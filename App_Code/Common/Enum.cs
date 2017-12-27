using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISFCU.Extention
{
    /// <summary>編碼類型</summary>
    public enum EnumEncoding : int
    {
        /// <summary>預設</summary>
        DefaultValue = 0,
        /// <summary>UTF8</summary>
        UTF8 = 1,
        /// <summary>Unicode</summary>
        Unicode = 2,
        /// <summary>UTF32</summary>
        UTF32 = 3
    };
}
