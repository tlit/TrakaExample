using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;
using TrakaExample.Context;

namespace UserLogin
{
    public class LoginToken
    {
        public string token;
    }

    public class LoginError
    {
        public string error;
    }

    [Binding]
    public class UserLogin_steps
    {

        static HttpClient client = new HttpClient();
        private readonly Uri baseUri = new Uri("https://reqres.in/api/");
        private readonly TrakaExample.Context.ApiContext _apiContext;

        public UserLogin_steps(TrakaExample.Context.ApiContext apiContext)
        {
            this._apiContext = apiContext;
        }

        [When(@"I login using valid credentials")]
        public async Task WhenILoginUsingValidCredentials()
        {
            string path = $"login";
            string fullUri = $"{this.baseUri}{path}";
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, fullUri);
            msg.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", "george.bluth@reqres.in"),
                new KeyValuePair<string, string>("password", "1234"),
            });
            _apiContext.response = await client.SendAsync(msg);
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
            string fullUri = $"{this.baseUri}{path}";
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, fullUri);
            msg.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", "george.bluth@reqres.in")
            });
            _apiContext.response = await client.SendAsync(msg);
            try
            {
                _apiContext.loginError= await _apiContext.response.Content.ReadAsAsync<LoginError>();
            }
            finally
            {
            }
        }

        [When(@"I login using an unrecognised user")]
        public async Task WhenILoginUsingAnUnrecognisedUser()
        {
            string path = $"login";
            string fullUri = $"{this.baseUri}{path}";
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, fullUri);
            msg.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", "unknown_user"),
                new KeyValuePair<string, string>("password", "1234"),
            });
            _apiContext.response = await client.SendAsync(msg);
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
