namespace PeerDrop.Shared.Constants;

public class ProjectConstants
{
    public static class FileUpload
    {
        public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
        public const string DefaultFolder = "peerdrop";
    }
    public static class Cloudinary
    {
        public const string ResultOk = "ok";
        public const string ResultNotFound = "not found";
    }
}