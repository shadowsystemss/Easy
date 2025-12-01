using EasyAuth.Abstractions;

namespace EasyAuth.Containers;

public class OrAuthProvider : IAuthorizeProvider
{
    private readonly IAuthorizeProvider[] _providers;

    public OrAuthProvider(IEnumerable<IAuthorizeProvider> providers) => _providers = [.. providers];

    public OrAuthProvider(params IAuthorizeProvider[] providers) => _providers = providers;
    public async Task<bool> AuthorizeTestAsync(CancellationToken ct)
    {
        var tasks = new List<Task<bool>>(_providers.Length);
        foreach (IAuthorizeProvider provider in _providers)
            tasks.Add(provider.AuthorizeTestAsync(ct));

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
