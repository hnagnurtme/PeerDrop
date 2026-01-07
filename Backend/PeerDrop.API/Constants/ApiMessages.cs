namespace PeerDrop.API.Constants;

public static class ApiMessages
{
    public static class Auth
    {
        public const string LoginSuccessful = "Login successful";
        public const string RegistrationSuccessful = "Registration successful";
        public const string TokenRefreshed = "Token refreshed successfully";
    }
    
    public static class Users
    {
        public const string UsersRetrieved = "Users retrieved successfully";
        public const string UserRetrieved = "User retrieved successfully";
        public const string UserUpdated = "User updated successfully";
        public const string AvatarUploaded = "Avatar uploaded successfully";
    }
}
