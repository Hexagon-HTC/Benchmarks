// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace Benchmarks
{
    public class Scenarios
    {
        [ScenarioPath("/plaintext")]
        public bool Plaintext { get; set; }

        [ScenarioPath("/json")]
        public bool Json { get; set; }

        [ScenarioPath("/128B.txt", "/512B.txt", "/1KB.txt", "/4KB.txt", "/16KB.txt", "/512KB.txt", "/1MB.txt", "/5MB.txt")]
        public bool StaticFiles { get; set; }

        [ScenarioPath("/db/raw")]
        public bool DbSingleQueryRaw { get; set; }

        [ScenarioPath("/db/ef")]
        public bool DbSingleQueryEf { get; set; }

        [ScenarioPath("/db/dapper")]
        public bool DbSingleQueryDapper { get; set; }

        [ScenarioPath("/queries/raw")]
        public bool DbMultiQueryRaw { get; set; }

        [ScenarioPath("/queries/ef")]
        public bool DbMultiQueryEf { get; set; }

        [ScenarioPath("/queries/dapper")]
        public bool DbMultiQueryDapper { get; set; }

        [ScenarioPath("/fortunes/raw")]
        public bool DbFortunesRaw { get; set; }

        [ScenarioPath("/fortunes/ef")]
        public bool DbFortunesEf { get; set; }

        [ScenarioPath("/fortunes/dapper")]
        public bool DbFortunesDapper { get; set; }

        [ScenarioPath("/mvc/plaintext", "/mvc/json")]
        public bool MvcApis { get; set; }

        [ScenarioPath("/mvc/view")]
        public bool MvcViews { get; set; }

        //[ScenarioPath("/mvc/db/raw")]
        //public bool MvcDbSingleQueryRaw { get; set; }

        //[ScenarioPath("/mvc/db/dapper")]
        //public bool MvcDbSingleQueryDapper { get; set; }

        //[ScenarioPath("/mvc/db/ef")]
        //public bool MvcDbSingleQueryEf { get; set; }

        //[ScenarioPath("/mvc/queries/raw")]
        //public bool MvcDbMultiQueryRaw { get; set; }

        //[ScenarioPath("/mvc/queries/dapper")]
        //public bool MvcDbMultiQueryDapper { get; set; }

        //[ScenarioPath("/mvc/queries/ef")]
        //public bool MvcDbMultiQueryEf { get; set; }

        [ScenarioPath("/mvc/fortunes/raw")]
        public bool MvcDbFortunesRaw { get; set; }

        [ScenarioPath("/mvc/fortunes/ef")]
        public bool MvcDbFortunesEf { get; set; }

        [ScenarioPath("/mvc/fortunes/dapper")]
        public bool MvcDbFortunesDapper { get; set; }

        public bool Any(string partialName) =>
            typeof(Scenarios).GetTypeInfo().DeclaredProperties
                .Where(p => p.Name.IndexOf(partialName, StringComparison.Ordinal) >= 0 && (bool)p.GetValue(this))
                .Any();

        public IEnumerable<string> GetNames() =>
            typeof(Scenarios).GetTypeInfo().DeclaredProperties
                .Select(p => p.Name);

        public IEnumerable<EnabledScenario> GetEnabled() =>
            typeof(Scenarios).GetTypeInfo().DeclaredProperties
                .Where(p => p.GetValue(this) is bool && (bool)p.GetValue(this))
                .Select(p => new EnabledScenario(p.Name, p.GetCustomAttribute<ScenarioPathAttribute>()?.Paths));

        public static string[] GetPaths(Expression<Func<Scenarios, bool>> scenarioExpression) =>
            scenarioExpression.GetPropertyAccess().GetCustomAttribute<ScenarioPathAttribute>().Paths;

        public int Enable(string partialName)
        {
            var props = typeof(Scenarios).GetTypeInfo().DeclaredProperties
                .Where(p => string.Equals(partialName, "[all]", StringComparison.OrdinalIgnoreCase) || p.Name.IndexOf(partialName, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            foreach (var p in props)
            {
                p.SetValue(this, true);
            }
            
            return props.Count;
        }
        
        public void EnableDefault()
        {
            Plaintext = true;
            Json = true;
        }

        public class EnabledScenario
        {
            public EnabledScenario(string name, IEnumerable<string> paths)
            {
                Name = name;
                Paths = paths;
            }

            public string Name { get; }

            public IEnumerable<string> Paths { get; }
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ScenarioPathAttribute : Attribute
    {
        public ScenarioPathAttribute(params string[] paths)
        {
            Paths = paths;
        }

        public string[] Paths { get; }
    }
}