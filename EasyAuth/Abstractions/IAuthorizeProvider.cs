namespace EasyAuth.Abstractions;

public interface IAuthorizeProvider
{
    Task<bool> AuthorizeTestAsync(CancellationToken cancel);
}
