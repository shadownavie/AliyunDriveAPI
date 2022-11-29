using AliyunDriveAPI.Models.Converters;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AliyunDriveAPI;

public partial class AliyunDriveApiClient
{
    private string _refreshToken;
    private string _token;
    private DateTime? _tokenExpireTime;

    private readonly HttpClient _httpClient;

    public static JsonSerializerOptions JsonSerializerOptions
        => new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            Converters =
            {
                new Models.Converters.JsonStringEnumConverter(),
                new JsonNodeConverter(),
                new TimeSpanSecondConverter(),
                new NullableTimeSpanSecondConverter(),
                new DatetimeConverter(),
            }
        };

    public AliyunDriveApiClient(string refreshToken)
    {
        _refreshToken = refreshToken;
        _httpClient = new() { BaseAddress = new Uri("https://api.aliyundrive.com/") };
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync()
    {
        var obj = new JsonObject
        {
            ["refresh_token"] = _refreshToken,
            ["grant_type"] = "refresh_token"
        };
        return await SendJsonPostAsync<RefreshTokenResponse>("https://auth.aliyundrive.com/v2/account/token", obj, false);
    }

    private bool IsTokenExpire()
        => _tokenExpireTime == null || _tokenExpireTime.Value <= DateTime.Now;

    private async Task PrepareTokenAsync()
    {
        if (!IsTokenExpire()) return;
        var res = await RefreshTokenAsync();
        _token = res.AccessToken;
        _refreshToken = res.RefreshToken;
        _tokenExpireTime = res.ExpireTime;
        if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
    }


    /// <summary>
    /// 获取登陆二维码
    /// </summary>
    /// <returns></returns>
    public async Task<QrCodeResponse> GetQrcodeAsync()
    {
        return await SendJsonGetAsync<QrCodeResponse>("https://passport.aliyundrive.com/newlogin/qrcode/generate.do?appName=aliyun_drive&fromSite=52&appName=aliyun_drive&appEntrance=web", false);
    }

    /// <summary>
    /// 获取状态
    /// </summary>
    /// <param name="t"></param>
    /// <param name="ck"></param>
    /// <returns></returns>
    public async Task<QrCodeResponse> QueryQrcodeStatusAsync(long? t,string ck)
    {
        var obj = new SendRequestFrom().AddParam("t", t).AddParam("ck", ck);
        return await SendJsonPostNoTokenAsync<QrCodeResponse>("https://passport.aliyundrive.com/newlogin/qrcode/query.do?appName=aliyun_drive", obj, false);
    }

}