using FetchMeFoss.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Models
{
    public class FossDataConstants
    {
        public static Dictionary<string, Type> FossItemType = new Dictionary<string, Type>
        {
            {"krita", typeof(Krita) },
            // todo 1; update as you add new interface implementers
        };

    }
}
