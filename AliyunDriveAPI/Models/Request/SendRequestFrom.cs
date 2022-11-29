using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace AliyunDriveAPI.Models.Request
{
    public class SendRequestFrom
    {
        private Dictionary<string, string> _params;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">The initial object</param>
        public SendRequestFrom(SendRequestFrom request = null)
        {
            if (request != null)
            {
                _params = new Dictionary<string, string>(request._params);
            }
            else
            {
                _params = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Add URL escape property and value pair
        /// </summary>
        /// <param name="property">property</param>
        /// <param name="value">value</param>
        /// <returns>Current object</returns>
        public SendRequestFrom AddParam(string property, string value)
        {
            if ((property != null) && (value != null))
            {
                _params.Add(Uri.EscapeDataString(property), Uri.EscapeDataString(value));
            }

            return this;
        }

        public SendRequestFrom AddParam(string property, object value)
        {
            if ((property != null) && (value != null))
            {
                _params.Add(Uri.EscapeDataString(property), Uri.EscapeDataString(value.ToString()));
            }

            return this;
        }

        /// <summary>
        /// Add and merge another object
        /// </summary>
        /// <param name="request">The object that want to add</param>
        /// <returns>Current object</returns>
        public SendRequestFrom AddParam(SendRequestFrom request)
        {
            _params.Concat(request._params);

            return this;
        }

        /// <summary>
        /// Concat the property and value pair
        /// </summary>
        /// <returns>string</returns>
        public string BuildParams()
        {
            if (_params.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var para in _params.OrderBy(i => i.Key, StringComparer.Ordinal))
            {
                sb.Append('&');
                sb.Append(para.Key).Append('=').Append(para.Value);
            }

            return sb.ToString().Substring(1);
        }

        

        public Dictionary<string, string> ToMap()
        {
            return this._params;
        }
    }
   
}
