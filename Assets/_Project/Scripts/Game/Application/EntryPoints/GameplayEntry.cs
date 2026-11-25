using Cysharp.Threading.Tasks;
using Game.Application.Asset;
using Game.Application.InputSystem;
using Game.Application.Scene;
using Game.Gameplay;
using Game.Gameplay.AI;
using Game.Gameplay.Agent;
using Game.Gameplay.Combat;
using Game.Gameplay.Damage;
using Game.Gameplay.Dash;
using Game.Gameplay.Destroy;
using Game.Gameplay.Input;
using Game.Gameplay.Movement;
using Game.Gameplay.Navigation;
using Game.Gameplay.Perception;
using Game.Gameplay.Spawn;
using Game.Gameplay.View;
using MessagePipe;
using R3;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Game.Application.EntryPoints
{
    public class GameplayEntry : SceneEntryPoint
    {
	    [Inject] private IInputSystem _inputSystem;
	    [Inject] private IAssetLoader _assetLoader;
	    [Inject] private ISceneSystem _sceneSystem;
	    [Inject] private Camera _camera;
	    [Inject] private IPublisher<WeaponPresentationMessage> _weaponPresentationPublisher;
	    [Inject] private IPublisher<PlayerDiedMessage> _playerDiedPublisher;
	    [Inject] private ISubscriber<PlayerDiedMessage> _playerDiedSubscriber;
	    
        [SerializeField] private AgentConfig _playerConfig;

        private World _world;

        protected override void Initialize()
        {
	        base.Initialize();

	        _world = World.Create();
	        _world.UpdateByUnity = true;

	        var sceneAgents = FindObjectsByType<AgentAuthoring>(FindObjectsSortMode.None);
	        
	        _world.AddSystemsGroup(GameplayOrder.Spawn, SpawnFeature.Create(_world, _playerConfig, Vector3.zero, sceneAgents));
	        _world.AddSystemsGroup(GameplayOrder.View, ViewFeature.Create(_world, _assetLoader, _camera));
	        _world.AddSystemsGroup(GameplayOrder.Input, InputFeature.Create(_world, _inputSystem));
	        _world.AddSystemsGroup(GameplayOrder.Perception, PerceptionFeature.Create(_world));
	        _world.AddSystemsGroup(GameplayOrder.Ai, AiFeature.Create(_world));
	        _world.AddSystemsGroup(GameplayOrder.Navigation, NavigationFeature.Create(_world));
	        
	        _world.AddSystemsGroup(GameplayOrder.VelocityReset, VelocityFeature.CreateReset(_world));
	        _world.AddSystemsGroup(GameplayOrder.Dash, DashFeature.Create(_world));
	        _world.AddSystemsGroup(GameplayOrder.Movement, MovementFeature.Create(_world, _camera));
	        _world.AddSystemsGroup(GameplayOrder.VelocityApply, VelocityFeature.CreateApply(_world));
	        
	        _world.AddSystemsGroup(GameplayOrder.Combat, CombatFeature.Create(_world, _camera, _weaponPresentationPublisher));
	        _world.AddSystemsGroup(GameplayOrder.Damage, DamageFeature.Create(_world, _playerDiedPublisher));
	        
	        _world.AddSystemsGroup(GameplayOrder.Destroy, DestroyFeature.Create(_world));

	        _playerDiedSubscriber.Subscribe(OnPlayerDied).AddTo(this);
        }

        private void OnPlayerDied(PlayerDiedMessage message)
        {
	        _sceneSystem.ReloadGameplay().Forget();
        }

        private void OnDestroy()
        {
	        _world?.Dispose();
	        _world = null;
        }
    }
}
