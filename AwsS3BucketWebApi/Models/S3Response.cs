﻿using System.Net;

namespace AwsS3BucketWebApi.Models
{
    public class S3Response
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }
}
