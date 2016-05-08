using System.Collections.Generic;
using System.Linq;

namespace SuperGlue.Cms.Features
{
    public class FeatureSettings
    {
        public FeatureSettings()
        {
            Features = new List<Feature>();
        }

        public ICollection<Feature> Features { get; set; }

        public bool IsActive(string feature)
        {
            var parts = new Queue<string>(feature.Split('/'));
            Feature currentFeature = null;

            while (parts.Any())
            {
                var currentPart = parts.Dequeue();

                currentFeature = currentFeature == null
                    ? Features.FirstOrDefault(x => x.Name == currentPart)
                    : currentFeature.Children.FirstOrDefault(x => x.Name == currentPart);

                if (currentFeature == null || !currentFeature.Active)
                    return false;
            }

            return true;
        }

        public Feature Get(string feature)
        {
            var parts = new Queue<string>(feature.Split('/'));
            Feature currentFeature = null;

            while (parts.Any())
            {
                var currentPart = parts.Dequeue();

                currentFeature = currentFeature == null
                    ? Features.FirstOrDefault(x => x.Name == currentPart)
                    : currentFeature.Children.FirstOrDefault(x => x.Name == currentPart);

                if (currentFeature == null)
                    return null;
            }

            return currentFeature;
        }

        public class Feature
        {
            public Feature()
            {
                Children = new List<Feature>();
            }

            public string Name { get; set; }
            public ICollection<Feature> Children { get; set; }
            public bool Active { get; set; }
        }
    }
}