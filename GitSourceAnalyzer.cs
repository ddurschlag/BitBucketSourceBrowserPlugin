using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace BitBucketGlyph
{

    public class GitSourceAnalyzer
    {
        public const string GIT_DIR = @".git";

        public GitSourceAnalyzer(string path)
        {
            path = GetProperFilePathCapitalization(Path.GetFullPath(path));
            var workdir = Repository.Discover(path);
            RepoRelativePath = path.Replace(workdir.Replace(string.Concat(Path.DirectorySeparatorChar, GIT_DIR), string.Empty), string.Empty);

            using (var repo = new Repository(workdir))
            {
                PossibleOrigins = repo.Network.Remotes.Select(r => Tuple.Create(r, new Uri(r.Url))).ToDictionary(t => t.Item1.Name, t => t.Item2);
            }
        }

        public string RepoRelativePath { get; private set; }

        public IReadOnlyDictionary<string, Uri> PossibleOrigins { get; private set; }

        private static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            DirectoryInfo parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
                return dirInfo.Name;
            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }

        private static string GetProperFilePathCapitalization(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            DirectoryInfo dirInfo = fileInfo.Directory;
            return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                                dirInfo.GetFiles(fileInfo.Name)[0].Name);
        }
    }
}
