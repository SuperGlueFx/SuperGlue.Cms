﻿using System;
using System.Collections.Generic;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public interface ITextParser
    {
        string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, Func<string, string> recurse);
        IEnumerable<string> GetTags();
    }
}