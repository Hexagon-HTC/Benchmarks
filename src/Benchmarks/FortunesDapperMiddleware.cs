// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Benchmarks.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Benchmarks
{
    public class FortunesDapperMiddleware
    {
        private static readonly PathString _path = new PathString(Scenarios.GetPaths(s => s.DbFortunesDapper)[0]);

        private readonly RequestDelegate _next;
        private readonly string _connectionString;
        private readonly DapperDb _db;
        private readonly HtmlEncoder _htmlEncoder;

        public FortunesDapperMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, DapperDb db, HtmlEncoder htmlEncoder)
        {
            _next = next;
            _connectionString = appSettings.Value.ConnectionString;
            _db = db;
            _htmlEncoder = htmlEncoder;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments(_path, StringComparison.Ordinal))
            {
                var rows = await _db.LoadFortunesRows(_connectionString);

                await MiddlewareHelpers.RenderFortunesHtml(rows, httpContext, _htmlEncoder);

                return;
            }

            await _next(httpContext);
        }
    }
    
    public static class FortunesDapperMiddlewareExtensions
    {
        public static IApplicationBuilder UseFortunesDapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FortunesDapperMiddleware>();
        }
    }
}
