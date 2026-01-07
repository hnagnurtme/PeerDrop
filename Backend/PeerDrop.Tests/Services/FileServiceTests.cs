// using System.Net;
// using CloudinaryDotNet;
// using CloudinaryDotNet.Actions;
// using FluentAssertions;
// using Microsoft.AspNetCore.Http;
// using Moq;
// using PeerDrop.BLL.Exceptions;
// using PeerDrop.BLL.Services;
// using PeerDrop.Shared.Constants;
// using Xunit;
//
// namespace PeerDrop.Tests.Services;
//
// public class FileServiceTests
// {
//     private readonly Mock<Cloudinary> _cloudinaryMock;
//     private readonly FileService _fileService;
//
//     public FileServiceTests()
//     {
//         _cloudinaryMock = new Mock<Cloudinary>();
//         _fileService = new FileService(_cloudinaryMock.Object);
//     }
//
//     #region UploadFileAsync Tests
//
//     [Fact]
//     public async Task UploadFileAsync_WithValidFile_ShouldReturnFileResponse()
//     {
//         // Arrange
//         var fileMock = new Mock<IFormFile>();
//         var content = "Test file content";
//         var fileName = "test.txt";
//         var ms = new MemoryStream();
//         var writer = new StreamWriter(ms);
//         await writer.WriteAsync(content);
//         await writer.FlushAsync();
//         ms.Position = 0;
//
//         fileMock.Setup(f => f.FileName).Returns(fileName);
//         fileMock.Setup(f => f.Length).Returns(ms.Length);
//         fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
//
//         var uploadResult = new ImageUploadResult
//         {
//             StatusCode = HttpStatusCode.OK,
//             PublicId = "peerdrop/test123",
//             SecureUrl = new Uri("https://cloudinary.com/image/test123")
//         };
//
//         _cloudinaryMock
//             .Setup(x => x.UploadAsync(It.IsAny<AutoUploadParams>(), default))
//             .ReturnsAsync(uploadResult);
//
//         // Act
//         var result = await _fileService.UploadFileAsync(fileMock.Object);
//
//         // Assert
//         result.Should().NotBeNull();
//         result.PublicId.Should().Be("peerdrop/test123");
//         result.SecureUrl.Should().Be("https://cloudinary.com/image/test123");
//         
//         _cloudinaryMock.Verify(x => x.UploadAsync(
//             It.Is<AutoUploadParams>(p => 
//                 p.Folder == "peerdrop" && 
//                 p.UseFilename == true && 
//                 p.UniqueFilename == true && 
//                 p.Overwrite == false), 
//             default), Times.Once);
//     }
//
//     [Fact]
//     public async Task UploadFileAsync_WithNullFile_ShouldThrowBusinessException()
//     {
//         // Arrange
//         IFormFile? nullFile = null;
//
//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<BusinessException>(
//             () => _fileService.UploadFileAsync(nullFile!));
//
//         exception.Message.Should().Be(ErrorMessages.CloudStorageForbidden);
//         exception.ErrorCode.Should().Be(ErrorCodes.CloudStorageForbidden);
//     }
//
//     [Fact]
//     public async Task UploadFileAsync_WithEmptyFile_ShouldThrowBusinessException()
//     {
//         // Arrange
//         var fileMock = new Mock<IFormFile>();
//         fileMock.Setup(f => f.Length).Returns(0);
//
//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<BusinessException>(
//             () => _fileService.UploadFileAsync(fileMock.Object));
//
//         exception.Message.Should().Be(ErrorMessages.CloudStorageForbidden);
//         exception.ErrorCode.Should().Be(ErrorCodes.CloudStorageForbidden);
//     }
//
//     [Fact]
//     public async Task UploadFileAsync_WithFileTooLarge_ShouldThrowFileTooLargeException()
//     {
//         // Arrange
//         var fileMock = new Mock<IFormFile>();
//         var fileName = "large-file.txt";
//         var largeSize = 11 * 1024 * 1024; // 11 MB (exceeds 10 MB limit)
//         
//         fileMock.Setup(f => f.FileName).Returns(fileName);
//         fileMock.Setup(f => f.Length).Returns(largeSize);
//
//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<FileTooLargeException>(
//             () => _fileService.UploadFileAsync(fileMock.Object));
//
//         exception.Message.Should().Be(ErrorMessages.FileTooLarge);
//         exception.ErrorCode.Should().Be(ErrorCodes.FileTooLarge);
//     }
//
//     [Fact]
//     public async Task UploadFileAsync_WhenCloudinaryFails_ShouldThrowException()
//     {
//         // Arrange
//         var fileMock = new Mock<IFormFile>();
//         var content = "Test file content";
//         var fileName = "test.txt";
//         var ms = new MemoryStream();
//         var writer = new StreamWriter(ms);
//         await writer.WriteAsync(content);
//         await writer.FlushAsync();
//         ms.Position = 0;
//
//         fileMock.Setup(f => f.FileName).Returns(fileName);
//         fileMock.Setup(f => f.Length).Returns(ms.Length);
//         fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
//
//         var uploadResult = new ImageUploadResult
//         {
//             StatusCode = HttpStatusCode.BadRequest,
//             Error = new Error { Message = "Upload failed" }
//         };
//
//         _cloudinaryMock
//             .Setup(x => x.UploadAsync(It.IsAny<AutoUploadParams>(), default))
//             .ReturnsAsync(uploadResult);
//
//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<Exception>(
//             () => _fileService.UploadFileAsync(fileMock.Object));
//
//         exception.Message.Should().Contain("Upload failed");
//     }
//
//     #endregion
//
//     #region DeleteFileByPublicIdAsync Tests
//
//     [Fact]
//     public async Task DeleteFileByPublicIdAsync_WithValidPublicId_ShouldReturnTrue()
//     {
//         // Arrange
//         var publicId = "peerdrop/test123";
//         var deleteResult = new DeletionResult
//         {
//             Result = "ok"
//         };
//
//         _cloudinaryMock
//             .Setup(x => x.DestroyAsync(It.IsAny<DeletionParams>()))
//             .ReturnsAsync(deleteResult);
//
//         // Act
//         var result = await _fileService.DeleteFileByPublicIdAsync(publicId);
//
//         // Assert
//         result.Should().BeTrue();
//         
//         _cloudinaryMock.Verify(x => x.DestroyAsync(
//             It.Is<DeletionParams>(p => 
//                 p.PublicId == publicId && 
//                 p.ResourceType == ResourceType.Auto)), 
//             Times.Once);
//     }
//
//     [Fact]
//     public async Task DeleteFileByPublicIdAsync_WhenFileNotFound_ShouldReturnTrue()
//     {
//         // Arrange
//         var publicId = "peerdrop/nonexistent";
//         var deleteResult = new DeletionResult
//         {
//             Result = "not found"
//         };
//
//         _cloudinaryMock
//             .Setup(x => x.DestroyAsync(It.IsAny<DeletionParams>()))
//             .ReturnsAsync(deleteResult);
//
//         // Act
//         var result = await _fileService.DeleteFileByPublicIdAsync(publicId);
//
//         // Assert
//         result.Should().BeTrue();
//     }
//
//     [Fact]
//     public async Task DeleteFileByPublicIdAsync_WithNullPublicId_ShouldReturnFalse()
//     {
//         // Act
//         var result = await _fileService.DeleteFileByPublicIdAsync(null!);
//
//         // Assert
//         result.Should().BeFalse();
//         
//         _cloudinaryMock.Verify(x => x.DestroyAsync(It.IsAny<DeletionParams>()), Times.Never);
//     }
//
//     [Fact]
//     public async Task DeleteFileByPublicIdAsync_WithEmptyPublicId_ShouldReturnFalse()
//     {
//         // Act
//         var result = await _fileService.DeleteFileByPublicIdAsync(string.Empty);
//
//         // Assert
//         result.Should().BeFalse();
//         
//         _cloudinaryMock.Verify(x => x.DestroyAsync(It.IsAny<DeletionParams>()), Times.Never);
//     }
//
//     [Fact]
//     public async Task DeleteFileByPublicIdAsync_WithWhitespacePublicId_ShouldReturnFalse()
//     {
//         // Act
//         var result = await _fileService.DeleteFileByPublicIdAsync("   ");
//
//         // Assert
//         result.Should().BeFalse();
//         
//         _cloudinaryMock.Verify(x => x.DestroyAsync(It.IsAny<DeletionParams>()), Times.Never);
//     }
//
//     [Fact]
//     public async Task DeleteFileByPublicIdAsync_WhenDeleteFails_ShouldReturnFalse()
//     {
//         // Arrange
//         var publicId = "peerdrop/test123";
//         var deleteResult = new DeletionResult
//         {
//             Result = "error"
//         };
//
//         _cloudinaryMock
//             .Setup(x => x.DestroyAsync(It.IsAny<DeletionParams>()))
//             .ReturnsAsync(deleteResult);
//
//         // Act
//         var result = await _fileService.DeleteFileByPublicIdAsync(publicId);
//
//         // Assert
//         result.Should().BeFalse();
//     }
//
//     #endregion
// }