using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace SuperGlue.Cms.Localization
{
    public class LocalizationSettings
    {
        public Func<IDictionary<string, object>, CultureInfo> FindCulture { get; private set; }

        public void FindCultureUsing(Func<IDictionary<string, object>, CultureInfo> findCulture)
        {
            FindCulture = findCulture;
        }

        internal CultureInfo GetCulture(IDictionary<string, object> environment)
        {
            return (FindCulture ?? (x => Thread.CurrentThread.CurrentCulture))(environment);
        }
    }
}