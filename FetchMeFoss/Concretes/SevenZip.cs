using CommonLibrary;
using FetchMeFoss.Models;
using System.Text.RegularExpressions;

namespace FetchMeFoss.Concretes
{
    // todo 3;
    public class SevenZip : FossInterface
    {
        public SoftwareConfigInfo SoftwareItem { get; set; }
        public Init.Initialization<Configuration> _init { get; set; }
        public Regex RgxCustomVersion { get; set; }

        // Default Constructor
        public SevenZip(SoftwareConfigInfo sci, Init.Initialization<Configuration> initialization)
        { 
            SoftwareItem = sci;
            _init = initialization;
            RgxCustomVersion = new Regex("([0-9][0-9]\\.+[0-9][0-9])", RegexOptions.IgnoreCase);
        }
        // todo 3;
        public string IteratePotentialLinks(string[] pageExecs)
        {
            foreach (string unparsedExec in pageExecs)
            {
                // Assumption that version number is contained within download link
                if (unparsedExec.Contains(SoftwareItem.VersionNo))
                {
                    // todo 4; eventually enable unsecure http?
                    // Finds the file type ending, and the last occurace of https
                    int aSlashIndex = unparsedExec.LastIndexOf("a/");
                    if (aSlashIndex > 0)
                    {
                        int substringLength = unparsedExec.Length - aSlashIndex;
                        string execHref = unparsedExec.Substring(aSlashIndex, substringLength);

                        return execHref + SoftwareItem.FileType;
                    }
                }
            }
            return string.Empty;
        }
        // todo 3;
        public void UpdateSoftwareConfigInfo(string nVersion)
        {
            _init.Logger.Log($"SevenZip-UpdateSoftwareConfigInfo called...");

            nVersion = string.Join("", nVersion.Split('.'));

            // Update the struct with new version data and quit searching
            SoftwareConfigInfo sci = SoftwareItem;
            sci.UriPathToExec = sci.UriPathToExec.Replace(sci.VersionNo, nVersion);
            sci.FileName = sci.FileName.Replace(sci.VersionNo, nVersion);
            sci.VersionNo = nVersion;
            SoftwareItem = sci;
        }
    }
}
