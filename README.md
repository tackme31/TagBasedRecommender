# Tag Based Recommender
*Tag Based Recommender* is a simple tag-based recommendation library for Sitecore.

**Warning: This software is in early stage of development.**

## Supports
This software is tested on the following environment.

- Sitecore: XM/XP 9.3
- SearchProvider: Solr

## Installation
Not released yet. Clone this repository and build it locally.

## Usage
1. Add a `Tags` field to a page template.
1. (Optional) Set the template's ID to `TagBasedRecommender.SearchTemplate` settings.
```xml
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <settings>
      <setting name="TagBasedRecommender.SearchTemplate">
        <patch:attribute name="value">{The template's ID here}</patch:attribute>
      </setting>
    </settings>
  </sitecore>
 </configuration>
```
3. Now you can get recommendations with `IRecommendationService.GetRecommendations` method.

```csharp
using TagBasedRecommender.Services;

public class RecommendExample
{
    public RecommendExample(IRecommendationService service)
    {
        var recommendations = service.GetRecommendations(count: 10);
    }
}
```

## Settings
|Name|Type|Description|Default|
|:-|:-|:-|:-|
|`TagBasedRecommender.SearchField`|`string`|An index field name for search by tags.|`_content`|
|`TagBasedRecommender.SearchTemplate`|`ID`|A template ID to use filtering recommendation. |empty (All templates)|
|`TagBasedRecommender.StoredItemCount`|`int`|A length of items stored in the cookie.|`20`|
|`TagBasedRecommender.BoostMultiplicand`|`float`|A value to be added to boosting when a tag is matched.|`1`|
|`TagBasedRecommender.FilterStoredItems`|`bool`|When set true, items stored in the cookie are filtered from recommendations.|`false`|
|`TagBasedRecommender.Cookie.Name`|`string`|A cookie's name (required).|`tagbasedrec_items`|
|`TagBasedRecommender.Cookie.Lifespan`|`int`|A cookie's lifespan to set to `Expire` attribute (in days).|`30`|
|`TagBasedRecommender.Cookie.Domain`|`string`|A cookie's `Domain` attribute.|empty|
|`TagBasedRecommender.Cookie.Path`|`string`|A cookies's `Path` attribute.|`/`|
|`TagBasedRecommender.Cookie.Secure`|`bool`|A cookie's `Secure` Attribute.|`true`|
|`TagBasedRecommender.Cookie.HttpOnly`|`bool`|A cookie's `HttpOnly` attribute.|`true`|


## Customization
### Custom Tags
This library uses item's `Tags` field to get recommendation tags (splited at whitespace). This behaviour can be customized by replacing `DefaultItemTagsResolver`.

```csharp
using TagBasedRecommender.Services;

public class CategoryTagsResolver : IItemTagsResolver
{
    public List<string> GetTags(Item item)
    {
        // Default
        // return item["Tags"].Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).ToList();

        // Use categories's name instead.
        var tags = (MultilistField)item.Fields["Categories"];
        return tags.GetItems().Select(tag => tag["Name"]).ToList();
    }
}
```

And replace the default resolver by applying the following configuration (This is just an example).

```xml
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <services>
      <register serviceType="TagBasedRecommender.Services.IItemTagsResolver, TagBasedRecommender">
        <patch:attribute name="implementationType">Namespace.To.CategoryTagsResolver, AssemblyName</patch:attribute>
      </register>
    </services>
  </sitecore>
</configuration>
```

### Custom Filter
WIP

## Author
- Takumi Yamada (xirtardauq@gmail.com)

## License
*Tag Based Recommender* is licensed under the MIT license. See LICENSE.txt.