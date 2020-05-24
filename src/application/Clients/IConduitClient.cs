using System.Threading;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Clients
{
    public interface IConduitClient
    {
        Task<ArticleList> GetArticleListAsync(ArticleListFilter articleListFilter, CancellationToken cancellationToken = default);
        Task<ArticleList> GetArticleFeedAsync(ArticleListFilter articleListFilter, string token, CancellationToken cancellationToken = default);
        Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default);
        Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default);
        Task<User> LoginWithTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}