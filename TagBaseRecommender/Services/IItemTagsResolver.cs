using Sitecore.Data.Items;
using System.Collections.Generic;

namespace TagBaseRecommender.Services
{
    public interface IItemTagsResolver
    {
        List<string> GetItemTags(Item item);
    }
}
