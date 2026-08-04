using Auth_Service.Data.Models;

namespace Auth_Service.Data
{
    public class UserDTO
    {
        public required string username { get; set; }
        public required string password { get; set; }

        public UserDTO() { }
        public UserDTO(User user) =>
            (username, password) = (user.username, user.password);
    }
}
