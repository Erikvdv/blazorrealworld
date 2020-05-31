using System.Threading;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Interactors
{
    public interface IArticlesInteractor
    {
        Task<ArticleList> GetArticleListAsync(ArticleListFilter articleListFilter, CancellationToken cancellationToken = default);
        Task<Article> GetArticleAsync(string slug, CancellationToken cancellationToken = default);
        Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default);
    }
}