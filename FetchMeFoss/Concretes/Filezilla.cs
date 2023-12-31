﻿using CommonLibrary;
using FetchMeFoss.Models;
using System.Text.RegularExpressions;

namespace FetchMeFoss.Concretes
{
    public class Filezilla : FossInterface
    { 
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }
        public Regex RgxCustomVersion { get; set; }

        // Default Constructor
        public Filezilla(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization)
        {
            SoftwareItem = sci;
            _init = initialization;
        }
        // todo 1; need to find last one in list cause it's an open directory
    }
}
