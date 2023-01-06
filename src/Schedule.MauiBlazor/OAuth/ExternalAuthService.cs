using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel.OidcClient;
using Schedule.MauiBlazor.Settings;

namespace Schedule.MauiBlazor.OAuth
{
    public class ExternalAuthService
    {
        private const string AuthenticationType = "Identity.Application";

        public event Action<ClaimsPrincipal> UserChanged;
        private ClaimsPrincipal _currentUser;

        private readonly IScheduleApplicationSettingService _scheduleApplicationSettingService;
        private readonly OidcClient _oidcClient;

        public ExternalAuthService(
            IScheduleApplicationSettingService myapp2ApplicationSettingService,
            OidcClient oidcClient)
        {
            _scheduleApplicationSettingService = myapp2ApplicationSettingService;
            _oidcClient = oidcClient;
        }

        public async Task<LoginResult> LoginAsync()
        {
            var loginResult = await _oidcClient.LoginAsync(new LoginRequest());

            if (loginResult.IsError)
            {
                return LoginResult.Failed(loginResult.Error, loginResult.ErrorDescription);
            }

            await _scheduleApplicationSettingService.SetAccessTokenAsync(loginResult.AccessToken);
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityTokenHandler().ReadJwtToken(loginResult.AccessToken).Claims, AuthenticationType));
            UserChanged?.Invoke(_currentUser);

            return LoginResult.Success();
        }

        public async Task SignOutAsync()
        {
            var logoutResult = await _oidcClient.LogoutAsync();
            await _scheduleApplicationSettingService.SetAccessTokenAsync(null);

            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            UserChanged?.Invoke(_currentUser);
        }

        public async Task<ClaimsPrincipal> GetCurrentUser()
        {
            var accessToken = await _scheduleApplicationSettingService.GetAccessTokenAsync();
            if (!accessToken.IsNullOrWhiteSpace())
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                if (jwtToken.ValidTo > DateTime.UtcNow)
                {
                    _currentUser = new ClaimsPrincipal(new ClaimsIdentity(new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims, AuthenticationType));
                }
            }

            return _currentUser ?? new ClaimsPrincipal();
        }

        public class LoginResult
        {
            public bool IsError { get; set; }

            public string Error { get; set; }

            public string ErrorDescription { get; set; }

            public static LoginResult Success()
            {
                return new LoginResult
                {
                    IsError = false
                };
            }

            public static LoginResult Failed(string error, string errorDescription)
            {
                return new LoginResult
                {
                    IsError = true,
                    Error = error,
                    ErrorDescription = errorDescription
                };
            }
        }
    }
}
