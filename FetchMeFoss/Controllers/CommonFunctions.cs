using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Controllers
{
    // todo 3;
    public static class CommonFunctions
    {
        // todo 3;
        public static bool ParseStringAsVersionNo(string stringWithVersionNumber)
        {
            bool isPossibleVersion = false;
            string[] versionSplit = stringWithVersionNumber.Split(
                                    ".", StringSplitOptions.RemoveEmptyEntries);

            // Verify all split values are numbers, if not, it's not a VerisonNo
            foreach (string number in versionSplit)
            {
                int isNum;
                if (int.TryParse(number, out isNum))
                {
                    isPossibleVersion = true;
                }
                else
                {
                    isPossibleVersion = false;
                    break;
                }
            }
            return isPossibleVersion;
        }
    }
}
