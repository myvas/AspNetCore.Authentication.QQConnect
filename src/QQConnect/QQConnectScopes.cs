using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Authentication
{
    public static class QQConnectScopes
    {
        public const string get_user_info = "get_user_info";
        public const string list_album = "list_album";
        public const string upload_pic = "upload_pic";
        public const string do_like = "do_like";

        #region Helpers
        public static ICollection<string> TryAdd(ICollection<string> currentScopes, params string[] scopes)
        {
            Array.ForEach(scopes, x =>
            {
                if (!currentScopes.Contains(x))
                {
                    currentScopes.Add(x);
                }
            });
            return currentScopes;
        }

        //public static ICollection<string> TryAdd(ICollection<string> currentScopes, params QQConnectScopes[] scopes)
        //{
        //    Array.ForEach(scopes, x =>
        //    {
        //        var s = x.ToString();
        //        if (!currentScopes.Contains(s))
        //        {
        //            currentScopes.Add(s);
        //        }
        //    });
        //    return currentScopes;
        //}
        #endregion
    }

    //
    // Summary:
    //     Defines constants for the well-known claim types that can be assigned to a subject.
    //     This class cannot be inherited.
    public static class QQConnectTokenNames
    {
        /// <summary>
        /// openid
        /// </summary>
        public const string openid = "openid";

        /// <summary>
        /// unionid
        /// </summary>
        public const string unionid = "unionid";

        /// <summary>
        /// scope
        /// </summary>
        public const string scope = "scope";

        /// <summary>
        /// access_token
        /// </summary>
        public const string access_token = "access_token";

        /// <summary>
        /// refresh_token
        /// </summary>
        public const string refresh_token = "refresh_token";

        /// <summary>
        /// token_type
        /// </summary>
        public const string token_type = "token_type";

        /// <summary>
        /// expires_at
        /// </summary>
        public const string expires_at = "expires_at";
    }
}
