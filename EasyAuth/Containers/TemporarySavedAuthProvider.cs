using EasyAuth.Abstractions;

namespace EasyAuth.Containers;

public class TemporarySavedAuthProvider : IAuthorizeProvider
{
    private readonly string _savePath;
    private readonly TemporaryAuthProvider _authProvider;
    private DateTime _lastAuth;

    public TemporarySavedAuthProvider(TemporaryAuthProvider authProvider, string savePath)
    {
        _authProvider = authProvider;
        _savePath = savePath;

        if (File.Exists(_savePath) && DateTime.TryParse(File.ReadAllText(_savePath), out DateTime date))
            _authProvider.LastAuth = date;

        _lastAuth = _authProvider.LastAuth;
    }

    public async Task<bool> AuthorizeTestAsync(CancellationToken cancel)
    {
        bool auth = await _authProvider.AuthorizeTestAsync(cancel);

        if (auth && _lastAuth != _authProvider.LastAuth)
        {
            File.WriteAllText(_savePath, _authProvider.LastAuth.ToString());
            _lastAuth = _authProvider.LastAuth;
        }

        return auth;
    }
}