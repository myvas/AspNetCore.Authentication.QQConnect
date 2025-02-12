﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Base64UrlTextEncoder = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder;

namespace Myvas.AspNetCore.Authentication.QQConnect.Internal
{
    internal class QQConnectHandler : RemoteAuthenticationHandler<QQConnectOptions>
    {
        protected HttpClient Backchannel => Options.Backchannel;

        /// <summary>
        /// The handler calls methods on the events which give the application control at certain points where processing is occurring.
        /// If it is not provided a default instance is supplied which does nothing when the methods are called.
        /// </summary>
        protected new OAuthEvents Events
        {
            get { return (OAuthEvents)base.Events; }
            set { base.Events = value; }
        }

        private readonly IQQConnectApi _api;
#if NET8_0_OR_GREATER
        /// <summary>
        /// Initializes a new instance of <see cref="QQConnectHandler"/>.
        /// </summary>
        /// <inheritdoc />
        [Obsolete("ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.")]
        public QQConnectHandler(
            IQQConnectApi api,
            IOptionsMonitor<QQConnectOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, loggerFactory, encoder, clock)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }
        
        public QQConnectHandler(
            IQQConnectApi api,
            IOptionsMonitor<QQConnectOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder)
            : base(options, loggerFactory, encoder)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }
#else
        public QQConnectHandler(
            IQQConnectApi api,
            IOptionsMonitor<QQConnectOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, loggerFactory, encoder, clock)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }
#endif
        //protected const string CorrelationPrefix = ".AspNetCore.Correlation.";
        protected const string CorrelationMarker = "N";
        protected const string CorrelationProperty = ".xsrf";
        //protected const string AuthSchemeKey = ".AuthScheme";

        //protected static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = OriginalPathBase + OriginalPath + Request.QueryString;
            }

            // OAuth2 10.12 CSRF
            GenerateCorrelationId(properties);

            var authorizationEndpoint = BuildChallengeUrl(properties, BuildRedirectUri(Options.CallbackPath));
            var redirectContext = new RedirectContext<OAuthOptions>(
                Context, Scheme, Options,
                properties, authorizationEndpoint);
            await Events.RedirectToAuthorizationEndpoint(redirectContext);

            var location = Context.Response.Headers[HeaderNames.Location];
            if (location == StringValues.Empty)
            {
                location = "(not set)";
            }
            var cookie = Context.Response.Headers[HeaderNames.SetCookie];
            if (cookie == StringValues.Empty)
            {
                cookie = "(not set)";
            }
            Logger.HandleChallenge(location, cookie);
        }

        protected virtual string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var queryStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "response_type", "code" },
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri }
            };

            var scope = PickAuthenticationProperty(properties, QQConnectChallengeProperties.ScopeKey, FormatScope, Options.Scope);
            var display = PickAuthenticationProperty(properties, QQConnectChallengeProperties.DisplayStyleKey, Options.DisplayStyle);

            var correlationId = properties.Items[CorrelationProperty];
            queryStrings.Add("state", correlationId);
            queryStrings.Add(QQConnectChallengeProperties.ScopeKey, scope);
            queryStrings.Add(QQConnectChallengeProperties.DisplayStyleKey, display);

            // Store protectedProperties in Cookie
            var protectedProperties = Options.StateDataFormat.Protect(properties);
            // Clean up all the deprecated cookies with pattern: "Options.CorrelationCookie.Name + Scheme.Name + "." + correlationId + "." + CorrelationMarker"
            var deprecatedCookieNames = Context.Request.Cookies.Keys.Where(x => x.StartsWith(Options.CorrelationCookie.Name + Scheme.Name + "."));
            deprecatedCookieNames.ToList().ForEach(x => Context.Response.Cookies.Delete(x));
            // Append a response cookie for state/properties
#if NET8_0_OR_GREATER
            var cookieOptions = Options.CorrelationCookie.Build(Context, TimeProvider.GetUtcNow());
#else
            var cookieOptions = Options.CorrelationCookie.Build(Context, Clock.UtcNow);
#endif
            var protectedPropertiesCookieName = FormatStateCookieName(correlationId);
            Context.Response.Cookies.Append(protectedPropertiesCookieName, protectedProperties, cookieOptions);

            var authorizationEndpoint = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, queryStrings);
            return authorizationEndpoint;
        }

        #region To satisfy too big protected properties, we should store it to cookie '.{CorrelationCookieName}.{SchemeName}.{CorrelationMarker}.{CorrelationId|state}'
        protected virtual string FormatCorrelationCookieName(string correlationId)
        {
            return Options.CorrelationCookie.Name + Scheme.Name + "." + correlationId;
        }

        protected virtual string FormatStateCookieName(string correlationId)
        {
            return Options.CorrelationCookie.Name + Scheme.Name + "." + CorrelationMarker + "." + correlationId;
        }

        /// <inheritdoc/>
        protected override void GenerateCorrelationId(AuthenticationProperties properties)
        {
            //base.GenerateCorrelationId(properties);

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var bytes = new byte[32];
            RandomNumberGenerator.Fill(bytes);
            var correlationId = Base64UrlTextEncoder.Encode(bytes);
#if NET8_0_OR_GREATER
            var cookieOptions = Options.CorrelationCookie.Build(Context, TimeProvider.GetUtcNow());
#else
            var cookieOptions = Options.CorrelationCookie.Build(Context, Clock.UtcNow);
#endif

            properties.Items[CorrelationProperty] = correlationId;

            //var cookieName = Options.CorrelationCookie.Name + correlationId;
            var cookieName = FormatCorrelationCookieName(correlationId);

            Response.Cookies.Append(cookieName, CorrelationMarker, cookieOptions);
        }

        /// <inheritdoc/>
        protected override bool ValidateCorrelationId(AuthenticationProperties properties)
        {
            //return base.ValidateCorrelationId(properties);

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (!properties.Items.TryGetValue(CorrelationProperty, out var correlationId))
            {
                Logger.LogWarning($"The CorrectionId not found in '{Options.CorrelationCookie.Name!}'");
                return false;
            }

            properties.Items.Remove(CorrelationProperty);

            //var cookieName = Options.CorrelationCookie.Name + correlationId;
            var cookieName = FormatCorrelationCookieName(correlationId);

            var correlationCookie = Request.Cookies[cookieName];
            if (string.IsNullOrEmpty(correlationCookie))
            {
                Logger.LogWarning($"The CorrectionCookie not found in '{cookieName}'");
                return false;
            }
#if NET8_0_OR_GREATER
            var cookieOptions = Options.CorrelationCookie.Build(Context, TimeProvider.GetUtcNow());
#else
            var cookieOptions = Options.CorrelationCookie.Build(Context, Clock.UtcNow);
#endif

            Response.Cookies.Delete(cookieName, cookieOptions);

            if (!string.Equals(correlationCookie, CorrelationMarker, StringComparison.Ordinal))
            {
                Logger.LogWarning($"Unexcepted CorrectionCookieValue: '{cookieName}'='{correlationCookie}'");
                return false;
            }

            return true;
        }
        #endregion

        #region Pick value from AuthenticationProperties
        private static string PickAuthenticationProperty<T>(
            AuthenticationProperties properties,
            string name,
            Func<T, string> formatter,
            T defaultValue)
        {
            string value;
            var parameterValue = properties.GetParameter<T>(name);
            if (parameterValue != null)
            {
                value = formatter(parameterValue);
            }
            else if (!properties.Items.TryGetValue(name, out value))
            {
                value = formatter(defaultValue);
            }

            // Remove the parameter from AuthenticationProperties so it won't be serialized into the state
            properties.Items.Remove(name);

            return value;
        }

        private static string PickAuthenticationProperty(
            AuthenticationProperties properties,
            string name,
            string defaultValue = null)
            => PickAuthenticationProperty(properties, name, x => x, defaultValue);
        #endregion

        protected virtual string FormatScope(IEnumerable<string> scopes)
            => string.Join(",", scopes); // OAuth2 3.3 space separated, but QQConnect not

        protected virtual List<string> SplitScope(string scope)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(scope)) return result;
            return scope.Split(',').ToList();
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var query = Request.Query;

            var error = query["error"];
            if (!StringValues.IsNullOrEmpty(error))
            {
                var failureMessage = new StringBuilder();
                failureMessage.Append(error);
                var errorDescription = query["error_description"];
                if (!StringValues.IsNullOrEmpty(errorDescription))
                {
                    failureMessage.Append(";Description=").Append(errorDescription);
                }
                var errorUri = query["error_uri"];
                if (!StringValues.IsNullOrEmpty(errorUri))
                {
                    failureMessage.Append(";Uri=").Append(errorUri);
                }

                return HandleRequestResult.Fail(failureMessage.ToString());
            }

            var state = query["state"]; // ie. correlationId
            if (StringValues.IsNullOrEmpty(state))
            {
                return HandleRequestResult.Fail("The oauth state was missing.");
            }

            var stateCookieName = FormatStateCookieName(state);
            var protectedProperties = Request.Cookies[stateCookieName];
            if (string.IsNullOrEmpty(protectedProperties))
            {
                Logger.LogError($"The protected properties not found in cookie '{stateCookieName}'");
                return HandleRequestResult.Fail($"The oauth state cookie was missing: Cookie: {stateCookieName}");
            }
            else
            {
                Logger.LogDebug($"The protected properties found in cookie '{stateCookieName}' with value '{protectedProperties}'");
            }

            var properties = Options.StateDataFormat.Unprotect(protectedProperties);

            if (properties == null)
            {
                return HandleRequestResult.Fail($"The oauth state cookie was invalid: Cookie: {stateCookieName}");
            }

            // OAuth2 10.12 CSRF
            if (!ValidateCorrelationId(properties))
            {
                return HandleRequestResult.Fail("Correlation failed.", properties);
            }

            // Cleanup state & correlation cookie
            Response.Cookies.Delete(stateCookieName);
            var correlationCookieName = FormatCorrelationCookieName(state);
            Response.Cookies.Delete(correlationCookieName);
            Logger.LogDebug($"Cookies deleted: '{stateCookieName}' and '{correlationCookieName}'");

            var code = query["code"];

            if (StringValues.IsNullOrEmpty(code))
            {
                Logger.LogWarning("Code was not found.");
                return HandleRequestResult.Fail("Code was not found.", properties);
            }

            //var codeExchangeContext = new OAuthCodeExchangeContext(properties, code, BuildRedirectUri(Options.CallbackPath));
            //var tokens = await ExchangeCodeAsync(codeExchangeContext);
            using var tokens = await ExchangeCodeAsync(code, BuildRedirectUri(Options.CallbackPath));

            if (tokens.Error != null)
            {
                return HandleRequestResult.Fail(tokens.Error, properties);
            }

            if (string.IsNullOrEmpty(tokens.AccessToken))
            {
                return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
            }

            var identity = new ClaimsIdentity(ClaimsIssuer);

            if (Options.SaveTokens)
            {
                var authTokens = new List<AuthenticationToken>
                {
                    new AuthenticationToken { Name = QQConnectTokenNames.access_token, Value = tokens.AccessToken }
                };
                if (!string.IsNullOrEmpty(tokens.RefreshToken))
                {
                    authTokens.Add(new AuthenticationToken { Name = QQConnectTokenNames.refresh_token, Value = tokens.RefreshToken });
                }
                if (!string.IsNullOrEmpty(tokens.TokenType))
                {
                    authTokens.Add(new AuthenticationToken { Name = QQConnectTokenNames.token_type, Value = tokens.TokenType });
                }
                if (!string.IsNullOrEmpty(tokens.GetOpenId()))
                {
                    authTokens.Add(new AuthenticationToken { Name = QQConnectTokenNames.openid, Value = tokens.GetOpenId() });
                }
                if (!string.IsNullOrEmpty(tokens.GetUnionId()))
                {
                    authTokens.Add(new AuthenticationToken { Name = QQConnectTokenNames.unionid, Value = tokens.GetUnionId() });
                }
                if (!string.IsNullOrEmpty(tokens.GetScope()))
                {
                    authTokens.Add(new AuthenticationToken { Name = QQConnectTokenNames.scope, Value = tokens.GetScope() });
                }
                if (!string.IsNullOrEmpty(tokens.ExpiresIn))
                {
                    if (int.TryParse(tokens.ExpiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                    {
                        // https://www.w3.org/TR/xmlschema-2/#dateTime
                        // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
#if NET8_0_OR_GREATER
                        var expiresAt = TimeProvider.GetUtcNow() + TimeSpan.FromSeconds(value);
#else
                        var expiresAt = Clock.UtcNow + TimeSpan.FromSeconds(value);
#endif
                        authTokens.Add(new AuthenticationToken
                        {
                            Name = QQConnectTokenNames.expires_at,
                            Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                        });
                    }
                }

                properties.StoreTokens(authTokens); //ExternalLoginInfo.AuthenticationTokens
            }

            var ticket = await CreateTicketAsync(identity, properties, tokens);
            if (ticket != null)
            {
                return HandleRequestResult.Success(ticket);
            }
            else
            {
                return HandleRequestResult.Fail("Failed to retrieve user information from remote server.", properties);
            }
        }

        /// <summary>
        /// 腾讯定义的接口方法与标准方法不一致，故须覆写此函数。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        protected virtual async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            return await _api.GetToken(Options.Backchannel, Options.TokenEndpoint, Options.AppId, Options.AppKey, code, redirectUri, Context.RequestAborted);
        }

        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            AuthenticationProperties properties,
            OAuthTokenResponse tokens)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            // Get the openId and clientId
            var payload = await _api.GetOpenId(Options.Backchannel, Options.OpenIdEndpoint, tokens.AccessToken, Context.RequestAborted);
            //{“client_id”:”YOUR_APPID”,”openid”:”YOUR_OPENID”}
            var clientId = payload.GetString("client_id");
            var openid = payload.GetString("openid");

            // Get the UserInfo
            var userInfoPayload = await _api.GetUserInfo(Options.Backchannel, Options.UserInformationEndpoint, tokens.AccessToken, openid, clientId, Context.RequestAborted);
            if (string.IsNullOrWhiteSpace(userInfoPayload.GetString("openid")))
                userInfoPayload = userInfoPayload.AppendElement("openid", openid);
            if (string.IsNullOrWhiteSpace(userInfoPayload.GetString("client_id")))
                userInfoPayload = userInfoPayload.AppendElement("client_id", clientId);

            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, userInfoPayload.RootElement);//, ticket, Context, Options, Backchannel, tokens, userInfoPayload);
            context.RunClaimActions();

            await Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
        }
    }
}