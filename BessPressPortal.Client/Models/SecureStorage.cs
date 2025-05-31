
namespace BessPressPortal.Client.Models
{
    using Microsoft.JSInterop;
    using System.Threading.Tasks;

    public class SecureStorage
    {
        private readonly IJSRuntime _jsRuntime;

        public SecureStorage(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public async ValueTask SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async ValueTask ClearTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }
    }

}
