using Sitecore.Data.Items;
using System.Collections.Generic;

namespace TagBasedRecommender.Services
{
    public interface IItemTagsResolver
    {
        List<string> GetItemTags(Item item);
    }
}
