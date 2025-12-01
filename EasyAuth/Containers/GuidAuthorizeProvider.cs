using EasyAuth.Abstractions;

namespace EasyAuth.Containers;

public class GuidAuthorizeProvider(IGuidProvider guidProvider, IGuidAuthorizeProvider guidAuthorizeProvider) : IAuthorizeProvider
{
    private readonly IGuidProvider _guidProvider = guidProvider;
    private readonly IGuidAuthorizeProvider _guidAuthorizeProvider = guidAuthorizeProvider;

    public async Task<bool> AuthorizeTestAsync(CancellationToken ct)
        => await _guidAuthorizeProvider.AuthorizeTestAsync(await _guidProvider.GetGuidAsync(ct), ct);
}
