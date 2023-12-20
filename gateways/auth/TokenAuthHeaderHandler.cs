﻿using System.Net.Http.Headers;

namespace AHStats.gateways.auth;

public class TokenAuthHeaderHandler(ITokenProvider tokenProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", await tokenProvider.GetAccessTokenAsync());
        return await base.SendAsync(request, cancellationToken);
    }
}