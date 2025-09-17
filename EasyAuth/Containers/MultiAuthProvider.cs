using EasyAuth.Abstractions;

namespace EasyAuth.Containers;

public class MultiAuthProvider : IAuthorizeProvider
{
    private readonly IAuthorizeProvider[] _providers;

    public MultiAuthProvider(IEnumerable<IAuthorizeProvider> providers) => _providers = [.. providers];

    public MultiAuthProvider(params IAuthorizeProvider[] providers) => _providers = providers;
    public async Task<bool> AuthorizeTestAsync(CancellationToken cancel)
    {
        var tasks = new List<Task<bool>>(_providers.Length);
        foreach (IAuthorizeProvider provider in _providers)
            tasks.Add(provider.AuthorizeTestAsync(cancel));

        while (tasks.Count > 0)
        {
            Task<bool> task = await Task.WhenAny(tasks);
            if (task.Result)
                return true;

            tasks.Remove(task);
        }
        return false;
    }
}
