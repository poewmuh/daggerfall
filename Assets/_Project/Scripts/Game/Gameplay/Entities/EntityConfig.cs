using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Entities
{
	public abstract class EntityConfig<TTrait> : ScriptableObject where TTrait : class, IEntityTrait
	{
		[ValidateInput(nameof(ValidateTraits))]
		[SerializeReference] private TTrait[] _traits = Array.Empty<TTrait>();

		public IReadOnlyList<TTrait> Traits => _traits;

		public void Install(Entity entity, World world)
		{
			foreach (var trait in _traits)
			{
				trait.Install(entity, world);
			}
		}

		private TriValidationResult ValidateTraits()
		{
			var types = new HashSet<Type>();

			foreach (var trait in _traits)
			{
				if (trait == null)
					return TriValidationResult.Error("Trait slot is empty.");

				if (!types.Add(trait.GetType()))
					return TriValidationResult.Error($"Duplicate trait: {trait.GetType().Name}.");
			}

			return TriValidationResult.Valid;
		}
	}
}
