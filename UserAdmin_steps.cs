using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Collections.Generic;
using TrakaExample.Context;
using TrakaExample.Models;

namespace UserAdmin
{
    [Binding]
    public class UserAdmin_steps
    {
        static HttpClient client = new HttpClient();
        private readonly Uri baseUri = new Uri("https://reqres.in/api/");
        private readonly ApiContext _apiContext;

        public UserAdmin_steps(ApiContext apiContext)
        {
            this._apiContext = apiContext;
        }

        [When(@"I request page (.*) of the User List")]
        public async Task WhenIRequestPageOfTheUserList(int pageNumber)
        {
            string path = $"users?page={pageNumber.ToString()}";
            string fullUri = $"{this.baseUri}{path}";
            _apiContext.response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, fullUri));
            try
            {
                _apiContext.userListResponse = await _apiContext.response.Content.ReadAsAsync<UserListResponse>();
            }
            finally
            {
            }
        }

        [When(@"I request details for User ID (.*)")]
        public async Task WhenIRequestDetailsForUserID(int userId)
        {
            string path = $"users/{userId}";
            string fullUri = $"{this.baseUri}{path}";
            _apiContext.response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, fullUri));
            try
            {
                _apiContext.userQueryResponse = await _apiContext.response.Content.ReadAsAsync<UserQueryResponse>();
                _apiContext.user = _apiContext.userQueryResponse.data;
            }
            finally
            {
            }
        }

        [When(@"I add the User '(.*) (.*)'")]
        public async Task WhenIAddTheUser(string firstName, string lastName)
        {
            string path = $"users";
            string fullUri = $"{this.baseUri}{path}";
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, fullUri);
            msg.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("first_name", firstName),
                new KeyValuePair<string, string>("last_name", lastName),
            });
            _apiContext.response = await client.SendAsync(msg);
            try
            {
                _apiContext.user = await _apiContext.response.Content.ReadAsAsync<User>();
            }
            finally
            {
            }
        }

        [When(@"I delete User (.*)")]
        public async Task WhenIDeleteUser(int userId)
        {
            string path = $"users/{userId}";
            string fullUri = $"{this.baseUri}{path}";
            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Delete, fullUri);
            _apiContext.response = await client.SendAsync(msg);
        }

        [Then(@"the response contains a single User")]
        public void ThenTheResponseContainsASingleUser()
        {
            _apiContext.userQueryResponse.data.Should().BeOfType<User>();
        }

        [Then(@"the response page is (.*)")]
        public void ThenTheResponsePageIs(int pageNumberExpected)
        {
            _apiContext.userListResponse.page.Should().Be(pageNumberExpected);
        }

        [Then(@"the response includes (.*) items per page")]
        public void ThenTheResponseIncludesItemsPerPage(int perPageExpected)
        {
            _apiContext.userListResponse.per_page.Should().Be(perPageExpected);
        }

        [Then(@"the response indicates (.*) items in total")]
        public void ThenTheResponseIndicatesItemsInTotal(int totalExpected)
        {
            _apiContext.userListResponse.total.Should().Be(totalExpected);
        }

        [Then(@"the response indicates (.*) pages in total")]
        public void ThenTheResponseIndicatesPagesInTotal(int totalPagesExpected)
        {
            _apiContext.userListResponse.total_pages.Should().Be(totalPagesExpected);
        }

        [Then(@"the response data includes (.*) Users")]
        public void ThenTheResponseDataIncludesUsers(int userCountExpected)
        {
            _apiContext.userListResponse.data.Length.Should().Be(userCountExpected);
        }

        [Then(@"the User's first name is (.*)")]
        public void ThenTheUserSFirstNameIs(string firstNameExpected)
        {
            _apiContext.user.first_name.Should().Be(firstNameExpected);
        }

        [Then(@"the User's last name is (.*)")]
        public void ThenTheUserSLastNameIs(string lastNameExpected)
        {
            _apiContext.user.last_name.Should().Be(lastNameExpected);
        }

        [Then(@"the User's email address is (.*)")]
        public void ThenTheUserSEmailAddressIs(string emailExpected)
        {
            _apiContext.user.email.Should().Be(emailExpected);
        }

        [Then(@"the User's avatar is a valid URI")]
        public void ThenTheUserSAvatarIsAValidURI()
        {
            Uri avatarUri = new Uri(_apiContext.user.avatar);
            avatarUri.Should().BeOfType<Uri>();
        }
    }
}