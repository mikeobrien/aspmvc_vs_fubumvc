﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace Core.Infrastructure.Fubu
{
    public class RegexUrlPolicy : IUrlPolicy
    {
        public enum Segment { Namespace, Class, Method }

        private class SegmentPattern
        {
            public Segment Type { get; set; }
            public Regex Regex { get; set; }
        }

        private class HttpConstraintPattern : SegmentPattern
        {
            public string Method { get; set; }
        }

        private static readonly Regex MatchAll = new Regex(".*");

        private readonly Func<ActionCall, bool> _matchFilter;
        private readonly List<SegmentPattern> _segmentPatterns = new List<SegmentPattern>();
        private readonly List<HttpConstraintPattern> _httpConstraintPatterns = new List<HttpConstraintPattern>();

        public RegexUrlPolicy(Func<ActionCall, bool> matchFilter)
        {
            _matchFilter = matchFilter;
        }

        public static RegexUrlPolicy Create()
        {
            return new RegexUrlPolicy(x => true);
        }

        public static RegexUrlPolicy Create(Func<ActionCall, bool> matchFilter)
        {
            return new RegexUrlPolicy(matchFilter);
        }

        public bool Matches(ActionCall call, IConfigurationObserver log)
        {
            return _matchFilter(call);
        }

        public RegexUrlPolicy IgnoreNamespace()
        {
            _segmentPatterns.Add(new SegmentPattern { Regex = MatchAll, Type = Segment.Namespace });
            return this;
        }

        public RegexUrlPolicy IgnoreNamespaces(params string[] patterns)
        {
            return IgnoreSegment(Segment.Namespace, patterns);
        }

        public RegexUrlPolicy IgnoreNamespace<T>(params string[] patterns)
        {
            return IgnoreNamespaces(typeof(T).Namespace).IgnoreNamespaces(patterns);
        }

        public RegexUrlPolicy IgnoreAssemblyNamespace(Type type, params string[] patterns)
        {
            return IgnoreNamespaces(Assembly.GetAssembly(type).GetName().Name).
                   IgnoreNamespaces(patterns);
        }

        public RegexUrlPolicy IgnoreAssemblyNamespace<T>(params string[] patterns)
        {
            return IgnoreNamespaces(Assembly.GetAssembly(typeof(T)).GetName().Name).
                   IgnoreNamespaces(patterns);
        }

        public RegexUrlPolicy IgnoreThisAssemblyNamespace(params string[] patterns)
        {
            return IgnoreNamespaces(Assembly.GetCallingAssembly().GetName().Name).
                   IgnoreNamespaces(patterns);
        }

        public RegexUrlPolicy IgnoreClassName()
        {
            _segmentPatterns.Add(new SegmentPattern { Regex = MatchAll, Type = Segment.Class });
            return this;
        }

        public RegexUrlPolicy IgnoreClassNames(params string[] patterns)
        {
            return IgnoreSegment(Segment.Class, patterns);
        }

        public RegexUrlPolicy IgnoreMethodName()
        {
            _segmentPatterns.Add(new SegmentPattern { Regex = MatchAll, Type = Segment.Method });
            return this;
        }

        public RegexUrlPolicy IgnoreMethodNames(params string[] patterns)
        {
            return IgnoreSegment(Segment.Method, patterns);
        }

        public RegexUrlPolicy IgnoreSegment(Segment segment, params string[] patterns)
        {
            if (patterns.Any()) 
                _segmentPatterns.AddRange(
                    patterns.Select(x => new SegmentPattern
                    {
                        Regex = new Regex(x),
                        Type = segment
                    }));
            return this;
        }

        public RegexUrlPolicy ConstrainNamespaceToHttpGet(params string[] patterns)
        { return ConstrainToHttpGet(Segment.Namespace, patterns); }

        public RegexUrlPolicy ConstrainNamespaceToHttpPost(params string[] patterns)
        { return ConstrainToHttpPost(Segment.Namespace, patterns); }

        public RegexUrlPolicy ConstrainNamespaceToHttpPut(params string[] patterns)
        { return ConstrainToHttpPut(Segment.Namespace, patterns); }

        public RegexUrlPolicy ConstrainNamespaceToHttpDelete(params string[] patterns)
        { return ConstrainToHttpDelete(Segment.Namespace, patterns); }

        public RegexUrlPolicy ConstrainClassToHttpGet(params string[] patterns)
        { return ConstrainToHttpGet(Segment.Class, patterns); }

        public RegexUrlPolicy ConstrainClassToHttpPost(params string[] patterns)
        { return ConstrainToHttpPost(Segment.Class, patterns); }

        public RegexUrlPolicy ConstrainClassToHttpPut(params string[] patterns)
        { return ConstrainToHttpPut(Segment.Class, patterns); }

        public RegexUrlPolicy ConstrainClassToHttpDelete(params string[] patterns)
        { return ConstrainToHttpDelete(Segment.Class, patterns); }

        public RegexUrlPolicy ConstrainClassToHttpGetStartingWith(params string[] patterns)
        { return ConstrainToHttpGet(Segment.Class, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainClassToHttpPostStartingWith(params string[] patterns)
        { return ConstrainToHttpPost(Segment.Class, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainClassToHttpPutStartingWith(params string[] patterns)
        { return ConstrainToHttpPut(Segment.Class, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainClassToHttpDeleteStartingWith(params string[] patterns)
        { return ConstrainToHttpDelete(Segment.Class, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainMethodToHttpGet(params string[] patterns)
        { return ConstrainToHttpGet(Segment.Method, patterns); }

        public RegexUrlPolicy ConstrainMethodToHttpPost(params string[] patterns)
        { return ConstrainToHttpPost(Segment.Method, patterns); }

        public RegexUrlPolicy ConstrainMethodToHttpPut(params string[] patterns)
        { return ConstrainToHttpPut(Segment.Method, patterns); }

        public RegexUrlPolicy ConstrainMethodToHttpDelete(params string[] patterns)
        { return ConstrainToHttpDelete(Segment.Method, patterns); }

        public RegexUrlPolicy ConstrainMethodToHttpGetStartingWith(params string[] patterns)
        { return ConstrainToHttpGet(Segment.Class, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainMethodToHttpPostStartingWith(params string[] patterns)
        { return ConstrainToHttpPost(Segment.Method, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainMethodToHttpPutStartingWith(params string[] patterns)
        { return ConstrainToHttpPut(Segment.Method, patterns.Select(RegexStartingWith).ToArray()); }

        public RegexUrlPolicy ConstrainMethodToHttpDeleteStartingWith(params string[] patterns)
        { return ConstrainToHttpDelete(Segment.Method, patterns.Select(RegexStartingWith).ToArray()); }

        public IRouteDefinition Build(ActionCall call)
        {
            var route = call.ToRouteDefinition();
            AppendNamespace(route, call, _segmentPatterns.Where(x => x.Type == Segment.Namespace).Select(x => x.Regex));
            AppendClass(route, call, _segmentPatterns.Where(x => x.Type == Segment.Class).Select(x => x.Regex));
            AppendMethod(route, call, _segmentPatterns.Where(x => x.Type == Segment.Method).Select(x => x.Regex));
            ConstrainToHttpMethod(route, call, _httpConstraintPatterns);
            return route;
        }

        private RegexUrlPolicy ConstrainToHttpGet(Segment segment, params string[] patterns)
        { return ConstrainSegmentToHttpMethod(segment, "GET", patterns); }

        private RegexUrlPolicy ConstrainToHttpPost(Segment segment, params string[] patterns)
        { return ConstrainSegmentToHttpMethod(segment, "POST", patterns); }

        private RegexUrlPolicy ConstrainToHttpPut(Segment segment, params string[] patterns)
        { return ConstrainSegmentToHttpMethod(segment, "PUT", patterns); }

        private RegexUrlPolicy ConstrainToHttpDelete(Segment segment, params string[] patterns)
        { return ConstrainSegmentToHttpMethod(segment, "DELETE", patterns); }

        private RegexUrlPolicy ConstrainSegmentToHttpMethod(Segment segment, string method, params string[] patterns)
        {
            _httpConstraintPatterns.AddRange(patterns.Select(x =>
                new HttpConstraintPattern { Type = segment, Method = method, Regex = new Regex(x) }));
            return this;
        }

        private static void ConstrainToHttpMethod(IRouteDefinition route, ActionCall call, IEnumerable<HttpConstraintPattern> patterns)
        {
            Func<Segment, string> getName = s => {
                switch (s) {
                    case Segment.Namespace: return call.HandlerType.Namespace;
                    case Segment.Class: return call.HandlerType.Name;
                    case Segment.Method: return call.Method.Name;
                } return null; };
            patterns.Where(x => x.Regex.IsMatch(getName(x.Type))).ToList().
                     ForEach(x => route.AddHttpMethodConstraint(x.Method));
        }

        private static void AppendNamespace(IRouteDefinition route, ActionCall call, IEnumerable<Regex> ignore)
        {
            var part = RemovePattern(call.HandlerType.Namespace, ignore).Replace('.', '/').ToLower();
            if (part.IsNotEmpty()) route.Append(part);
        }

        private static void AppendClass(IRouteDefinition route, ActionCall call, IEnumerable<Regex> ignore)
        {
            var part = RemovePattern(call.HandlerType.Name, ignore).ToLower();
            if (part.IsNotEmpty()) route.Append(part);
        }

        private static void AppendMethod(IRouteDefinition route, ActionCall call, IEnumerable<Regex> ignore)
        {
            var part = RemovePattern(call.Method.Name, ignore);
            if (MethodToUrlBuilder.Matches(call.Method.Name) && call.HasInput)
            {
                MethodToUrlBuilder.Alter(route, part, new TypeDescriptorCache().GetPropertiesFor(call.InputType()).Keys, x => { });
                route.ApplyInputType(call.InputType());
            }
            else if (part.IsNotEmpty()) route.Append(part.ToLower());
        }

        private static string RemovePattern(string source, IEnumerable<Regex> pattern)
        {
            return pattern.Aggregate(source, (a, i) => i.Replace(a, string.Empty));
        }

        private static string RegexStartingWith(string value)
        {
            return string.Format("^{0}.*", value);
        }
    }
}