using Game.Application.Asset;
using Game.Application.InputSystem;
using Game.Gameplay;
using Game.Gameplay.Agent;
using Game.Gameplay.Combat;
using Game.Gameplay.Damage;
using Game.Gameplay.Dash;
using Game.Gameplay.Destroy;
using Game.Gameplay.Input;
using Game.Gameplay.Movement;
using Game.Gameplay.Spawn;
using Game.Gameplay.View;
using MessagePipe;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace Game.Application.EntryPoints
{
    public class GameplayEntry : SceneEntryPoint
    {
	    [Inject] private IInputSystem _inputSystem;
	    [Inject] private IAssetLoader _assetLoader;
	    [Inject] private Camera _camera;
	    [Inject] private IPublisher<WeaponPresentationMessage> _weaponPresentationPublisher;
	    
        [SerializeField] private AgentConfig _playerConfig;

        protected override void Initialize()
        {
	        base.Initialize();

	        var world = World.Default;
	        
	        world.AddSystemsGroup(GameplayOrder.Spawn, SpawnFeature.Create(world, _playerConfig, Vector3.zero));
	        world.AddSystemsGroup(GameplayOrder.View, ViewFeature.Create(world, _assetLoader));
	        world.AddSystemsGroup(GameplayOrder.Input, InputFeature.Create(world, _inputSystem));
	        world.AddSystemsGroup(GameplayOrder.VelocityReset, VelocityFeature.CreateReset(world));
	        world.AddSystemsGroup(GameplayOrder.Dash, DashFeature.Create(world));
	        world.AddSystemsGroup(GameplayOrder.Movement, MovementFeature.Create(world, _camera));
	        world.AddSystemsGroup(GameplayOrder.VelocityApply, VelocityFeature.CreateApply(world));
	        world.AddSystemsGroup(GameplayOrder.Combat, CombatFeature.Create(world, _camera, _weaponPresentationPublisher));
	        world.AddSystemsGroup(GameplayOrder.Damage, DamageFeature.Create(world));
	        world.AddSystemsGroup(GameplayOrder.Destroy, DestroyFeature.Create(world));
        }
    }
}
