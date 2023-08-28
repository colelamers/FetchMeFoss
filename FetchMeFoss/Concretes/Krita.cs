using CommonLibrary;
using FetchMeFoss.Models;
using System.Text.RegularExpressions;

namespace FetchMeFoss.Concretes
{
    // todo 3;
    public class Krita : FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }
        public Regex RgxCustomVersion { get; set; }
        public Krita(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization) 
        { 
            SoftwareItem = sci;
            _init = initialization;
            RgxCustomVersion = new Regex("([0-9]+[0-9]?[0-9]?[0-9]?\\.+" +
                                         "[0-9]+[0-9]?[0-9]?[0-9]?\\." +
                                         "[0-9]?[0-9]?[0-9]?[0-9]?" +
                                         "(?=[.][0-9]*))", 
                                          RegexOptions.IgnoreCase);
        }
        // todo 3;
 
    }
}
