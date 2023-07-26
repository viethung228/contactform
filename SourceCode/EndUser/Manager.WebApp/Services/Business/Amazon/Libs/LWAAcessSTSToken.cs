namespace Manager.WebApp.Services.Amazon
{
    public class STSAssumedRoleUser
    {
        public string Arn { get; set; }
        public string AssumedRoleId { get; set; }
    }

    public class STSAssumeRoleResponse
    {
        public STSAssumeRoleResult AssumeRoleResult { get; set; }
        public STSResponseMetadata ResponseMetadata { get; set; }
    }

    public class STSAssumeRoleResult
    {
        public STSAssumedRoleUser AssumedRoleUser { get; set; }
        public STSCredentials Credentials { get; set; }
        public object PackedPolicySize { get; set; }
        public object SourceIdentity { get; set; }
    }

    public class STSCredentials
    {
        public string AccessKeyId { get; set; }
        public double Expiration { get; set; }
        public string SecretAccessKey { get; set; }
        public string SessionToken { get; set; }
    }

    public class STSResponseMetadata
    {
        public string RequestId { get; set; }
    }

    public class STSTAccessToken
    {
        public STSAssumeRoleResponse AssumeRoleResponse { get; set; }
    }
}
