using System.Globalization;
using FubuLocalization;
using FubuLocalization.Basic;

namespace ProductsManagement.Infrastructure
{
    public class LocaleCacheFactory : ILocaleCacheFactory
    {
        private readonly CultureInfo _defaultCulture;
        private readonly ILocalizationCache _cache;
        private readonly ILocalizationStorage _storage;

        public LocaleCacheFactory(CultureInfo defaultCulture, ILocalizationStorage storage, ILocalizationCache cache)
        {
            _defaultCulture = defaultCulture;
            _storage = storage;
            _cache = cache;
        }

        public ILocaleCache CacheFor(CultureInfo culture)
        {
            var cache = _cache.CacheFor(culture, () => _storage.Load(culture));
            return cache;
        }

        public ILocaleCache GetDefault()
        {
            return CacheFor(_defaultCulture);
        }
    }

    public interface ILocaleCacheFactory
    {
        ILocaleCache CacheFor(CultureInfo culture);
        ILocaleCache GetDefault();
    }
}