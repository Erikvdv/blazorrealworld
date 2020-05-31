using System.Threading;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services
{
    public interface IConduitApiService
    {
        Task<ArticleList> GetArticleListAsync(ArticleListFilter articleListFilter, string? token, CancellationToken cancellationToken = default);
        Task<ArticleList> GetArticleFeedAsync(ArticleListFilter articleListFilter, string token, CancellationToken cancellationToken = default);
        Task<Article> GetArticleAsync(string slug, string? token, CancellationToken cancellationToken = default);
        Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default);
        Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default);
        Task<User> LoginWithTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}