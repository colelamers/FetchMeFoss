using CommonLibrary;
using FetchMeFoss.Models;
using System.Text.RegularExpressions;

namespace FetchMeFoss.Concretes
{
    // todo 3;
    public class Git : FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }
        public Regex RgxCustomVersion { get; set; }

        public Git(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization) 
        { 
            SoftwareItem = sci;
            _init = initialization;
            RgxCustomVersion = new Regex("([0-9]?[0-9]?[0-9]?[0-9]?" +
                                         "\\.[0-9]?[0-9]?[0-9]?[0-9]?" +
                                         "\\.?[0-9]?[0-9]?[0-9]?[0-9]?" +
                                         "\\.?[A-z]*" +
                                         "\\.?[0-9]?[0-9]?[0-9]?[0-9]?[/])",
                                         RegexOptions.IgnoreCase);
        }
        // todo 3;
        public void UpdateSoftwareConfigInfo(string nVersion)
        {
            _init.Logger.Log($"Git-UpdateSoftwareConfigInfo called...");

            SoftwareConfigInfo sci = SoftwareItem;

            string windows = ".windows";
            string gitForWinVersionDirName = this.RgxCustomVersion.Split(sci.UriPathToExec)[1];
            // Update the struct with new version data and quit searching
            sci.UriPathToExec = sci.UriPathToExec.Replace(gitForWinVersionDirName, nVersion);
            // todo 4; limitiation in current code, defaulting to first hotfix version
            //         should resolve some day, but v1 should guarantee a version
            sci.UriPathToExec = sci.UriPathToExec + windows + ".1/";
            sci.FileName = sci.FileName.Replace(sci.VersionNo, nVersion);
            sci.VersionNo = nVersion;
            SoftwareItem = sci;
        }
    }
}
