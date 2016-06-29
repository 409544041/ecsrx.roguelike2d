﻿using Assets.EcsRx.Framework.Blueprints;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Unity.Components;

namespace Assets.Game.Blueprints
{
    public class EnemyBlueprint : IBlueprint
    {
        public void Apply(IEntity entity)
        {
            var enemyComponent = new EnemyComponent();
            enemyComponent.Health.Value = 3;

            entity.AddComponent(enemyComponent);
            entity.AddComponent<ViewComponent>();
            entity.AddComponent<MovementComponent>();
            entity.AddComponent<RandomlyPlacedComponent>();
        }
    }
}