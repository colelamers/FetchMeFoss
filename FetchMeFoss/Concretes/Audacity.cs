﻿using FetchMeFoss.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchMeFoss.Concretes
{
    public class Audacity : FossActions
    {
        public SoftwareConfigInfo SoftwareItem { get; private set; }
        public Audacity(SoftwareConfigInfo si)
        {
            SoftwareItem = si;
        }

        public async Task<string> ParseHtmlForDownloadLink(Uri siteLink)
        {
            string fullExecLink = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                string rawHtml = await client.GetStringAsync(siteLink);
                string[] pageExecs = rawHtml.Split(new string[] { ".exe" }, StringSplitOptions.None);
                foreach (string unparsedExec in pageExecs)
                {
                    string appendExe = ".exe";
                    int httpsIndex = unparsedExec.LastIndexOf("https://");
                    if (httpsIndex > 0)
                    {
                        int substringLength = unparsedExec.Length - httpsIndex;
                        string executableHref = unparsedExec.Substring(httpsIndex, substringLength);
                        fullExecLink = executableHref + appendExe;
                    }
                }
            }
            return fullExecLink;
        }
    }
}