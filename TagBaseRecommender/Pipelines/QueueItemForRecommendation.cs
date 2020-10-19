﻿using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Web;

namespace TagBaseRecommender.Pipelines
{
    public class QueueItemForRecommendation : HttpRequestProcessor
    {
        public QueueItemForRecommendation()
        {
        }

        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item == null || Context.Item.ID != KnownSettings.SearchTemplate)
            {
                return;
            }

            if (!(Context.PageMode.IsPreview || Context.PageMode.IsNormal))
            {
                return;
            }

            // クッキーからタグ一覧を取得
            var cookie = GetCookie();
            var cookieItems = new Queue<string>(cookie.Value?.Split('|') ?? Array.Empty<string>());
            cookieItems.Enqueue(Context.Item.ID.ToShortID().ToString());

            while (cookieItems.Count > KnownSettings.StoredItemCount)
            {
                cookieItems.Dequeue();
            }

            cookie.Value = string.Join("|", cookieItems);

            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        private HttpCookie GetCookie()
        {
            var cookieName = KnownSettings.Cookie.Name;
            var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (cookie == null)
            {
                // Ensure cookie exists
                cookie = new HttpCookie(cookieName)
                {
                    Expires = DateTime.Now.AddDays(KnownSettings.Cookie.LifeSpan),
                    Domain = KnownSettings.Cookie.Domain,
                    Path = KnownSettings.Cookie.Path,
                    Secure = KnownSettings.Cookie.Secure,
                    HttpOnly = KnownSettings.Cookie.HttpOnly,
                };
            }

            return cookie;
        }
    }
}