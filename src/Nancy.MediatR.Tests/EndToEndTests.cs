using System;
using FluentAssertions;
using MediatR;
using Nancy.Testing;
using Xunit;

namespace Nancy.MediatR.Tests
{
    public class EndToEndTests
    {
        [Fact]
        public void ForRequest_should_throw_if_mediatr_not_set()
        {
            var module = new TestModule(new Mediator(null, null));
            module.SetMediatR(null);

            module.Invoking(y => y.ForRequest<TestMessage>("/path"))
                .ShouldThrow<InvalidOperationException>();

            module.Invoking(y => y.ForRequest<TestMessageWithResponse, string>("/path"))
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ForRequest_should_return_204_for_request_with_unit_response()
        {
            // Given
            var bootstrapper = new Bootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Put("/testmessage", with => {
                with.HttpRequest();
            });

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public void ForRequest_should_return_200_for_request_with_response()
        {
            // Given
            var bootstrapper = new Bootstrapper();
            var browser = new Browser(bootstrapper);

            // When
            var result = browser.Get("/testmessagewithresponse", with => {
                with.HttpRequest();
            });

            // Then
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
            result.Body.AsString().ShouldBeEquivalentTo("Testing");
        }
    }
}
