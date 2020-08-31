using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using TrakaExample.Context;
using TrakaExample.Models;

namespace UserLogin
{
    [Binding]
    public class UserLogin_steps
    {

        static HttpClient client = new HttpClient();
        private readonly ApiContext _apiContext;

        public UserLogin_steps(ApiContext apiContext)
        {
            this._apiContext = apiContext;
        }

        [When(@"I login using valid credentials")]
        public async Task WhenILoginUsingValidCredentials()
        {
            string path = $"login";
            string fullUri = $"{_apiContext.baseUri}{path}";
            User user = new User
            {
                email = "george.bluth@reqres.in",
                password = "1234"
            };
            _apiContext.response = await client.PostAsJsonAsync<User>(fullUri, user);
            try
            {
                _apiContext.loginToken = await _apiContext.response.Content.ReadAsAsync<LoginToken>();
            }
            finally
            {
            }
        }
        
        [When(@"I login without a password")]
        public async Task WhenILoginWithoutAPassword()
        {
            string path = $"login";
            string fullUri = $"{_apiContext.baseUri}{path}";
            User user = new User
            {
                email = "george.bluth@reqres.in"
            };
            _apiContext.response = await client.PostAsJsonAsync<User>(fullUri, user);
            try
            {
                _apiContext.loginError = await _apiContext.response.Content.ReadAsAsync<LoginError>();
            }
            finally
            {
            }
        }

        [When(@"I login using an unrecognised user")]
        public async Task WhenILoginUsingAnUnrecognisedUser()
        {
            string path = $"login";
            string fullUri = $"{_apiContext.baseUri}{path}";
            User user = new User
            {
                email = "unknown_user",
                password = "1234"
            };
            _apiContext.response = await client.PostAsJsonAsync<User>(fullUri, user);
            try
            {
                _apiContext.loginError = await _apiContext.response.Content.ReadAsAsync<LoginError>();
            }
            finally
            {
            }
        }

        [Then(@"the response contains a token")]
        public void ThenTheResponseContainsAToken()
        {
            _apiContext.loginToken.Should().NotBeNull();
        }
        
        [Then(@"the response error is ""(.*)""")]
        public void ThenTheResponseErrorIs(string expectedError)
        {
            _apiContext.loginError.Should().NotBeNull();
            _apiContext.loginError.error.Should().Be(expectedError);
        }
    }
}
