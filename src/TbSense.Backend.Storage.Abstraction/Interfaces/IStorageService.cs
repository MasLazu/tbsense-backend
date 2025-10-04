namespace TbSense.Backend.Storage.Abstraction.Interfaces;

public interface IStorageService
{
    /// <summary>
    /// Downloads a file from storage
    /// </summary>
    /// <param name="path">The file path in storage</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>File as a stream</returns>
    Task<Stream> DownloadAsync(string path, CancellationToken ct = default);

    /// <summary>
    /// Uploads a file to storage
    /// </summary>
    /// <param name="path">The file path in storage</param>
    /// <param name="fileStream">The file stream to upload</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The storage path of the uploaded file</returns>
    Task<string> UploadAsync(string path, Stream fileStream, CancellationToken ct = default);

    /// <summary>
    /// Deletes a file from storage
    /// </summary>
    /// <param name="path">The file path in storage</param>
    /// <param name="ct">Cancellation token</param>
    Task DeleteAsync(string path, CancellationToken ct = default);

    /// <summary>
    /// Checks if a file exists in storage
    /// </summary>
    /// <param name="path">The file path in storage</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if the file exists</returns>
    Task<bool> ExistsAsync(string path, CancellationToken ct = default);

    /// <summary>
    /// Gets the file size
    /// </summary>
    /// <param name="path">The file path in storage</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>File size in bytes</returns>
    Task<long> GetFileSizeAsync(string path, CancellationToken ct = default);
}
