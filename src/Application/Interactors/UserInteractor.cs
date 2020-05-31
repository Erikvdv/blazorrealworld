using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;

namespace Application.Interactors
{
    public class UserInteractor : IUserInteractor
    {
        private readonly IConduitApiService _conduitClient;
        private User? _user;

        public UserInteractor(IConduitApiService conduitClient)
        {
            _conduitClient = conduitClient ?? throw new System.ArgumentNullException(nameof(conduitClient));
        }

        public async Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default)
        {
            _user = await _conduitClient.LoginAsync(login, cancellationToken);
            return _user;
        }

        public async Task<User> LoginWithTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            _user = await _conduitClient.LoginWithTokenAsync(token, cancellationToken);
            return _user;
        }

    }
}
