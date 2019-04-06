using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Authentication
{
    /// <summary>
    /// Defines constants for the well-known claim types that can be assigned to a subject.
    /// This class cannot be inherited.
    /// </summary>
    public static class QQConnectClaimTypes
    {
        /// <summary>
        /// urn:qq:unionid
        /// </summary>
        public const string ClientId = "urn:qq:client_id";

        /// <summary>
        /// urn:qq:unionid
        /// </summary>
        public const string UnionId = "urn:qq:unionid";

        #region snsapi_base
        /// <summary>
        /// urn:qq:openid, should be <see cref="ClaimTypes.NameIdentifier"/>
        /// </summary>
        public const string OpenId = "urn:qq:openid";

        /// <summary>
        /// urn:qq:scope
        /// </summary>
        public const string Scope = "urn:qq:scope";
        #endregion
        #region snsapi_userinfo
        /// <summary>
        /// urn:qq:nickname, should be <see cref="ClaimTypes.Name"/>，用户在QQ空间的昵称。
        /// </summary>
        public const string NickName = "urn:qq:nickname";

        /// <summary>
        /// urn:qq:headimgurl
        /// </summary>
        public const string HeadImageUrl = "urn:qq:headimgurl";

        /// <summary>
        /// urn:qq:figureurl, 大小为30×30像素的QQ空间头像URL。
        /// </summary>
        public const string FigureUrl = "urn:qq:figureurl";

        /// <summary>
        /// urn:qq:headimgurl_1, 大小为50x50像素的QQ空间头像URL。
        /// </summary>
        public const string FigureUrl_1 = "urn:qq:figureurl_1";

        /// <summary>
        /// urn:qq:headimgurl_2, 大小为100x100像素的QQ空间头像URL。
        /// </summary>
        public const string FigureUrl_2 = "urn:qq:figureurl_2";

        /// <summary>
        /// urn:qq:headimgurl_qq_1, 大小为40×40像素的QQ头像URL。
        /// </summary>
        public const string FigureUrl_qq_1 = "urn:qq:figureurl_qq_1";

        /// <summary>
        /// urn:qq:headimgurl_qq_2, 大小为100x100像素的QQ头像URL。
        /// </summary>
        public const string FigureUrl_qq_2 = "urn:qq:figureurl_qq_2";
        
        /// <summary>
        /// urn:qq:vip，是否QQ会员:1 or 0。
        /// </summary>
        public const string Vip = "urn:qq:vip";

        /// <summary>
        /// urn:qq:is_qq_vip，是否QQ会员:1 or 0。
        /// </summary>
        public const string IsQQVip = "urn:qq:is_qq_vip";

        /// <summary>
        /// urn:qq:is_qq_year_vip，是否年费QQ会员:1 or 0。
        /// </summary>
        public const string IsQQYearVip = "urn:qq:is_qq_year_vip";

        /// <summary>
        /// urn:qq:level，QQ会员等级：1~7。
        /// </summary>
        public const string Level = "urn:qq:level";

        /// <summary>
        /// urn:qq:qq_vip_level，QQ会员等级：1~7。
        /// </summary>
        public const string QQVipLevel = "urn:qq:qq_vip_level";

        /// <summary>
        /// urn:qq:is_yellow_vip，超级会员：1 or 0。
        /// </summary>
        public const string IsYellowVip = "urn:qq:is_yellow_vip";

        /// <summary>
        /// urn:qq:is_yellow_year_vip，年费超级会员:1 or 0。
        /// </summary>
        public const string IsYellowYearVip = "urn:qq:is_yellow_year_vip";

        /// <summary>
        /// urn:qq:yellow_vip_level，超级会员等级:1~8。
        /// </summary>
        public const string YellowVipLevel = "urn:qq:yellow_vip_level";
        
        /// <summary>
        /// urn:qq:sex，性别。 如果获取不到则默认返回"男"。
        /// </summary>
        public const string Gender = "urn:qq:gender";

        /// <summary>
        /// urn:qq:country
        /// </summary>
        public const string Country = "urn:qq:country";

        /// <summary>
        /// urn:qq:province
        /// </summary>
        public const string Province = "urn:qq:province";

        /// <summary>
        /// urn:qq:city
        /// </summary>
        public const string City = "urn:qq:city";
        
        /// <summary>
        /// urn:qq:privilege，JArray, 可能有多个Claims。
        /// </summary>
        public const string Privilege = "urn:qq:privilege";
        #endregion
    }
}
