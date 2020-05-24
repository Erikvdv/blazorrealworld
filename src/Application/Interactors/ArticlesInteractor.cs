using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Clients;
using Application.Models;

namespace Application.Interactors
{
    public class ArticlesInteractor : IArticlesInteractor
    {
        private readonly IConduitClient _conduitClient;

        public ArticlesInteractor(IConduitClient conduitClient)
        {
            _conduitClient = conduitClient;
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
