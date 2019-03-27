namespace Myvas.AspNetCore.Authentication
{
    public static class QQConnectDefaults
    {
        /// <summary>
        /// QQConnect
        /// </summary>
        public const string AuthenticationScheme = "QQConnect";

        /// <summary>
        /// QQConnect
        /// </summary>
        public const string DisplayName = "QQConnect";

        /// <summary>
        /// QQConnect
        /// </summary>
        public const string ClaimsIssuer = "QQConnect";

        /// <summary>
        /// /signin-qqconnect
        /// </summary>
        public const string CallbackPath = "/signin-qqconnect";

		/// <summary>
		/// https://graph.qq.com/oauth2.0/authorize
		/// </summary>
		public static readonly string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

		/// <summary>
		/// https://graph.qq.com/oauth2.0/token
		/// </summary>
		public static readonly string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

		/// <summary>
		/// https://graph.qq.com/oauth2.0/me
		/// </summary>
		public static readonly string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

		/// <summary>
		/// https://graph.qq.com/user/get_user_info
		/// </summary>
		public static readonly string UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";

		/// <summary>
		/// https://graph.qq.com/user/get_vip_info
		/// </summary>
		public static readonly string UserVipInfoEndpoint = "https://graph.qq.com/user/get_vip_info";

		/// <summary>
		/// https://graph.qq.com/user/get_vip_rich_info
		/// </summary>
		public static readonly string UserVipRichInfoEndpoint = "https://graph.qq.com/user/get_vip_rich_info";

		/// <summary>
		/// https://graph.qq.com/photo/list_album
		/// </summary>
		public static readonly string PhotoListAlbumEndpoint = "https://graph.qq.com/photo/list_album";

		/// <summary>
		/// https://graph.qq.com/photo/upload_pic
		/// </summary>
		public static readonly string PhotoUploadPictureEndpoint = "https://graph.qq.com/photo/upload_pic";

    }
}