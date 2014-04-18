using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkTajm.Resources;

namespace WorkTajm.Constants
{
    class WebExceptions
    {
        private static readonly Dictionary<WebExceptionStatus, string> ErrorCodes = new Dictionary<WebExceptionStatus, string>
        {
	        { WebExceptionStatus.ConnectFailure, AppResources.WebExceptionStatus_ConnectFailure },
	        { WebExceptionStatus.ConnectionClosed, AppResources.WebExceptionStatus_ConnectionClosed },
	        { WebExceptionStatus.KeepAliveFailure, AppResources.WebExceptionStatus_KeepAliveFailure },
	        { WebExceptionStatus.MessageLengthLimitExceeded, AppResources.WebExceptionStatus_MessageLengthLimitExceeded },
	        { WebExceptionStatus.NameResolutionFailure, AppResources.WebExceptionStatus_NameResolutionFailure },
	        { WebExceptionStatus.Pending, AppResources.WebExceptionStatus_Pending },
	        { WebExceptionStatus.PipelineFailure, AppResources.WebExceptionStatus_PipelineFailure },
	        { WebExceptionStatus.ProtocolError, AppResources.WebExceptionStatus_ProtocolError },
	        { WebExceptionStatus.ProxyNameResolutionFailure, AppResources.WebExceptionStatus_ProxyNameResolutionFailure },
	        { WebExceptionStatus.ReceiveFailure, AppResources.WebExceptionStatus_ReceiveFailure },
	        { WebExceptionStatus.RequestCanceled, AppResources.WebExceptionStatus_RequestCanceled },
	        { WebExceptionStatus.SecureChannelFailure, AppResources.WebExceptionStatus_SecureChannelFailure },
	        { WebExceptionStatus.SendFailure, AppResources.WebExceptionStatus_SendFailure },
	        { WebExceptionStatus.ServerProtocolViolation, AppResources.WebExceptionStatus_ServerProtocolViolation },
	        { WebExceptionStatus.Success, AppResources.WebExceptionStatus_Success },
	        { WebExceptionStatus.Timeout, AppResources.WebExceptionStatus_Timeout },
	        { WebExceptionStatus.TrustFailure, AppResources.WebExceptionStatus_TrustFailure },
	        { WebExceptionStatus.UnknownError, AppResources.WebExceptionStatus_UnknownError }
        };
        public static string Lookup(WebExceptionStatus value)
        {
            string result = ErrorCodes[WebExceptionStatus.UnknownError];
            ErrorCodes.TryGetValue(value, out result);
            return result;
        }
    }
}
