using FluentValidation;
using Microsoft.AspNetCore.Http;
using PeerDrop.Shared.Constants;

namespace PeerDrop.BLL.Validators.User;

public class UploadAvatarValidator : AbstractValidator<IFormFile>
{
    public UploadAvatarValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Avatar file is required");

        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage("Avatar file cannot be empty")
            .LessThanOrEqualTo(ProjectConstants.FileUpload.MaxAvatarSizeBytes)
            .WithMessage($"Avatar file size cannot exceed {ProjectConstants.FileUpload.MaxAvatarSizeBytes / 1024 / 1024}MB");

        RuleFor(x => x.FileName)
            .Must(HaveValidExtension)
            .WithMessage($"Invalid file extension. Allowed extensions: {string.Join(", ", ProjectConstants.FileUpload.AllowedAvatarExtensions)}");
    }

    private static bool HaveValidExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return false;

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return ProjectConstants.FileUpload.AllowedAvatarExtensions.Contains(extension);
    }
}
