using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SourceBrowser.MEF;
using System.ComponentModel.Composition;

namespace BitBucketGlyph
{
    [Export(typeof(ISourceBrowserPlugin))]
    [ExportMetadata("Name", "BitBucket")]
    public class BitBucketSourceBrowserPlugin : ISourceBrowserPlugin
    {
        private string BitBucketUrl;
        private BitBucketFileVisitor Visitor;

        public void Init(Dictionary<string, string> configuration, ILog logger)
        {
            if (!configuration.TryGetValue("BitBucketUrl", out BitBucketUrl))
            {
                Visitor = null;
                logger.Error("BitBucketUrl not configured; cannot create BitBucket links");
            }
            else
            {
                string preferredRemote;
                if (configuration.TryGetValue("PreferredRemote", out preferredRemote))
                {
                    Visitor = new BitBucketFileVisitor(BitBucketUrl, preferredRemote);
                }
                else
                {
                    Visitor = new BitBucketFileVisitor(BitBucketUrl);
                }
            }
        }

        public IEnumerable<ISymbolVisitor> ManufactureSymbolVisitors(string projectPath)
        {
            yield break;
        }

        public IEnumerable<ITextVisitor> ManufactureTextVisitors(string projectPath)
        {
            if (Visitor != null)
                yield return Visitor;
        }
    }
}
