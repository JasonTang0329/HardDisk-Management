using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISFCU.Extention
{
    /// <summary></summary>
    public struct Base64String
    {
        private string value;

        /// <summary></summary>
        /// <param name="value"></param>
        public Base64String(string value)  //constructor
        {
            this.value = value;
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public implicit operator Base64String(string value)
        {
            return new Base64String(value);
        }

        /// <summary></summary>
        /// <param name="Base64"></param>
        /// <returns></returns>
        static public implicit operator string(Base64String Base64)
        {
            return Base64.value;
        }
    }
}
