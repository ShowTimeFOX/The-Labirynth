using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLabirynth
{
    public static class EnumExtensions
    {
        public static EDirection Next(this EDirection current)
        {
            var values = Enum.GetValues(typeof(EDirection));
            int index = (Array.IndexOf(values, current) + 1) % values.Length;
            return (EDirection)values.GetValue(index);
        }

        public static EDirection Previous(this EDirection current)
        {
            var values = Enum.GetValues(typeof(EDirection));
            int index = (Array.IndexOf(values, current) - 1 + values.Length) % values.Length;
            return (EDirection)values.GetValue(index);
        }
    }
}
