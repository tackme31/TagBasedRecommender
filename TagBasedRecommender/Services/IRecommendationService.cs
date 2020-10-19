using Sitecore.Data.Items;
using System.Collections.Generic;

namespace TagBasedRecommender.Services
{
    public interface IRecommendationService
    {
        List<Item> GetRecommedations(int recommendCount);
    }
}
