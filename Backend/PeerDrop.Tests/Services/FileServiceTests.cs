using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using PeerDrop.BLL.Exceptions;
using PeerDrop.BLL.Interfaces.Services;
using PeerDrop.BLL.Services;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.DTOs.Files;
using Xunit;

namespace PeerDrop.Tests.Services;

public class FileServiceTests
{
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly FileService _fileService;

    public FileServiceTests()
    {
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _fileService = new FileService(_fileStorageServiceMock.Object);
    }

    #region UploadFileAsync Tests

    [Fact]
    public async Task UploadFileAsync_WithValidFile_ShouldReturnFileResponse()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Test file content";
        var fileName = "test.txt";
        var contentType = "text/plain";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        ms.Position = 0;

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.ContentType).Returns(contentType);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        var expectedResponse = new FileResponse
        {
            PublicId = "peerdrop/test123",
            SecureUrl = "https://cloudinary.com/image/test123"
        };

        _fileStorageServiceMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), fileName, contentType, default))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _fileService.UploadFileAsync(fileMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.PublicId.Should().Be("peerdrop/test123");
        result.SecureUrl.Should().Be("https://cloudinary.com/image/test123");
        
        _fileStorageServiceMock.Verify(x => x.UploadAsync(
            It.IsAny<Stream>(), 
            fileName, 
            contentType, 
            default), Times.Once);
    }

    [Fact]
    public async Task UploadFileAsync_WithNullFile_ShouldThrowBusinessException()
    {
        // Arrange
        IFormFile? nullFile = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _fileService.UploadFileAsync(nullFile!));

        exception.Message.Should().Be(ErrorMessages.InvalidFile);
        exception.ErrorCode.Should().Be(ErrorCodes.InvalidFile);
        
        _fileStorageServiceMock.Verify(x => x.UploadAsync(
            It.IsAny<Stream>(), 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            default), Times.Never);
    }

    [Fact]
    public async Task UploadFileAsync_WithEmptyFile_ShouldThrowBusinessException()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _fileService.UploadFileAsync(fileMock.Object));

        exception.Message.Should().Be(ErrorMessages.InvalidFile);
        exception.ErrorCode.Should().Be(ErrorCodes.InvalidFile);
        
        _fileStorageServiceMock.Verify(x => x.UploadAsync(
            It.IsAny<Stream>(), 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            default), Times.Never);
    }

    [Fact]
    public async Task UploadFileAsync_WithFileTooLarge_ShouldThrowFileTooLargeException()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "large-file.txt";
        var largeSize = ProjectConstants.FileUpload.MaxFileSizeBytes + 1; // Exceeds limit
        
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(largeSize);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FileTooLargeException>(
            () => _fileService.UploadFileAsync(fileMock.Object));

        exception.Message.Should().Be(ErrorMessages.FileTooLarge);
        exception.ErrorCode.Should().Be(ErrorCodes.FileTooLarge);
        
        _fileStorageServiceMock.Verify(x => x.UploadAsync(
            It.IsAny<Stream>(), 
            It.IsAny<string>(), 
            It.IsAny<string>(), 
            default), Times.Never);
    }

    [Fact]
    public async Task UploadFileAsync_WhenStorageServiceThrowsException_ShouldPropagateException()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "Test file content";
        var fileName = "test.txt";
        var contentType = "text/plain";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        ms.Position = 0;

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.ContentType).Returns(contentType);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        _fileStorageServiceMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), fileName, contentType, default))
            .ThrowsAsync(new Exception("Upload failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _fileService.UploadFileAsync(fileMock.Object));

        exception.Message.Should().Be("Upload failed");
    }

    #endregion

    #region DeleteFileByPublicIdAsync Tests

    [Fact]
    public async Task DeleteFileByPublicIdAsync_WithValidPublicId_ShouldReturnTrue()
    {
        // Arrange
        var publicId = "peerdrop/test123";

        _fileStorageServiceMock
            .Setup(x => x.DeleteAsync(publicId, default))
            .ReturnsAsync(true);

        // Act
        var result = await _fileService.DeleteFileByPublicIdAsync(publicId);

        // Assert
        result.Should().BeTrue();
        
        _fileStorageServiceMock.Verify(x => x.DeleteAsync(publicId, default), Times.Once);
    }

    [Fact]
    public async Task DeleteFileByPublicIdAsync_WhenFileNotFound_ShouldReturnTrue()
    {
        // Arrange
        var publicId = "peerdrop/nonexistent";

        _fileStorageServiceMock
            .Setup(x => x.DeleteAsync(publicId, default))
            .ReturnsAsync(true);

        // Act
        var result = await _fileService.DeleteFileByPublicIdAsync(publicId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteFileByPublicIdAsync_WithNullPublicId_ShouldReturnFalse()
    {
        // Act
        var result = await _fileService.DeleteFileByPublicIdAsync(null!);

        // Assert
        result.Should().BeFalse();
        
        _fileStorageServiceMock.Verify(x => x.DeleteAsync(It.IsAny<string>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteFileByPublicIdAsync_WithEmptyPublicId_ShouldReturnFalse()
    {
        // Act
        var result = await _fileService.DeleteFileByPublicIdAsync(string.Empty);

        // Assert
        result.Should().BeFalse();
        
        _fileStorageServiceMock.Verify(x => x.DeleteAsync(It.IsAny<string>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteFileByPublicIdAsync_WithWhitespacePublicId_ShouldReturnFalse()
    {
        // Act
        var result = await _fileService.DeleteFileByPublicIdAsync("   ");

        // Assert
        result.Should().BeFalse();
        
        _fileStorageServiceMock.Verify(x => x.DeleteAsync(It.IsAny<string>(), default), Times.Never);
    }

    [Fact]
    public async Task DeleteFileByPublicIdAsync_WhenDeleteFails_ShouldReturnFalse()
    {
        // Arrange
        var publicId = "peerdrop/test123";

        _fileStorageServiceMock
            .Setup(x => x.DeleteAsync(publicId, default))
            .ReturnsAsync(false);

        // Act
        var result = await _fileService.DeleteFileByPublicIdAsync(publicId);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}