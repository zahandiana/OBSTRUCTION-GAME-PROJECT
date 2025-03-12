using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Backend_Obstruction
{
    public class ReturnIndexByDefault
    {
        private readonly RequestDelegate _next;
        private readonly string _filePath;

        public ReturnIndexByDefault(RequestDelegate next, string filePath)
        {
            _next = next;
            _filePath = filePath;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Response.HasStarted && !context.Request.Path.StartsWithSegments("/api"))
            {
                // Check if the request hasn't been handled yet.
                var fileInfo = new FileInfo(_filePath);

                if (fileInfo.Exists)
                {
                    // Serve the specific static file if it exists.
                    context.Response.Clear();
                    context.Response.ContentType = "text/html"; // Adjust the content type as needed.
                    await context.Response.SendFileAsync(_filePath);
                    return;
                }
            }

            // If the file doesn't exist or the request has already been handled,
            // proceed to the next middleware.
            await _next(context);
        }
    }
}
