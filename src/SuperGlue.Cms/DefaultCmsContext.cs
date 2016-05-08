using System;
using System.Collections.Generic;
using System.Linq;
using SuperGlue.Cms.Features;

namespace SuperGlue.Cms
{
    public class DefaultCmsContext : ICmsContext
    {
        private readonly IDictionary<Guid, RequestContext> _currentContexts = new Dictionary<Guid, RequestContext>();
        private readonly IFeatureValidator _featureValidator;

        public DefaultCmsContext(IFeatureValidator featureValidator)
        {
            _featureValidator = featureValidator;
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

        public bool HasContext(string name)
        {
            return _currentContexts.Any(x => x.Value.Name == name);
        }

        public IDisposable EnterContexts(params RequestContext[] contexts)
        {
            var contextDictionary = contexts.ToDictionary(x => Guid.NewGuid(), x => x);
            foreach (var context in contextDictionary)
                EnterContext(context.Key, context.Value);

            return new ContextManager(() =>
            {
                foreach (var context in contextDictionary)
                    ExitContext(context.Key);
            });
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

        private void EnterContext(Guid id, RequestContext context)
        {
            _currentContexts[id] = context;
        }

        private void ExitContext(Guid id)
        {
            if (_currentContexts.ContainsKey(id))
                _currentContexts.Remove(id);
        }

        public class ContextManager : IDisposable
        {
            private readonly Action _done;

            public ContextManager(Action done)
            {
                _done = done;
            }

            public void Dispose()
            {
                _done();
            }
        }
    }
}