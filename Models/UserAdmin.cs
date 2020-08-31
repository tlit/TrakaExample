namespace TrakaExample.Models
{
    public class User
    {
        public int id;
        public string email;
        public string password;
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
}
