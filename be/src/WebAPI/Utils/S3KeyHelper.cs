using System;

namespace VisionCare.WebAPI.Utils;

public static class S3KeyHelper
{
    public static string? TryExtractObjectKey(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;
        // Expected: https://{bucket}.s3.{region}.amazonaws.com/{objectKey}
        var marker = ".amazonaws.com/";
        var idx = url.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
            return null;
        var start = idx + marker.Length;
        if (start >= url.Length)
            return null;
        return url.Substring(start);
    }
}
