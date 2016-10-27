using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BitBucketGlyph
{
    public class BitBucketFileCorrelator
    {
        public const string GIT_EXTENSION = @".git";
        //Prefer a remote named "origin" given multiple options
        public const string PREFERRED_REMOTE = "origin";
        public const string BITBUCKET_REPO_PATH_PREFIX = "scm/";

        private BitBucketAvailableRepositoryFactory Client;
        private string Host;
        private string PreferredRemote;

        public BitBucketFileCorrelator(string bitbucketServerUrl, string preferredRemote = PREFERRED_REMOTE)
        {
            Client = new BitBucketAvailableRepositoryFactory(bitbucketServerUrl);
            Host = new Uri(bitbucketServerUrl).Host;
            PreferredRemote = preferredRemote;
        }

        public bool TryGetUriForFile(string filePath, out Uri bitBucketUri)
        {
            var analyzer = new GitSourceAnalyzer(filePath);
            Dictionary<string, Uri> possibleOrigins = analyzer.PossibleOrigins.Where(kvp => kvp.Value.Host == Host).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (possibleOrigins.Count > 0)
            {
                Uri origin;

                if (!possibleOrigins.TryGetValue(PreferredRemote, out origin))
                {
                    origin = possibleOrigins.First().Value;
                }

                var projectAndRepo = origin.AbsolutePath.Trim('/').Replace(BITBUCKET_REPO_PATH_PREFIX, string.Empty).Split('/');
                var remote = Client.Manufacture(projectAndRepo[0]).FirstOrDefault(r => r.Name + GIT_EXTENSION == projectAndRepo[1]);
                if (remote != null)
                {
                    var remoteUri = remote.Uris.FirstOrDefault(u => u.Scheme.StartsWith("http"));
                    if (remoteUri != null)
                    {
                        bitBucketUri = AddToPath(remoteUri, analyzer.RepoRelativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Where(p => !string.IsNullOrEmpty(p)));
                        return true;
                    }
                }
            }

            bitBucketUri = null;
            return false;
        }

        private static Uri AddToPath(Uri remoteUri, IEnumerable<string> pieces)
        {
            var builder = new UriBuilder(remoteUri);
            foreach (var piece in pieces)
            {
                if (!builder.Path.EndsWith("/"))
                    builder.Path += "/";
                builder.Path += piece;
            }
            return builder.Uri;
        }
    }
}
