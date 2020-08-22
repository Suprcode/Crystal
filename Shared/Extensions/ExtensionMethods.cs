using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Extensions
{
    public static class HelperExtensions
    {
        public static T ValueOrDefault<T>(this object value)
        {
            if (value == null || value.ToString() == "")
            {
                return default;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
