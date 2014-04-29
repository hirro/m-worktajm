using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkTajm.Resources;

namespace WorkTajm.Constants
{
    class HttpStatusCodeHelper
    {
        private static readonly Dictionary<HttpStatusCode, string> ErrorCodes = new Dictionary<HttpStatusCode, string>
        {
	        { HttpStatusCode.NotFound, AppResources.HttpStatusCode_NotFound },
	        { HttpStatusCode.Forbidden, AppResources.HttpStatusCode_Forbidden },
	        { HttpStatusCode.Unauthorized, AppResources.HttpStatusCode_Unauthorized },
	        { HttpStatusCode.InternalServerError, AppResources.HttpStatusCode_InternalServerError },
        };
        public static string Lookup(HttpStatusCode value)
        {
            string result = AppResources.HttpStatusCode_DefaultError;
            ErrorCodes.TryGetValue(value, out result);
            return result;
        }
    }
}
