using System.Net.Http;
using UserAdmin;

namespace TrakaExample.Context
{
    public class ApiContext
    {
        public HttpResponseMessage response;
        public User user;
        public UserListResponse userListResponse;
        public UserQueryResponse userQueryResponse;
        public LoginToken loginToken;
        public LoginError loginError;

        public ApiContext()
        {
            response = new HttpResponseMessage();
            user = new User();
            userListResponse = new UserListResponse();
            userQueryResponse = new UserQueryResponse();
            loginToken = new LoginToken();
            loginError = new LoginError();
        }
    }
}
