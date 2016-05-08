using System;
using System.IO;

namespace SuperGlue.Cms.Views
{
    public class CmsView
    {
        private readonly Action<TextWriter, string> _render;

        public CmsView(Action<TextWriter, string> render)
        {
            _render = render;
        }

        public void Render(TextWriter renderTo, string contentType)
        {
            _render(renderTo, contentType);
        }
    }
}