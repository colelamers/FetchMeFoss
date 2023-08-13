using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace FetchMeFoss
{
    public class MainProcessing
    {
        public Initialization<Configuration> Initialization;
        public MainProcessing(Initialization<Configuration> initialization)
        {
            Initialization = initialization;
        }
    }
}
