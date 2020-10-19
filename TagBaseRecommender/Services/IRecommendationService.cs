using Sitecore.Data.Items;
using System.Collections.Generic;

namespace TagBaseRecommender.Services
{
    public interface IRecommendationService
    {
        List<Item> GetRecommedations(int recommendCount);
    }
}
