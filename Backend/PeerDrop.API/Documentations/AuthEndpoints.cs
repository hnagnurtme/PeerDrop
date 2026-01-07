namespace PeerDrop.API.Documentations;

public static class AuthEndpoints
{
    public static class Login
    {
        public const string Summary = "Authenticate user and generate JWT tokens";
        public const string Description = """
            Authenticate user credentials and return JWT access token and refresh token.

            **Requirements:**
            - Valid email format
            - Password minimum 6 characters

            **Rate Limit:** 5 attempts per minute
            """;
    }

    public static class Register
    {
        public const string Summary = "Register a new user account";
        public const string Description = """
            Create a new user account with email, password, full name, and username.

            **Requirements:**
            - Unique email address
            - Password minimum 6 characters
            - Valid full name and username

            **Returns:** JWT tokens for automatic login
            """;
    }

    public static class RefreshToken
    {
        public const string Summary = "Refresh expired access token";
        public const string Description = """
            Generate new access token using refresh token.

            **Requirements:**
            - Valid refresh token
            - User ID must match token

            **Returns:** New JWT access token and refresh token
            """;
    }

    public static class Logout
    {
        public const string Summary = "Logout current user";
        public const string Description = """
            Invalidate current user session and tokens.

            **Requires:** Valid JWT token in Authorization header
            """;
    }
}