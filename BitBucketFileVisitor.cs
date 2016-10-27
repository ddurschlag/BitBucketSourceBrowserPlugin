using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SourceBrowser.MEF;

namespace BitBucketGlyph
{
    public class BitBucketFileVisitor : ITextVisitor
    {
        private BitBucketFileCorrelator Correlator;

        public BitBucketFileVisitor(string url)
        {
            Correlator = new BitBucketFileCorrelator(url);
        }

        public BitBucketFileVisitor(string url, string preferredRemote)
        {
            Correlator = new BitBucketFileCorrelator(url, preferredRemote);
        }

        public string Visit(string text, IReadOnlyDictionary<string, string> context)
        {
            if ( context[ContextKeys.LineNumber] == "1")
            {
                Uri maybeUri;
                if ( Correlator.TryGetUriForFile(context[ContextKeys.FilePath], out maybeUri) )
                {
                    return string.Format(
                        @"<a target='_top' href='{0}'><svg xmlns='http://www.w3.org/2000/svg' height='16' viewBox='1 16 31 35' width='16'> <title>Bitbucket logo</title>   <g fill='#205081'> <path d='M14.54,16.44h0C6.53,16.44,0,18.59,0,21.26,0,22,1.74,32.06,2.44,36.06c.31,1.79,4.95,4.42,12.09,4.42v0c7.15,0,11.78-2.63,12.09-4.42.69-4,2.44-14.09,2.44-14.79C29.07,18.59,22.54,16.44,14.54,16.44Zm0,20.8a4.62,4.62,0,1,1,4.62-4.62A4.62,4.62,0,0,1,14.54,37.24Zm0-14.49c-5.14,0-9.31-.9-9.31-2s4.17-2,9.31-2,9.31.9,9.31,2S19.67,22.76,14.53,22.75Z'></path> <path d='M25,40.07a.68.68,0,0,0-.4.16s-3.58,2.83-10,2.83-10-2.83-10-2.83a.68.68,0,0,0-.4-.16.51.51,0,0,0-.51.57.67.67,0,0,0,0,.12c.56,3,1,5.08,1,5.4C5.1,48.35,9.38,50,14.54,50h0c5.15,0,9.43-1.65,9.92-3.84.07-.32.48-2.43,1-5.4a.67.67,0,0,0,0-.12A.51.51,0,0,0,25,40.07Z'></path> <circle cx='14.53' cy='32.62' r='2.32'></circle></g> </svg></a>",
                        maybeUri
                    );
                }
            }
            return null;
        }
    }
}
