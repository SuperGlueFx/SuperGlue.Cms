
using System;
using System.Collections.Generic;

namespace SuperGlue.Cms.Parsing
{
    public interface ISetupTextParser
    {
        void DependsOn(Func<IDictionary<string, object>, object> func);
    }
}