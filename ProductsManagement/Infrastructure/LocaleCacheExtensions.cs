using System;
using System.Linq.Expressions;
using FubuLocalization;

namespace ProductsManagement.Infrastructure
{
    public static class LocaleCacheExtensions
    {
        public static void Append(this ILocaleCache cache, StringToken token, string value)
        {
            cache.Append(token.ToLocalizationKey(), value);
        }

        public static void Append<T>(this ILocaleCache cache, Expression<Func<T, object>> property, string value)
        {
            cache.Append(new LocalizationKey(PropertyToken.For(property).StringTokenKey), value);
        }
    }
}