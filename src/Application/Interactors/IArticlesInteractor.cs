using System.Threading;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Interactors
{
    public interface IArticlesInteractor
    {
        Task<ArticleList> GetArticleListAsync(ArticleListFilter articleListFilter, string? token, CancellationToken cancellationToken = default);
        Task<Article> GetArticleAsync(string slug, string? token, CancellationToken cancellationToken = default);
        Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default);
    }
}