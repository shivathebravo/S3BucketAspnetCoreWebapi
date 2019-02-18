using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AwsS3BucketWebApi.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
namespace AwsS3BucketWebApi.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _client.PutBucketAsync(putBucketRequest);
                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        Status = response.HttpStatusCode

                    };
                }

            }
            catch (AmazonS3Exception e)
            {
                return new S3Response
                {
                    Status = e.StatusCode,
                    Message = e.Message
                };
            }
            catch (Exception e)
            {
                return new S3Response
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = e.Message
                };
            }
            return new S3Response
            {
                Status = HttpStatusCode.InternalServerError,
                Message = "Something went wrong"
            };
        }


        private const string filePath = "C:\\Users\\Shiva\\Desktop\\Temp\\logo.jpg";
        private const string uploadWithKeyName = "UploadWithKeyName";
        private const string filesStreamUpload = "FileStreamUpload";
        private const string advancedUpload = "AdvancedUpload";

        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_client);
                //option1
                await fileTransferUtility.UploadAsync(filePath, bucketName);
                //option2
                await fileTransferUtility.UploadAsync(filePath, bucketName, uploadWithKeyName);
                //option3
                using (var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, filesStreamUpload);
                }
                //option4
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456,
                    Key = advancedUpload,
                    CannedACL = S3CannedACL.NoACL
                };

                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {
                System.Console.WriteLine("Error encountered on server. Message'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error encountered on server. Message'{0}' when writtign to an object", e.Message);
            }
        }

    }

}


