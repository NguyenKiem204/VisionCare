using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VisionCare.Application.Interfaces;

namespace VisionCare.Infrastructure.Services;

public class S3StorageService : IS3StorageService
{
    private readonly ILogger<S3StorageService> _logger;
    private readonly IConfiguration _configuration;

    public S3StorageService(ILogger<S3StorageService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        string keyPrefix,
        CancellationToken cancellationToken = default
    )
    {
        if (content == null)
        {
            throw new ArgumentException("Content stream is null", nameof(content));
        }

        var bucket = _configuration["AWS:BucketName"];
        var region = _configuration["AWS:Region"];
        var accessKey = _configuration["AWS:AccessKeyId"];
        var secretKey = _configuration["AWS:SecretAccessKey"];
        if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(region))
        {
            throw new InvalidOperationException(
                "AWS configuration is missing. Please set AWS:BucketName and AWS:Region."
            );
        }

        var awsRegion = RegionEndpoint.GetBySystemName(region);
        using var s3 = CreateClient(accessKey, secretKey, awsRegion);

        var extension = Path.GetExtension(fileName);
        var objectKey = $"{keyPrefix.Trim('/')}/{Guid.NewGuid():N}{extension}";
        var put = new PutObjectRequest
        {
            BucketName = bucket,
            Key = objectKey,
            InputStream = content,
            ContentType = string.IsNullOrWhiteSpace(contentType)
                ? "application/octet-stream"
                : contentType,
            CannedACL = S3CannedACL.PublicRead,
        };
        put.Metadata["x-amz-meta-original-name"] = fileName;

        var resp = await s3.PutObjectAsync(put, cancellationToken);
        _logger.LogInformation(
            "S3 PutObject {Bucket}/{Key} -> {Status}",
            bucket,
            objectKey,
            resp.HttpStatusCode
        );

        var url = $"https://{bucket}.s3.{awsRegion.SystemName}.amazonaws.com/{objectKey}";
        return url;
    }

    public async Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(objectKey))
        {
            throw new ArgumentException("Object key is required", nameof(objectKey));
        }

        var bucket = _configuration["AWS:BucketName"];
        var region = _configuration["AWS:Region"];
        var accessKey = _configuration["AWS:AccessKeyId"];
        var secretKey = _configuration["AWS:SecretAccessKey"];
        if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(region))
        {
            throw new InvalidOperationException(
                "AWS configuration is missing. Please set AWS:BucketName and AWS:Region."
            );
        }
        var awsRegion = RegionEndpoint.GetBySystemName(region);
        using var s3 = CreateClient(accessKey, secretKey, awsRegion);

        var del = new DeleteObjectRequest { BucketName = bucket, Key = objectKey };
        var resp = await s3.DeleteObjectAsync(del, cancellationToken);
        _logger.LogInformation(
            "S3 DeleteObject {Bucket}/{Key} -> {Status}",
            bucket,
            objectKey,
            resp.HttpStatusCode
        );
    }

    private static IAmazonS3 CreateClient(
        string? accessKey,
        string? secretKey,
        RegionEndpoint region
    )
    {
        if (!string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secretKey))
        {
            var creds = new BasicAWSCredentials(accessKey, secretKey);
            return new AmazonS3Client(creds, region);
        }
        return new AmazonS3Client(region);
    }
}
