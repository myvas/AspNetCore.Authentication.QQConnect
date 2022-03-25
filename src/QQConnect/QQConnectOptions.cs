using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Myvas.AspNetCore.Authentication.QQConnect;
using System;
using System.Security.Claims;

namespace Myvas.AspNetCore.Authentication
{
    public class QQConnectOptions : OAuthOptions
    {
        public string OpenIdEndpoint { get; set; }

        /// <summary>
        /// 用于决定展示的样式，网站接入时使用。
        /// （1）不传则默认展示为PC下的样式。
        /// （2）如果传入“mobile”，则展示为mobile端下的样式。
        /// </summary>
        public string DisplayStyle { get; set; }

        /// <summary>
        /// aka. client_id, consumer_key
        /// </summary>
        public string AppId { get => ClientId; set => ClientId = value; }
        /// <summary>
        /// aka. client_secret, consumer_secret
        /// </summary>
        public string AppKey { get => ClientSecret; set => ClientSecret = value; }

        public QQConnectOptions()
        {
            OpenIdEndpoint = QQConnectDefaults.OpenIdEndpoint;

            CallbackPath = new PathString(QQConnectDefaults.CallbackPath);
            AuthorizationEndpoint = QQConnectDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQConnectDefaults.TokenEndpoint;
            UserInformationEndpoint = QQConnectDefaults.UserInformationEndpoint;

            Scope.Add(QQConnectScopes.get_user_info);
            //QQOAuthScopes.TryAdd(Scope,
            //    QQOAuthScopes.Items.get_user_info,
            //    QQOAuthScopes.Items.list_album,
            //    QQOAuthScopes.Items.upload_pic,
            //    QQOAuthScopes.Items.do_like);

            DisplayStyle = "";//mobile, default for Desktop Web Style.

            ClaimsIssuer = QQConnectDefaults.ClaimsIssuer;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "openid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");

            ClaimActions.MapJsonKey(QQConnectClaimTypes.ClientId, "client_id");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.OpenId, "openid");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.NickName, "nickname");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.FigureUrl, "figureurl");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.FigureUrl_1, "figureurl_1");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.FigureUrl_2, "figureurl_2");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.FigureUrl_qq_1, "figureurl_qq_1");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.FigureUrl_qq_2, "figureurl_qq_2");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.IsYellowVip, "is_yellow_vip");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.Vip, "vip");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.YellowVipLevel, "yellow_vip_level");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.Level, "level");
            ClaimActions.MapJsonKey(QQConnectClaimTypes.IsYellowYearVip, "is_yellow_year_vip");
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(OpenIdEndpoint))
            {
                throw new ArgumentNullException(nameof(OpenIdEndpoint));
            }

            base.Validate();
        }
    }
}
