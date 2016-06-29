﻿using System;
using System.Collections.Generic;
using Assets.Game.Events;
using Assets.Game.Extensions;
using Assets.Game.SceneCollections;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class SoundEffectSystem : IManualSystem
    {
        public IGroup TargetGroup { get { return new EmptyGroup();} }

        private AudioSource _soundEffectSource;
        private EnemyAttackSounds _enemyAttackSounds;
        private PlayerAttackSounds _playerAttackSounds;
        private WalkingSounds _walkingSounds;
        private DeathSounds _deathSounds;
        private DrinkSounds _drinkSounds;
        private FoodSounds _foodSounds;

        private readonly IEventSystem _eventSystem;
        private List<IDisposable> _subscriptions = new List<IDisposable>();

        public SoundEffectSystem(EnemyAttackSounds enemyAttackSounds, PlayerAttackSounds playerAttackSounds, 
            WalkingSounds walkingSounds, DeathSounds deathSounds, DrinkSounds drinkSounds, 
            FoodSounds foodSounds, IEventSystem eventSystem)
        {
            _enemyAttackSounds = enemyAttackSounds;
            _playerAttackSounds = playerAttackSounds;
            _walkingSounds = walkingSounds;
            _deathSounds = deathSounds;
            _drinkSounds = drinkSounds;
            _foodSounds = foodSounds;

            _eventSystem = eventSystem;

            var soundEffectObject = GameObject.Find("SoundEffectSource");
            _soundEffectSource = soundEffectObject.GetComponent<AudioSource>();
        }

        public void StartSystem(GroupAccessor @group)
        {
            _eventSystem.Receive<FoodPickupEvent>().Subscribe(x => {
                var clips = x.IsSoda ? _drinkSounds.AvailableClips : _foodSounds.AvailableClips;
                PlayOneOf(clips);
            }).AddTo(_subscriptions);

            _eventSystem.Receive<EntityMovedEvent>()
                .Subscribe(x => PlayOneOf(_walkingSounds.AvailableClips))
                .AddTo(_subscriptions);

            _eventSystem.Receive<EnemyHitEvent>()
                .Subscribe(x => PlayOneOf(_playerAttackSounds.AvailableClips))
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerHitEvent>()
                .Subscribe(x => PlayOneOf(_enemyAttackSounds.AvailableClips))
                .AddTo(_subscriptions);
        }

        private void PlayOneOf(IEnumerable<AudioClip> clips)
        {
            var audioSource = clips.TakeRandom();
            _soundEffectSource.PlayOneShot(audioSource);
        }

        public void StopSystem(GroupAccessor @group)
        {
            _subscriptions.DisposeAll();
        }
    }
}