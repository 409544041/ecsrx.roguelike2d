﻿using Assets.EcsRx.Framework.Attributes;
using Assets.Game.Components;
using Assets.Game.Extensions;
using Assets.Game.Groups;
using Assets.Game.SceneCollections;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace Assets.Game.ViewResolvers
{
    [Priority(2)]
    public class WallViewResolver : ViewResolverSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(WallComponent), typeof(ViewComponent));
        private readonly WallTiles _wallTiles;

        public override IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public WallViewResolver(IPoolManager poolManager, WallTiles wallTiles) : base(poolManager)
        {
            _wallTiles = wallTiles;
        }

        public override GameObject ResolveView(IEntity entity)
        {
            var tileChoice = _wallTiles.AvailableTiles.TakeRandom();
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity) as GameObject;
            gameObject.name = string.Format("wall-{0}", entity.Id);
            return gameObject;
        }
    }
}