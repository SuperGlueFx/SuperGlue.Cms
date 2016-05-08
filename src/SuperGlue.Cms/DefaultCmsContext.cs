using System.Collections.Generic;
using System.Linq;
using SuperGlue.Cms.Features;

namespace SuperGlue.Cms
{
    public class DefaultCmsContext : ICmsContext
    {
        private readonly IFeatureValidator _featureValidator;

        public DefaultCmsContext(IFeatureValidator featureValidator)
        {
            _featureValidator = featureValidator;
        }

    
        public IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input)
        {
            return input.Where(x => CanRender(x)).ToList();
        }

        public bool CanRender(object input)
        {
            return IsFeaturesEnabledFor(input);
        }

        private bool IsFeaturesEnabledFor(object input)
        {
            var belongToFeatures = input as IBelongToFeatures;

            return belongToFeatures == null || belongToFeatures.GetFeatures().All(x => _featureValidator.IsActive(x));
        }
    }
}