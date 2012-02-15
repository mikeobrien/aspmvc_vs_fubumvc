using System;
using System.Reflection;
using Core.Infrastructure.Reflection;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Behaviors;

namespace FubuMvc.Behaviors
{
    public class AssetModeBehavior : IActionBehavior
    {
        private static readonly Lazy<bool> Debug = new Lazy<bool>(() => Assembly.GetExecutingAssembly().IsInDebugMode());
        private readonly IAssetRequirements _assetRequirements;
        private readonly IActionBehavior _innerBehavior;

        public AssetModeBehavior(IAssetRequirements assetRequirements, IActionBehavior innerBehavior)
        {
            _assetRequirements = assetRequirements;
            _innerBehavior = innerBehavior;
        }

        public void Invoke()
        {
            if (Debug.Value) _assetRequirements.Require("scripts.debug", "styles.debug");
            else _assetRequirements.Require("scripts", "styles");
            _innerBehavior.Invoke();
        }

        public void InvokePartial() { }
    }
}