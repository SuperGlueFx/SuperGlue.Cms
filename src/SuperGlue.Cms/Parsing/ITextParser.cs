﻿using System;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public interface ITextParser
    {
        Task<string> Parse(string text, ICmsRenderer cmsRenderer, Func<string, Task<string>> recurse);
    }
}