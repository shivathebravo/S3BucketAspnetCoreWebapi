using AwsS3BucketWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AwsS3BucketWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/S3Bucket")]
    public class S3BucketController : Controller
    {
        private readonly IS3Service _service;

        public S3BucketController(IS3Service service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("CreateBucket/{bucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string bucketName)
        {
            var response = await _service.CreateBucketAsync(bucketName);
            return Ok(response);
        }
        [HttpPost]
        [Route("AddFile/{bucketName}")]
        public async Task<IActionResult> AddFile([FromRoute] string bucketName)
        {
            await _service.UploadFileAsync(bucketName);
            return ok();
        }

    }
}
