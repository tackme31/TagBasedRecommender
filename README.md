# Tag Base Recommender
*Tag Base Recommendder* is a simple tag-based recommendation library for Sitecore

**Warning: This software is in early stage of development.**

## Installation
No releases yet. Clone this repository and build it locally.

## Usage
1. Add `Tags` field to a page template.
1. (Optional) Set the template's ID to `TagBaseRecommender.SearchTemplate` settings.
```xml
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <settings>
      <setting name="TagBaseRecommender.SearchTemplate"></setting>
        <patch:attribute name="value">{The template's ID here}</patch:attribute>
      </setting>
    </settings>
  </sitecore>
 </configuration>
```
3. Now you can get recommendation to use `IRecommendationService.GetRecommendations` method.

```csharp
using TagBaseRecommender.Services;

public class RecommendExample
{
    public RecommendExample(IRecommendationService service)
    {
        var recommendations = service.GetRecommendations(recommendCount: 10);
    }
}
```

## Settings
|Name|Type|Description|Default|
|:-|:-|:-|:-|
|`TagBaseRecommender.SearchField`|`string`|An index field name for search by tags.|`_content`|
|`TagBaseRecommender.SearchTemplate`|`ID`|A template ID to use filtering recommendation. |empty (All templates)|
|`TagBaseRecommender.StoredItemCount`|`int`|A length of items stored in a cookie value.|`20`|
|`TagBaseRecommender.BoostMultiplicand`|`float`|A value to be added to the boost due to a tag match.|`1`|
|`TagBaseRecommender.Cookie.Name`|`string`|A cookie's name (required).|`tagbaserec_items`|
|`TagBaseRecommender.Cookie.Lifespan`|`int`|A cookie's lifespan to set to `Expire` attribute (in days).|`30`|
|`TagBaseRecommender.Cookie.Domain`|`string`|A cookie's `Domain` attribute.|empty|
|`TagBaseRecommender.Cookie.Path`|`string`|A cookies's `Path` attribute.|`/`|
|`TagBaseRecommender.Cookie.Secure`|`bool`|A cookie's `Secure` Attribute.|`true`|
|`TagBaseRecommender.Cookie.HttpOnly`|`bool`|A cookie's `HttpOnly` attribute.|`true`|


## Customization
### Custom Tags
This library uses item's `Tags` field to get recommendation tags (splited at whitespace). This behaviour can be customized by replacing `DefaultItemTagsResolver`.

```csharp
using TagBaseRecommender.Services;

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

And replace the default class by apply the following configuration (This is just an example).

```xml
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <services>
      <register serviceType="TagBaseRecommender.Services.IItemTagsResolver, TagBaseRecommender">
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
*Tag Base Recommender* is licensed unther the MIT license. See LICENSE.txt.