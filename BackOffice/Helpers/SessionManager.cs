using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Helpers
{
    public static class SessionManager
    {
        private static readonly Dictionary<string, object> _sessionData = new Dictionary<string, object>();

        /// <summary>
        /// Adds or updates a session value.
        /// </summary>
        /// <param name="key">The key to identify the session data.</param>
        /// <param name="value">The value to store in the session.</param>
        public static void Set(string key, object value)
        {
            if (_sessionData.ContainsKey(key))
            {
                _sessionData[key] = value;
            }
            else
            {
                _sessionData.Add(key, value);
            }
        }

        /// <summary>
        /// Retrieves a session value by key.
        /// </summary>
        /// <param name="key">The key of the session data to retrieve.</param>
        /// <returns>The session value if it exists; otherwise, <c>null</c>.</returns>
        public static object? Get(string key)
        {
            return _sessionData.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Removes a session value by key.
        /// </summary>
        /// <param name="key">The key of the session data to remove.</param>
        public static void Remove(string key)
        {
            if (_sessionData.ContainsKey(key))
            {
                _sessionData.Remove(key);
            }
        }

        /// <summary>
        /// Clears all session data.
        /// </summary>
        public static void Clear()
        {
            _sessionData.Clear();
        }
    }

}
