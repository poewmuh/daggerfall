using System;
using System.Collections.Generic;
using Game.Application.Asset;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Game.Application.GUI.OverlaySystem
{
    public class OverlaySystem : IInitializable, IOverlaySystem, IDisposable
    {
        [Inject] private IGUISystem _guiSystem;
        [Inject] private IAssetLoader _assetLoader;
        [Inject] private IObjectResolver _objectResolver;

        private Transform _overlaysParent;
        private readonly Dictionary<string, GameObject> _createdOverlays = new();

        void IInitializable.Initialize()
        {
            _overlaysParent = _guiSystem.Canvas.CreateContainer("OverlaysGUI");
        }

        void IOverlaySystem.CreateOverlay(OverlayType type)
        {
            CreateOverlay(type.ToString());
        }

        private void CreateOverlay(string overlayKey)
        {
            if (_createdOverlays.TryGetValue(overlayKey, out var existing) && existing)
                return;

            var prefab = _assetLoader.LoadImmediate<GameObject>(overlayKey);
            _createdOverlays[overlayKey] = _objectResolver.Instantiate(prefab, _overlaysParent);
        }

        void IDisposable.Dispose()
        {
            foreach (var assetKey in _createdOverlays.Keys)
            {
                _assetLoader.Release(assetKey);
            }

            _createdOverlays.Clear();
        }
    }
}
