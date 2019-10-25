using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Authentication.QQConnect.Internal
{
    internal interface IQQConnectApi
    {
        Task<JsonDocument> AddAlbum(HttpClient backchannel, string addAlbumEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<JsonDocument> GetOpenId(HttpClient backchannel, string openIdEndpoint, string accessToken, CancellationToken cancellationToken);
        Task<OAuthTokenResponse> GetToken(HttpClient backchannel, string tokenEndpoint, string clientId, string clientSecret, string code, string redirectUri, CancellationToken cancellationToken);
        Task<JsonDocument> GetUserInfo(HttpClient backchannel, string userInformationEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<JsonDocument> GetUserVipInfo(HttpClient backchannel, string userVipInfoEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<JsonDocument> GetUserVipRichInfo(HttpClient backchannel, string userVipRichInfoEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<JsonDocument> ListAlbum(HttpClient backchannel, string listAlbumEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<JsonDocument> ListPhoto(HttpClient backchannel, string listPhotoEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<OAuthTokenResponse> RefreshToken(HttpClient backchannel, string refreshTokenEndpoint, string appId, string refreshToken, CancellationToken cancellationToken);
        Task<JsonDocument> UploadPicture(HttpClient backchannel, string uploadPictureEndpoint, string accessToken, string openid, string clientId, CancellationToken cancellationToken);
        Task<bool> ValidateToken(HttpClient backchannel, string validateTokenEndpoint, string appId, string accessToken, CancellationToken cancellationToken);
    }
}