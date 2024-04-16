using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Rainfall.API.Filters;

namespace Rainfall.API.Tests.Filters
{
    [TestFixture]
    public class ValidateModelStateAttributeTests
    {
        private ValidateModelStateAttribute _attribute;
        private ActionExecutingContext _actionExecutingContext;

        [SetUp]
        public void Setup()
        {
            _attribute = new ValidateModelStateAttribute();

            var services = new ServiceCollection();
            services.AddLogging(); // Add the required services
            var serviceProvider = services.BuildServiceProvider();

            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProvider }, // Assign the ServiceProvider
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            };

            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                new object());
        }

        [Test]
        public void OnActionExecuting_SetsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            _actionExecutingContext.ModelState.AddModelError("Test", "Test error");

            // Act
            _attribute.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.That(_actionExecutingContext.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void OnActionExecuting_DoesNotSetResult_WhenModelStateIsValid()
        {
            // Act
            _attribute.OnActionExecuting(_actionExecutingContext);

            // Assert
            Assert.That(_actionExecutingContext.Result, Is.Null);
        }
    }

}
