using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ray.Blog.Blazor.Helpers
{
    public class BlazorColorHelper
    {
        public static Color GetRandomColor()
        {
            Color[] enums = Enum.GetValues(typeof(Color)) as Color[];
            return enums[new Random().Next(0, enums.Length)];
        }
    }
}
