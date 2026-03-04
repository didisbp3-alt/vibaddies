using System.Net;
using System.Net.Http.Json;
using  MVC_Buddies.Dtos;


namespace MVC_Buddies.Services
{


    public interface IAuthApiService
    {

        Task<(AuthResponseDto? Data, string? Error)> LoginAsync(LoginDto dto);
        //Task<(AuthResponseDto? Data, string? Error)> RegisterAsync(RegisterDto dto);
        Task<(AuthResponseDto? Data, string? Error)> RegisterAsync(RegisterRequestDto dto);

        Task<(bool Success, string Message)> SeedAdminAsync();
        Task<(string? Hash, string? Error)> HashPasswordAsync(string password);
    }


    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<(AuthResponseDto? Data, string? Error)> LoginAsync(LoginDto dto)
        {
            var resp = await _http.PostAsJsonAsync("Auth/login", dto);

            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
                return (data, null);
            }

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
                return (null, "Credenciais inválidas.");

            var msg = await SafeReadBodyAsync(resp);
            return (null, string.IsNullOrWhiteSpace(msg) ? "Erro ao fazer login." : msg);
        }

        //public async Task<(AuthResponseDto? Data, string? Error)> RegisterAsync(RegisterDto dto)
        //{
        //    var resp = await _http.PostAsJsonAsync("Auth/register", dto);

        //    if (resp.IsSuccessStatusCode)
        //    {
        //        var data = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        //        return (data, null);
        //    }

        //    if (resp.StatusCode == HttpStatusCode.Conflict)
        //        return (null, "Email já registado.");

        //    var msg = await SafeReadBodyAsync(resp);
        //    return (null, string.IsNullOrWhiteSpace(msg) ? "Erro ao registar." : msg);
        //}

        public async Task<(AuthResponseDto? Data, string? Error)> RegisterAsync(RegisterRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("auth/register", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (null, error);
            }

            var data = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            return (data, null);
        }

        public async Task<(bool Success, string Message)> SeedAdminAsync()
        {
            var resp = await _http.PostAsync("Auth/seed-admin", content: null);

            if (resp.IsSuccessStatusCode)
            {
                var msg = await SafeReadBodyAsync(resp);
                return (true, string.IsNullOrWhiteSpace(msg) ? "Admin criado/atualizado." : msg);
            }

            var err = await SafeReadBodyAsync(resp);
            return (false, string.IsNullOrWhiteSpace(err) ? "Erro ao fazer seed-admin." : err);
        }

        public async Task<(string? Hash, string? Error)> HashPasswordAsync(string password)
        {
            var resp = await _http.PostAsJsonAsync("Auth/hash-password", new HashPasswordDto { Password = password });

            if (resp.IsSuccessStatusCode)
            {
                // API devolve: { PasswordHash = "..." }
                var json = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (json != null && json.TryGetValue("PasswordHash", out var hash))
                    return (hash, null);

                return (null, "Resposta inesperada da API.");
            }

            var err = await SafeReadBodyAsync(resp);
            return (null, string.IsNullOrWhiteSpace(err) ? "Erro ao gerar hash." : err);
        }

        private static async Task<string> SafeReadBodyAsync(HttpResponseMessage resp)
        {
            try { return await resp.Content.ReadAsStringAsync(); }
            catch { return ""; }
        }
    }

}
