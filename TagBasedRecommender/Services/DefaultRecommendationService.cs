using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagBasedRecommender.Services
{
    public class DefaultRecommendationService<T> : IRecommendationService where T : SearchResultItem
    {
        protected IItemTagsResolver TagsResolver { get; }

        public DefaultRecommendationService(IItemTagsResolver tagsResolver)
        {
            TagsResolver = tagsResolver;
        }

        public List<Item> GetRecommedations(int count = 10)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException($"'{nameof(count)}' should be a positive integer.");
            }

            var index = GetSearchIndex();
            using (var context = index.CreateSearchContext())
            {
                var tagsWeight = GetTagsWeight();
                var boosting = tagsWeight.Keys.Aggregate(
                    PredicateBuilder.Create<T>(item => item.Name.MatchWildcard("*").Boost(0.0f)),
                    (acc, tag) => acc.Or(item => item[KnownSettings.SearchField].Equals(tag).Boost(tagsWeight[tag])));

                var query = ApplyFilterQuery(context.GetQueryable<T>())
                    .Where(boosting)
                    .Take(count)
                    .OrderByDescending(item => item["score"]);

                return query.GetResults().Hits.Select(hit => hit.Document.GetItem()).ToList();
            }
        }

        protected virtual ISearchIndex GetSearchIndex() => ContentSearchManager.GetIndex((SitecoreIndexableItem)Context.Item);

        protected virtual IQueryable<T> ApplyFilterQuery(IQueryable<T> query)
        {
            if (!KnownSettings.SearchTemplate.IsNull)
            {
                query = query.Filter(item => item.TemplateId == KnownSettings.SearchTemplate);
            }

            return query
                .Filter(item => item.Paths.Contains(ItemIDs.ContentRoot))
                .Filter(item => item.Language == Context.Language.Name);
        }


        private IDictionary<string, float> GetTagsWeight()
        {
            var cookieValue = HttpContext.Current.Request.Cookies[KnownSettings.Cookie.Name]?.Value ?? string.Empty;
            var itemIds = cookieValue
                .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(idStr => ID.Parse(idStr, ID.Null))
                .Where(id => !id.IsNull);
            var tags = itemIds
                .Select(id => Context.Database.GetItem(id))
                .Where(item => item != null)
                .SelectMany(TagsResolver.GetItemTags)
                .Where(tag => !tag.IsNullOrEmpty());

            var tagsWeight = new Dictionary<string, float>();
            foreach (var tag in tags)
            {
                if (string.IsNullOrWhiteSpace(tag))
                {
                    continue;
                }

                if (tagsWeight.ContainsKey(tag))
                {
                    tagsWeight[tag] += 1.0f * KnownSettings.BoostMultiplicand;
                }
                else
                {
                    tagsWeight[tag] = 1.0f * KnownSettings.BoostMultiplicand;
                }
            }

            return tagsWeight;
        }
    }
}
