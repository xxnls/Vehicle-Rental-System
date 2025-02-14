using Microsoft.Extensions.Localization;
using System.Reflection;

namespace API.Resources
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IStringLocalizer _localizer;

        public LocalizationService(IStringLocalizerFactory localizerFactory)
        {
            var type = typeof(Documents); // Ensure this is generated from the .resx file
            _localizer = localizerFactory.Create(type);
        }

        public string GetString(string key)
        {
            return _localizer[key].Value;
        }
    }
    public interface ILocalizationService
    {
        string GetString(string key);
    }
}
