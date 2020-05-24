using System.Threading;
using System.Threading.Tasks;
using Application.Clients;
using Application.Models;

namespace Application.Interactors
{
    public class UserInteractor : IUserInteractor
    {
        private readonly IConduitClient _conduitClient;
        private User _user;

        public UserInteractor(IConduitClient conduitClient)
        {
            _conduitClient = conduitClient ?? throw new System.ArgumentNullException(nameof(conduitClient));
        }

        public async Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default)
        {
            _user = await _conduitClient.LoginAsync(login, cancellationToken);
            return _user;
        }

    }
}
