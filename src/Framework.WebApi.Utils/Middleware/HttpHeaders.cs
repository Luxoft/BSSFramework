namespace Framework.WebApi.Utils
{
    public static class HttpHeaders
    {
        public static string Correlation => "X-Correlation-ID";

        public static string ClientIp => "X-Client-IP";

        public static string ContentType => "Content-Type";

        public static string ContentDisposition => "Content-Disposition";
    }
}
