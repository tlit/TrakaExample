using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;

namespace ApiTestAutomation_01
{
    public class Context
    {
        public HttpResponseMessage response;
        public User user;
        public UserListResponse userListResponse;
        public UserQueryResponse userQueryResponse;
    }

    public class User
    {
        public int id;
        public string email;
        public string first_name;
        public string last_name;
        public string avatar;
    }

    public class UserListResponse
    {
        public int page;
        public int per_page;
        public int total;
        public int total_pages;
        public User[] data;
    }

    public class UserQueryResponse
    {
        public User data;
    }

    [Binding]
    public class UserOperations_steps
    {
        static HttpClient client = new HttpClient();
        private readonly Uri baseUri = new Uri("https://reqres.in/api/");
        private Context context;
        
        public UserOperations_steps(Context context)
        {
            this.context = context;
        }

        [When(@"I request page (.*) of the User List")]
        public async Task WhenIRequestPageOfTheUserList(int pageNumber)
        {
            string path = $"users?page={pageNumber.ToString()}";
            string fullUri = $"{this.baseUri}{path}";
            context.response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, fullUri));
            try
            {
                context.userListResponse = await context.response.Content.ReadAsAsync<UserListResponse>();
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
            context.response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, fullUri));
            try
            {
                context.userQueryResponse = await context.response.Content.ReadAsAsync<UserQueryResponse>();
                context.user = context.userQueryResponse.data;
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
            context.response = await client.SendAsync(msg);
            try
            {
                context.user = await context.response.Content.ReadAsAsync<User>();
            }
            finally
            {
            }
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
            context.response.StatusCode.Should().Be(statusCode);
        }

        [Then(@"the response page is (.*)")]
        public void ThenTheResponsePageIs(int pageNumberExpected)
        {
            context.userListResponse.page.Should().Be(pageNumberExpected);
        }

        [Then(@"the response includes (.*) items per page")]
        public void ThenTheResponseIncludesItemsPerPage(int perPageExpected)
        {
            context.userListResponse.per_page.Should().Be(perPageExpected);
        }

        [Then(@"the response indicates (.*) items in total")]
        public void ThenTheResponseIndicatesItemsInTotal(int totalExpected)
        {
            context.userListResponse.total.Should().Be(totalExpected);
        }

        [Then(@"the response indicates (.*) pages in total")]
        public void ThenTheResponseIndicatesPagesInTotal(int totalPagesExpected)
        {
            context.userListResponse.total_pages.Should().Be(totalPagesExpected);
        }

        [Then(@"the response data includes (.*) Users")]
        public void ThenTheResponseDataIncludesUsers(int userCountExpected)
        {
            context.userListResponse.data.Length.Should().Be(userCountExpected);
        }

        [Then(@"the response contains a single User")]
        public void ThenTheResponseContainsASingleUser()
        {
            context.userQueryResponse.data.Should().BeOfType<User>();
        }

        [Then(@"the User's first name is (.*)")]
        public void ThenTheUserSFirstNameIs(string firstNameExpected)
        {
            context.user.first_name.Should().Be(firstNameExpected);
        }

        [Then(@"the User's last name is (.*)")]
        public void ThenTheUserSLastNameIs(string lastNameExpected)
        {
            context.user.last_name.Should().Be(lastNameExpected);
        }

        [Then(@"the User's email address is (.*)")]
        public void ThenTheUserSEmailAddressIs(string emailExpected)
        {
            context.user.email.Should().Be(emailExpected);
        }

        [Then(@"the User's avatar is a valid URI")]
        public void ThenTheUserSAvatarIsAValidURI()
        {
            Uri avatarUri = new Uri(context.user.avatar);
            avatarUri.Should().BeOfType<Uri>();
        }
    }
}