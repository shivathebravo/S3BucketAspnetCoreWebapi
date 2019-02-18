using AwsS3BucketWebApi.Models;
using System.Threading.Tasks;

namespace AwsS3BucketWebApi.Services
{
    public interface IS3Service
    {
        Task<S3Response> CreateBucketAsync(string bucketName);
        Task UploadFileAsync(string bucketName);
    }
}
