using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib10
{
    public class ManufacturerComparer : IComparer<Engine>
    {
        public int Compare(Engine x, Engine y)
        {
            if (x == null || y == null) return 0;
            return string.Compare(x.Model, y.Model, StringComparison.Ordinal);
        }
    }
}