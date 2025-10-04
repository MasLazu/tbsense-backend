using FluentStorage;
using FluentStorage.Blobs;
using TbSense.Backend.Storage.Abstraction.Interfaces;
using Microsoft.Extensions.Logging;

namespace TbSense.Backend.Storage.Services;

public class StorageService : IStorageService
{
    private readonly IBlobStorage _blobStorage;
    private readonly ILogger<StorageService> _logger;

    public StorageService(
        IBlobStorage blobStorage,
        ILogger<StorageService> logger)
    {
        _blobStorage = blobStorage;
        _logger = logger;
    }

    public async Task<Stream> DownloadAsync(string path, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Downloading file from {Path}", path);

            var stream = new MemoryStream();
            await _blobStorage.ReadToStreamAsync(path, stream, ct);
            stream.Position = 0;

            _logger.LogInformation("Successfully downloaded file from {Path}, size: {Size} bytes",
                path, stream.Length);

            return stream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file from {Path}", path);
            throw;
        }
    }

    public async Task<string> UploadAsync(string path, Stream fileStream, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Uploading file to {Path}", path);

            await _blobStorage.WriteAsync(path, fileStream, false, ct);

            _logger.LogInformation("Successfully uploaded file to {Path}", path);

            return path;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file to {Path}", path);
            throw;
        }
    }

    public async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Deleting file from {Path}", path);

            await _blobStorage.DeleteAsync(new[] { path }, ct);

            _logger.LogInformation("Successfully deleted file from {Path}", path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file from {Path}", path);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string path, CancellationToken ct = default)
    {
        try
        {
            IReadOnlyCollection<Blob> blobs = await _blobStorage.ListAsync(new ListOptions { FilePrefix = path, MaxResults = 1 }, ct);
            return blobs.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of file at {Path}", path);
            throw;
        }
    }

    public async Task<long> GetFileSizeAsync(string path, CancellationToken ct = default)
    {
        try
        {
            IReadOnlyCollection<Blob> blobs = await _blobStorage.ListAsync(new ListOptions { FilePrefix = path, MaxResults = 1 }, ct);

            Blob? blob = blobs.FirstOrDefault() ?? throw new FileNotFoundException($"File not found at path: {path}");

            return blob.Size ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file size for {Path}", path);
            throw;
        }
    }
}
