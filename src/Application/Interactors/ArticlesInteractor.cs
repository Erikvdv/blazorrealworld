using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Models;
using Application.Services;

namespace Application.Interactors
{
    public class ArticlesInteractor : IArticlesInteractor
    {
        private readonly IConduitApiService _conduitClient;

        public ArticlesInteractor(IConduitApiService conduitClient)
        {
            _conduitClient = conduitClient;
        }

        public Task<Article> GetArticleAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _conduitClient.GetArticleAsync(slug, cancellationToken);
        }

        public async Task<ArticleList> GetArticleListAsync(ArticleListFilter articleListFilter, CancellationToken cancellationToken = default)
        {
            return await _conduitClient.GetArticleListAsync(articleListFilter, cancellationToken);
        }


        public async Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default)
        {
            return await _conduitClient.GetTagListAsync(cancellationToken);
        }
    }
}
