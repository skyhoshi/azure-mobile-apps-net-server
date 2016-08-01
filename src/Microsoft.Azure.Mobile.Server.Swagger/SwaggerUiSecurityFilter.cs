// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Swashbuckle.Application;

namespace Microsoft.Azure.Mobile.Server
{
    internal class SwaggerUiSecurityFilter : DelegatingHandler
    {
        private HttpConfiguration configuration;

        public SwaggerUiSecurityFilter(HttpConfiguration config)
        {
            this.configuration = config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            IHttpRouteData routeData = this.configuration.Routes.GetRouteData(request);
            if (routeData?.Route?.Handler?.GetType() == typeof(SwaggerUiHandler))
            {
                response.Headers.Add("Content-Security-Policy", "connect-src 'self' online.swagger.io");
            }

            return response;
        }
    }
}
