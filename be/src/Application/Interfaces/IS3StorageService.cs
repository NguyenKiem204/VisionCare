using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VisionCare.Application.Interfaces;

public interface IS3StorageService
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType, string keyPrefix, CancellationToken cancellationToken = default);
    Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default);
}
