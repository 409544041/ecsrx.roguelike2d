﻿using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class StandardInputSystem : IReactToGroupSystem
    {
        private IEventSystem _eventSystem;
        private IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(StandardInputComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
        {
            return _eventSystem.Receive<PlayerTurnEvent>().Select(x => @group);
        }

        public StandardInputSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void Execute(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            if(movementComponent.Movement.Value != Vector2.zero) { return; }
            
            var horizontal = 0;
            var vertical = 0;

            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));
            
            if (horizontal != 0)
            {
                vertical = 0;
            }

            if (horizontal != 0 || vertical != 0)
            {
                var movement = new Vector2(horizontal, vertical);
                movementComponent.Movement.Value = movement;
            }
        }
    }
}