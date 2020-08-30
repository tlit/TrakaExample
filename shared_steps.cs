using FluentAssertions;
using System.Net;
using TechTalk.SpecFlow;
using TrakaExample.Context;

namespace TrakaExample
{
    [Binding]
    public class shared_steps
    {
        private readonly ApiContext _apiContext;

        public shared_steps(ApiContext apiContext)
        {
            this._apiContext = apiContext;
        }

        [Then(@"the response code is (.*)")]
        public void ThenTheResponseCodeIs(int responseCodeExpected)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            switch (responseCodeExpected)
            {
                case 200:
                    statusCode = HttpStatusCode.OK;
                    break;
                case 201:
                    statusCode = HttpStatusCode.Created;
                    break;
                case 204:
                    statusCode = HttpStatusCode.NoContent;
                    break;
                case 400:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case 404:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case 500:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            _apiContext.response.StatusCode.Should().Be(statusCode);
        }
    }
}