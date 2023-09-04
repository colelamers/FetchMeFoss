﻿using CommonLibrary;
using FetchMeFoss.Models;
using System.Text.RegularExpressions;

namespace FetchMeFoss.Concretes
{
    public class Okular : FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }
        public Regex RgxCustomVersion { get; set; }

        // Default Constructor
        public Okular(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization)
        {
            SoftwareItem = sci;
            _init = initialization;
        }
    }
}
