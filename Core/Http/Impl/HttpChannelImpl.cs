using System;
using System.Net;
using System.Text;
using Core.Http.Enums;
using Core.IO;
using Core.Primitive;

namespace Core.Http.Impl {

    internal sealed class HttpChannelImpl : HttpChannel {

        public override HttpResponse Get() {
            try {
                using(var response = CreateHttpRequest(HttpMethod.Get).GetResponse() as HttpWebResponse) {
                    if(response == null) {
                        return new HttpResponse {
                            Success    = false,
                            Error      = new NullReferenceException("Response is null."),
                            Response   = null,
                            StatusCode = Numbers.IntOf(HttpStatusCode.InternalServerError)
                        };
                    }

                    return new HttpResponse {
                        Success = true,
                        Error = null,
                        Response = Streams.GetBytes(response.GetResponseStream()),
                        StatusCode = Numbers.IntOf(response.StatusCode),
                    }
                    .PutCookies(response.Cookies)
                    .PutHeaders(response.Headers);
                }

            }
            catch(Exception error) {
                return HandleException(error);
            }
        }

        public override HttpResponse Send() {
            Preconditions.CheckNotNull(Entity, "HttpEntity can not be null.");

            try {
                var request = CreateHttpRequest(HttpMethod.Post);
                using(var steam = request.GetRequestStream()) {

                    Streams.PutBytes(Entity.GetBody(), steam, Encoding.UTF8);

                    using(var response = request.GetResponse() as HttpWebResponse) {
                        if(response == null) {
                            return new HttpResponse {
                                Success = false,
                                Error = new NullReferenceException("Response is null."),
                                Response = null,
                                StatusCode = Numbers.IntOf(HttpStatusCode.ServiceUnavailable)
                            };
                        }

                        return new HttpResponse {
                            Success = true,
                            Error = null,
                            Response = Streams.GetBytes(response.GetResponseStream()),
                            StatusCode = Numbers.IntOf(response.StatusCode),
                        }
                       .PutCookies(response.Cookies)
                       .PutHeaders(response.Headers);

                    }
                }
            }
            catch(Exception error) {
                return HandleException(error);
            }

        }

        private HttpWebRequest CreateHttpRequest(HttpMethod method) {
            var request = WebRequest.CreateHttp(HttpUrl.Uri);
            request.Proxy = null; // disable proxy to avoid network round-trip
            request.AllowAutoRedirect = AllowRedirect;
            request.Host = Strings.Of(HttpUrl.Host);
            request.Method = Enum.GetName(method.GetType(), method)?.ToUpper();
            request.Accept = Strings.Of(Accept);
            request.Connection = Strings.Of(Connection);
            request.UserAgent = Strings.Of(UserAgent);
            request.KeepAlive = true;
            request.AllowWriteStreamBuffering = true;

            ConfigureHeaders(request);

            if(Authentication != null) {
                switch(Authentication.Kind) {
                    case HttpAuthentication.HttpAuthenticaionKind.Network:
                        request.Credentials = new NetworkCredential(Authentication.Name, Authentication.Password);
                        break;
                    case HttpAuthentication.HttpAuthenticaionKind.Form:
                        request.Headers[HttpRequestHeader.Authorization] =
                            "BASIC" + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Authentication.Name}:{Authentication.Password}"));
                        break;
                    default:
                        request.UseDefaultCredentials = true;
                        break;
                }
            }

            if(AllowCookies) {
                request.CookieContainer = new CookieContainer(); // enable Cookies 
                ConfigureCookies(request.CookieContainer);
            }

            if(Entity == null) {
                return request;
            }

            request.ContentType = Entity.ContentType;
            request.ContentLength = Entity.ContentLength;

            return request;
        }

        private void ConfigureHeaders(HttpWebRequest request) {
            foreach(var item in Headers.AsReadOnly()) {
                request.Headers.Add(item.Key, item.Value);
            }
        }

        private void ConfigureCookies(CookieContainer container) {
            foreach(var pair in Cookies.AsReadOnly()) {
                container.Add(pair.Value);
            }
        }

        private static HttpResponse HandleException(Exception exception) {

            if(exception is WebException) {
                var webException = exception as WebException; // we dun need to check null here
                var response = webException.Response as HttpWebResponse;

                // 1. Handle Transport-Level Error
                if (webException.Status == WebExceptionStatus.ConnectFailure
                 || webException.Status == WebExceptionStatus.NameResolutionFailure
                 || webException.Status == WebExceptionStatus.Timeout) {
                    return new HttpResponse {
                        Error      = new Exception($"NetworkError[{webException.Status}]"),
                        Response   = Streams.GetBytes(webException.Response?.GetResponseStream()),
                        StatusCode = Numbers.IntOf(HttpStatusCode.ServiceUnavailable),
                        Success    = false
                    };
                }

                // 2. Handle Http Errors
                if(webException.Status == WebExceptionStatus.ProtocolError) {
                    return new HttpResponse {
                        Success    = false,
                        Error      = webException,
                        Response   = Streams.GetBytes(response?.GetResponseStream()),
                        StatusCode = Numbers.IntOf(response?.StatusCode)
                    };
                }

                // 3. Other Errors
                return new HttpResponse {
                    Error      = new Exception($"Unexpected Error[{webException.Status}]"),
                    Response   = Streams.GetBytes(response?.GetResponseStream()),
                    StatusCode = Numbers.IntOf(HttpStatusCode.ServiceUnavailable),
                    Success    = false
                };
            }

            return new HttpResponse {
                Error      = exception,
                StatusCode = Numbers.IntOf(HttpStatusCode.ServiceUnavailable),
                Success    = false
            };

        }
    }

}