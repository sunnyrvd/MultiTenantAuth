﻿using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace MultiTenant.Api.Extension
{
    public class JweBearerOptionsProvider : IOptionsMonitor<JwtBearerOptions>
    {
        private readonly ConcurrentDictionary<(string name, string tenant), Lazy<JwtBearerOptions>> _cache;
        private readonly IOptionsFactory<JwtBearerOptions> _optionsFactory;
        private readonly TenantProvider _tenantProvider;
        public JweBearerOptionsProvider(
            IOptionsFactory<JwtBearerOptions> optionsFactory,
            TenantProvider tenantProvider)
        {
            _cache = new ConcurrentDictionary<(string, string), Lazy<JwtBearerOptions>>();
            _optionsFactory = optionsFactory;
            _tenantProvider = tenantProvider;

        }
        public JwtBearerOptions CurrentValue => Get(Options.DefaultName);
        public JwtBearerOptions Get(string name)
        {
            var tenant = _tenantProvider.GetCurrentTenant();
            Lazy<JwtBearerOptions> Create() => new Lazy<JwtBearerOptions>(() => _optionsFactory.Create(name));
            return _cache.GetOrAdd((name, tenant), _ => Create()).Value;
        }
        public IDisposable OnChange(Action<JwtBearerOptions, string> listener) => null;
    }
}
