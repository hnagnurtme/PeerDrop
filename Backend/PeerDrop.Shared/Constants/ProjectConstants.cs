namespace PeerDrop.Shared.Constants;

public static class ProjectConstants
{
    public static class FileUpload
    {
        public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
        public const long MaxAvatarSizeBytes = 5 * 1024 * 1024; // 5 MB
        public const string DefaultFolder = "peerdrop";
        public static readonly string[] AllowedAvatarExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    }
    public static class Cloudinary
    {
        public const string ResultOk = "ok";
        public const string ResultNotFound = "not found";
    }
    public static class CorsConstants
    {
        public const string AllowDevPolicy = "AllowDev";       
        public const string AllowProductionPolicy = "AllowProduction"; 
    }
}