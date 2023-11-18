namespace FPT_Vote.Services;

using Microsoft.JSInterop;
using System.Threading.Tasks;

public class JsInterpop
{
    public ValueTask SaveFileAs(string filename, byte[] data, IJSRuntime jsRuntime)
    {
        return jsRuntime.InvokeVoidAsync("saveAsFile", filename, Convert.ToBase64String(data));
    }
}