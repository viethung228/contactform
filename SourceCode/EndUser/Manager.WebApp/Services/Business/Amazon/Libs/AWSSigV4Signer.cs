﻿using System;
using System.Text;
using RestSharp;

namespace Manager.WebApp.Services.Amazon
{
    public class AWSSigV4Signer

    {
        public virtual AWSSignerHelper AwsSignerHelper { get; set; }
        private AWSAuthenticationCredentials awsCredentials;

        /// <summary>
        /// Constructor for AWSSigV4Signer
        /// </summary>
        /// <param name="awsAuthenticationCredentials">AWS Developer Account Credentials</param>
        public AWSSigV4Signer(AWSAuthenticationCredentials awsAuthenticationCredentials)
        {
            awsCredentials = awsAuthenticationCredentials;
            AwsSignerHelper = new AWSSignerHelper();
        }

        /// <summary>
        /// Signs a Request with AWS Signature Version 4
        /// </summary>
        /// <param name="request">RestRequest which needs to be signed</param>
        /// <param name="host">Request endpoint</param>
        /// <returns>RestRequest with AWS Signature</returns>
        public IRestRequest Sign(IRestRequest request, string host, string serviceName)
        {
            DateTime signingDate = AwsSignerHelper.InitializeHeaders(request, host);
            string signedHeaders = AwsSignerHelper.ExtractSignedHeaders(request);

            string hashedCanonicalRequest = CreateCanonicalRequest(request, signedHeaders);

            string stringToSign = AwsSignerHelper.BuildStringToSign(signingDate,
                                                                    hashedCanonicalRequest,
                                                                    awsCredentials.Region, serviceName);

            string signature = AwsSignerHelper.CalculateSignature(stringToSign,
                                                                  signingDate,
                                                                  awsCredentials.SecretKey,
                                                                  awsCredentials.Region, serviceName);

            AwsSignerHelper.AddSignature(request,
                                         awsCredentials.AccessKeyId,
                                         signedHeaders,
                                         signature,
                                         awsCredentials.Region,
                                         serviceName,
                                         signingDate);

            return request;
        }

        private string CreateCanonicalRequest(IRestRequest restRequest, string signedHeaders)
        {
            var canonicalizedRequest = new StringBuilder();
            //Request Method
            canonicalizedRequest.AppendFormat("{0}\n", restRequest.Method);

            //CanonicalURI
            canonicalizedRequest.AppendFormat("{0}\n", AwsSignerHelper.ExtractCanonicalURIParameters(restRequest.Resource));

            //CanonicalQueryString
            canonicalizedRequest.AppendFormat("{0}\n", AwsSignerHelper.ExtractCanonicalQueryString(restRequest));

            //CanonicalHeaders
            canonicalizedRequest.AppendFormat("{0}\n", AwsSignerHelper.ExtractCanonicalHeaders(restRequest));

            //SignedHeaders
            canonicalizedRequest.AppendFormat("{0}\n", signedHeaders);

            // Hash(digest) the payload in the body
            canonicalizedRequest.AppendFormat(AwsSignerHelper.HashRequestBody(restRequest));

            string canonicalRequest = canonicalizedRequest.ToString();

            //Create a digest(hash) of the canonical request
            return AWSUtils.ToHex(AWSUtils.Hash(canonicalRequest));
        }
    }
}