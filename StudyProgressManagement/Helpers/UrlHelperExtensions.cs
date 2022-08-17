using System.IO;
using System.Web.Caching;

namespace System.Web.Mvc
{
    public static class UrlExtensions
    {
        private const string FileDateTicksCacheKeyFormat = "FileDateTicks_{0}";

        private static long GetFileDateTicks(this UrlHelper urlHelper, string filename)
        {
            var context = urlHelper.RequestContext.HttpContext;
            string cacheKey = string.Format(FileDateTicksCacheKeyFormat, filename);

            // Check if we already cached the ticks in the cache.
            if (context.Cache[cacheKey] != null)
            {
                return (long)context.Cache[cacheKey];
            }

            var physicalPath = context.Server.MapPath(filename);
            var fileInfo = new FileInfo(physicalPath);
            var dependency = new CacheDependency(physicalPath);

            // If file exists, read number of ticks from last write date. Or fall back to 0.
            long ticks = fileInfo.Exists ? fileInfo.LastWriteTime.Ticks : 0;

            // Add the number of ticks to cache for 12 hours.
            // The cache dependency will remove the entry if file is changed or deleted.
            context.Cache.Add(cacheKey, ticks, dependency,
                DateTime.Now.AddHours(12), TimeSpan.Zero,
                CacheItemPriority.Normal, null);

            return ticks;
        }

        public static string ContentVersioned(this UrlHelper urlHelper, string contentPath)
        {
            string url = urlHelper.Content(contentPath);

            long fileTicks = GetFileDateTicks(urlHelper, url);

            return $"{url}?v={fileTicks}";
        }
    }
}