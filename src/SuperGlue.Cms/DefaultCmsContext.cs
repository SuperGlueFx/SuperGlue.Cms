using System;
using System.Collections.Generic;
using System.Linq;
using SuperGlue.Cms.Features;

namespace SuperGlue.Cms
{
    public class DefaultCmsContext : ICmsContext
    {
        private readonly IDictionary<Guid, RequestContext> _currentContexts = new Dictionary<Guid, RequestContext>();
        private readonly Func<Type, object> _resolve;
        private readonly IFeatureValidator _featureValidator;

        public DefaultCmsContext(Func<Type, object> resolve, IFeatureValidator featureValidator)
        {
            _resolve = resolve;
            _featureValidator = featureValidator;
        }

        public object Resolve(Type serviceType)
        {
            return _resolve(serviceType);
        }

        public TService Resolve<TService>()
        {
            return (TService)_resolve(typeof(TService));
        }

        public IEnumerable<RequestContext> FindCurrentContexts()
        {
            return _currentContexts.Select(x => x.Value).ToList();
        }

        public RequestContext FindContext(string name)
        {
            return _currentContexts.Where(x => x.Value.Name == name).Select(x => x.Value).FirstOrDefault();
        }

        public RequestContext FirstContextOf(string name, Func<RequestContext, bool> filter = null)
        {
            return FindContexts(name, filter).FirstOrDefault();
        }

        public RequestContext LastContextOf(string name, Func<RequestContext, bool> filter = null)
        {
            return FindContexts(name, filter).LastOrDefault();
        }

        public IEnumerable<RequestContext> FindContexts(string name, Func<RequestContext, bool> filter = null)
        {
            filter = filter ?? (x => true);

            return FindCurrentContexts().Where(x => x.Name == name && filter(x));
        }

        public void EnterContext(Guid id, RequestContext context)
        {
            _currentContexts[id] = context;
        }

        public void ExitContext(Guid id)
        {
            if (_currentContexts.ContainsKey(id))
                _currentContexts.Remove(id);
        }

        public bool HasContext(string name)
        {
            return _currentContexts.Any(x => x.Value.Name == name);
        }

        public IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input)
        {
            return input.Where(x => CanRender(x)).ToList();
        }

        public bool CanRender(object input)
        {
            return HasRequiredContextsFor(input) && IsFeaturesEnabledFor(input);
        }

        private bool IsFeaturesEnabledFor(object input)
        {
            var belongToFeatures = input as IBelongToFeatures;

            return belongToFeatures == null || belongToFeatures.GetFeatures().All(x => _featureValidator.IsActive(x));
        }

        private bool HasRequiredContextsFor(object input)
        {
            var requireContexts = input as IRequireContexts;

            return requireContexts == null || requireContexts.GetRequiredContexts().All(HasContext);
        }
    }
}