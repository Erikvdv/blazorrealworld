using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Interactors
{
    public interface IUserInteractor
    {
        Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default);
        Task<User> LoginWithTokenAsync(string token, CancellationToken cancellationToken = default);
    }

}
