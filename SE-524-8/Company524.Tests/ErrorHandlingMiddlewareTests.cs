using Company524.API.Middleware;
using Company524.Application.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Company524.Tests
{
    /// <summary>
    /// Tests for ErrorHandlingMiddleware.
    ///
    /// KEY CONCEPT — Testing Middleware:
    /// Middleware sits between requests and controllers. We test it by:
    ///   1. Creating a fake "next" delegate that throws a specific exception
    ///   2. Invoking the middleware with a real DefaultHttpContext
    ///   3. Asserting the response has the correct status code and body
    ///
    /// This tests the middleware contract without spinning up a full web server.
    /// </summary>
    public class ErrorHandlingMiddlewareTests
    {
        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new System.IO.MemoryStream();
            return context;
        }

        private static async Task<string> ReadResponseBody(DefaultHttpContext context)
        {
            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            return await new System.IO.StreamReader(context.Response.Body).ReadToEndAsync();
        }

        [Fact]
        public async Task InvokeAsync_WhenBadRequestExceptionThrown_Returns400()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new BadRequestException("Invalid input"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_Returns404()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new NotFoundException("Resource not found"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task InvokeAsync_WhenUnauthorizedExceptionThrown_Returns401()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new UnauthorizedException("Not authorized"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task InvokeAsync_WhenNotAllowedExceptionThrown_Returns403()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new NotAllowedException("Forbidden"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task InvokeAsync_WhenUnhandledExceptionThrown_Returns500()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new InvalidOperationException("Unexpected error"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_ResponseBodyContainsExceptionMessage()
        {
            const string errorMessage = "Specific error detail";
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new BadRequestException(errorMessage));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            var body = await ReadResponseBody(context);
            body.Should().Contain(errorMessage);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_ContentTypeIsJson()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new BadRequestException("Error"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            context.Response.ContentType.Should().Be("application/json");
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_ResponseBodyIsValidJson()
        {
            var middleware = new ErrorHandlingMiddleware(_ =>
                throw new NotFoundException("Not found"));

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            var body = await ReadResponseBody(context);
            var act = () => JsonDocument.Parse(body);

            act.Should().NotThrow("the response body must be valid JSON");
        }

        [Fact]
        public async Task InvokeAsync_WhenNoExceptionThrown_PassesThrough()
        {
            var nextWasCalled = false;
            var middleware = new ErrorHandlingMiddleware(_ =>
            {
                nextWasCalled = true;
                return Task.CompletedTask;
            });

            var context = CreateHttpContext();
            await middleware.InvokeAsync(context);

            nextWasCalled.Should().BeTrue("next delegate must be called when no exception occurs");
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
