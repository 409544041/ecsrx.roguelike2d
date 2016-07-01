﻿using System;
using EcsRx.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Components
{
    public class MovementComponent : IComponent, IDisposable
    {
        public ReactiveProperty<Vector2> Movement { get; set; }

        public MovementComponent()
        {
            Movement = new Vector2ReactiveProperty();
        }

        public void Dispose()
        {
            Movement.Dispose();
        }
    }
}