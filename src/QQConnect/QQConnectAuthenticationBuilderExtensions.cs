using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Authentication;
using Myvas.AspNetCore.Authentication.QQConnect;
using Myvas.AspNetCore.Authentication.QQConnect.Internal;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class QQConnectApplicationBuilderExtensions
    {
        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder)
            => builder.AddQQConnect(QQConnectDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder, Action<QQConnectOptions> setupAction)
            => builder.AddQQConnect(QQConnectDefaults.AuthenticationScheme, setupAction);

        public static AuthenticationBuilder AddQQConnect(this AuthenticationBuilder builder, string authenticationScheme, Action<QQConnectOptions> setupAction)
            => builder.AddQQConnect(authenticationScheme, QQConnectDefaults.DisplayName, setupAction);

        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<QQConnectOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddTransient<IQQConnectApi, QQConnectApi>();
            //return builder.AddOAuth<QQConnectOptions, QQConnectHandler>(authenticationScheme, displayName, setupAction);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<QQConnectOptions>, QQConnectPostConfigureOptions>());
            return builder.AddRemoteScheme<QQConnectOptions, QQConnectHandler>(authenticationScheme, displayName, setupAction);

        }
    }
}
