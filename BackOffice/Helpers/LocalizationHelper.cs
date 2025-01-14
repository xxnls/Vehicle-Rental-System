using System.Globalization;
using System.Resources;

namespace BackOffice.Helpers
{
    public class LocalizationHelper
    {
        // The base namespace for all resources
        private const string ResourceBaseNamespace = "BackOffice.Resources.";

        /// <summary>
        /// Gets a string from a resource file based on the current UI culture
        /// </summary>
        /// <param name="folder">
        /// The folder where the resource file is located.
        /// </param>
        /// <param name="key">
        /// The key of the string to fetch from the resource file.
        /// </param>
        /// <returns>
        /// The string from the resource file based on the current UI culture.
        /// </returns>
        public static string GetString(string folder, string key)
        {
            var resourceName = $"{ResourceBaseNamespace}{folder}.Resources";
            var resourceManager = new ResourceManager(resourceName, typeof(LocalizationHelper).Assembly);

            return resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }
    }
}
