using CommonLibrary;
using FetchMeFoss.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Concretes
{
    public class Krita : FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }

        public Krita(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization) 
        { 
            SoftwareItem = sci;
            _init = initialization;
        }
    }
}
