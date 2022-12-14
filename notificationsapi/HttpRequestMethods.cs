namespace NotificationsApi {
    public static class HttpRequestMethods
        {
            //
            // Summary:
            //     Represents the HTTP CONNECT protocol method that is used with a proxy that can
            //     dynamically switch to tunneling, as in the case of SSL tunneling.
            public const string Connect = "CONNECT";
            //
            // Summary:
            //     Represents an HTTP GET protocol method.
            public const string Get = "GET";
            //
            // Summary:
            //     Represents an HTTP HEAD protocol method. The HEAD method is identical to GET
            //     except that the server only returns message-headers in the response, without
            //     a message-body.
            public const string Head = "HEAD";
            //
            // Summary:
            //     Represents an HTTP MKCOL request that creates a new collection (such as a collection
            //     of pages) at the location specified by the request-Uniform Resource Identifier
            //     (URI).
            public const string MkCol = "MKCOL";
            //
            // Summary:
            //     Represents an HTTP POST protocol method that is used to post a new entity as
            //     an addition to a URI.
            public const string Post = "POST";
            //
            // Summary:
            //     Represents an HTTP PUT protocol method that is used to replace an entity identified
            //     by a URI.
            public const string Put = "PUT";
            //
            // Summary:
            //     HTTP "DELETE" method.
            public const string Delete = "DELETE";
            //
            // Summary:
            //     HTTP "PATCH" method.
            public const string Patch = "PATCH";
            //
            // Summary:
            //     HTTP "TRACE" method.
            public const string Trace = "TRACE";
            //
            // Summary:
            //     HTTP "OPTIONS" method.
            public const string Options = "OPTIONS";
        }
}