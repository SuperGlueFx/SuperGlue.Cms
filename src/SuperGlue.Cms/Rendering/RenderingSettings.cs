using System;

namespace SuperGlue.Cms.Rendering
{
	public class RenderingSettings
	{
		public TimeSpan CompiliationCacheTimeout { get; private set; }

		public void CacheCompilationsFor(TimeSpan length)
		{
			CompiliationCacheTimeout = length;
		}
	}
}