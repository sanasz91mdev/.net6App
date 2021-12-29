﻿using Serilog;
using System.Text;

namespace DigitalBanking.ServiceExtensions
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            // Read and log request body data
            string requestBodyPayload = await ReadRequestBody(context.Request);
            logger.LogInformation($"Http Method: [{context.Request.Method}], Protoclol [{context.Request.Protocol}], Path [{context.Request.Path}], Request Body: [{requestBodyPayload}]");

            // Read and log response body data
            // Copy a pointer to the original response body stream
           var originalResponseBodyStream = context.Response.Body;

            // Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                // ...and use that for the temporary response body
                context.Response.Body = responseBody;

                // Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                var responseBodyPayload = await ReadResponseBody(context.Response);
                logger.LogInformation($"Response Body: {responseBodyPayload}");
                await responseBody.CopyToAsync(originalResponseBodyStream);

            }
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{responseBody}";
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }
    }
}
