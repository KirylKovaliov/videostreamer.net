using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using VideoStreamer.Net.Configuration;
using VideoStreamer.Net.Extensions;

namespace VideoStreamer.Net.Storage
{
    public class S3Storage : StorageBase
    {
        private IAmazonS3 _client;

        public override void ValidateConfig(StorageTypeElement config)
        {
            if (string.IsNullOrWhiteSpace(config.AccessKeyId))
                throw new ConfigurationErrorsException("VideoStreaming.Net configuration error. AccessKeyId isn't defined for S3 Storage");

            if (string.IsNullOrWhiteSpace(config.Region))
                throw new ConfigurationErrorsException("VideoStreaming.Net configuration error. Region isn't defined for S3 Storage");

            if (string.IsNullOrWhiteSpace(config.SecretAccessKey))
                throw new ConfigurationErrorsException("VideoStreaming.Net configuration error. SecretAccessKey isn't defined for S3 Storage");

            if (string.IsNullOrWhiteSpace(config.Bucket))
                throw new ConfigurationErrorsException("VideoStreaming.Net configuration error. Bucket isn't defined for S3 Storage");

            var credentials = new BasicAWSCredentials(config.AccessKeyId, config.SecretAccessKey);
            _client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(config.Region));
        }

        public override long GetLength(string url)
        {
            string file = GetFileName(url);

            var response = _client.GetObjectMetadata(Config.Bucket, file);
            return response.ContentLength;
        }

        public override byte[] Read(string url, long start, long end)
        {
            string file = GetFileName(url);
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = Config.Bucket,
                Key = file,
                ByteRange = new ByteRange(start, end)
            };
            var response = _client.GetObject(request);
            
            using (Stream responseStream = response.ResponseStream)
            {
                return responseStream.ToByteArray();
            }
        }

        protected string GetFileName(string url)
        {
            var result = url;
            if (result.StartsWith("/"))
                result = result.Substring(1);

            return result;
        }
    }
}
