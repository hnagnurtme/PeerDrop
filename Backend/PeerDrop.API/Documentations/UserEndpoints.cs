namespace PeerDrop.API.Documentations;

public static class UserEndpoints
{
    public static class GetAllUsers
    {
        public const string Summary = "Get all users in the system";
        public const string Description = "Retrieve a list of all registered users. Requires authentication.";
    }

    public static class GetUserById
    {
        public const string Summary = "Get user by ID";
        public const string Description = "Retrieve user details by unique identifier (GUID).";
    }

    public static class GetUserByEmail
    {
        public const string Summary = "Get user by email address";
        public const string Description = "Find and retrieve user details using their email address.";
    }

    public static class UpdateUser
    {
        public const string Summary = "Update user information";
        public const string Description = "Update user profile information. User can only update their own profile.";
    }

    public static class DeleteUser
    {
        public const string Summary = "Delete user account";
        public const string Description = "Permanently delete a user account from the system.";
    }

    public static class UploadAvatar
    {
        public const string Summary = "Upload user avatar";
        public const string Description = """
            Upload or update user profile avatar image.
            
            **Accepted formats:** JPG, PNG, GIF
            **Maximum size:** 10MB
            **Image processing:** Automatically optimized for web
            """;
    }
}
