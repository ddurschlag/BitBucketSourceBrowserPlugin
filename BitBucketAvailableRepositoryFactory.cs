using System.Collections.Generic;
using System.Linq;
using Atlassian.Stash;
using Atlassian.Stash.Helpers;
using BB = Atlassian.Stash.Entities;

namespace BitBucketGlyph
{
    public class BitBucketAvailableRepositoryFactory
    {
        private StashClient Client;

        public BitBucketAvailableRepositoryFactory(string bitbucketServerUrl)
        {
            Client = new StashClient(bitbucketServerUrl);
        }

        private IEnumerable<BB.Repository> GetRepos(string project)
        {
            var resp = Client.Repositories.Get(project).Result;
            foreach (var repo in resp.Values)
                yield return repo;
            while (!resp.IsLastPage)
            {
                resp = Client.Repositories.Get(project, new RequestOptions { Start = resp.NextPageStart }).Result;
                foreach (var repo in resp.Values)
                    yield return repo;
            }
        }

        public IEnumerable<AvailableRepository> Manufacture(string project)
        {
            //This will notably hold onto the response from BitBucket to lazily evaluate the links.
            //This could be prevented by adding ToList() and casting back into IEnumerable (IReadOnlyDictionary
            //is not covariant), but it's a bit ugly, and it's not clear that the memory cost of the response
            //is worse than the performance cost of converting all the links up front.
            return GetRepos(project).Select(r => new AvailableRepository(r.Slug, r.Links.Self.Select(self => self.Href)));
        }
    }
}
