using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBucketGlyph
{
    public class AvailableRepository
    {
        public AvailableRepository(string name, IEnumerable<Uri> uris)
        {
            Name = name;
            Uris = uris;
        }

        public string Name { get; private set; }
        public IEnumerable<Uri> Uris { get; private set; }
    }
}
