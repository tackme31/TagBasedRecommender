using Sitecore;
using Sitecore.Data;

namespace TagBasedRecommender
{
    public static class KnownSettings
    {
        private static string GetSetting(string name, string @default = null) => Sitecore.Configuration.Settings.GetSetting($"TagBasedRecommender.{name}", @default);

        public static string SearchField = GetSetting("SearchField");
        public static ID SearchTemplate = MainUtil.GetID(GetSetting("SearchTemplate"), ID.Null);
        public static int StoredItemCount = MainUtil.GetInt(GetSetting("SearchField"), 20);
        public static float BoostMultiplicand = MainUtil.GetFloat(GetSetting("BoostMultiplicand"), 1.0f);
        public static class Cookie
        {
            public static string Name = GetSetting("Cookie.Name");
            public static int LifeSpan = MainUtil.GetInt(GetSetting("Cookie.Lifespan"), 30);
            public static string Domain = GetSetting("Cookie.Domain", string.Empty);
            public static string Path = GetSetting("Cookie.Path", "/");
            public static bool Secure = MainUtil.GetBool(GetSetting("Cookie.Secure"), false);
            public static bool HttpOnly = MainUtil.GetBool(GetSetting("Cookie.HttpOnly"), false);
        }
    }
}
