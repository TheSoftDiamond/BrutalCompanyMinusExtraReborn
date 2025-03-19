using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours
{
    public class AntiJesterAI : EnemyAI
    {
        // Essentially, the AntiJester is a modified version of the Jester that works like a Dog does in Lethal Company.

        // Enemy AI Phases:

        // PHASE 1) Passive Concealment: The AntiJester will find a random spot to hide in the facility. It will not move from this spot unless a few criteria are met.
        // At this point, the AntiJester will have a radius that it listens to noises, whether it be player, enemy or item noises.
        // It will have a set value called "Alertness score" that will increase when it hears noises.
        // NV = NoiseBase/d^2. Every sound generated generates a noise value (NV), where Noise Base is the sound level assigned to each sound type, and d is the distance from the AntiJester to the sound source.
        // NV gets added to the Alertness score. If the Alertness score reaches a certain threshold, the AntiJester will enter the next phase.
        // Alertness score can decay over time where: AS(t) = AS initial - (decayRate * t), where AS(t) is the Alertness score at time t, AS initial is the initial Alertness score, and decayRate is the rate at which the Alertness score decays.

        // PHASE 2) Investigate Audio Phase) The AntiJester will move towards the source of the noise. If it reaches the source of the noise, it will enter the next phase should a criteria be met.
        // Activated when the alertness score is above the threshold.
        // Get the source of the last noise generated. Move towards the source of the noise.
        // Once the AntiJester reaches the source of the noise, it will enter the next phase

        // PHASE 3) Winding up phase: The AntiJester will wind up for a few seconds before it starts to chase the player.
        // The AntiJester will play a sound cue to indicate that it is about to chase the player.
        // Cannot be stopped unless players are not in the facility anymore.

        // Phase 4) Indscrimate Noise Pursuit)
        // The AntiJester will chase after any audio it hears within a set radius. This sound can be anything from players, enemies to items even.
        // When it reaches the source, if it is a player, it will instant kill the player. If it is an enemy, deal damage to the enemy. If it is an item, it has a chance to destroy the item.
        // Every time it hears a noise, the timer is reset to a maximum value. If the timer reaches 0, it will return to phase 1 and look for a new location.
        // Timer decreases linearly with time. Timer(t) = Timer Initial - (AngerDecayRate * t), where Timer(t) is the timer at time t, Timer Initial is the initial timer value, and AngerDecayRate is the rate at which the timer decreases.

        // Declare variables and everything below

        private float currentChaseSpeed = 1f;

        private float currentAnimSpeed = 1f;

        public AudioClip[] springNoises;

        public Collider mainCollider;

        private bool stoppingMovement;

        private bool hasStopped;

        private float timeSinceHittingPlayer;

        private float timeSinceReachingSoundLocation; // Timer for Phase 4

        private float TimerInitial = 10f; // Initial timer value for Phase 4

        private float AngerDecayRate = 0.5f; // Rate at which the timer decreases for Phase 4

        private float AlertnessScore = 0f; // Initial Alertness score for Phase 1

        private float AlertnessThreshold = 10f; // Threshold for the Alertness score during Phase 1

        private float AlertnessDecayRate = 0.5f; // Rate at which the alertness score decays during Phase 1

        private Vector3 lastSoundLocation; // Last sound location, used in Phase 2 and 4


        public override void DoAIInterval()
        {
        }

        public override void Update()
        {
        }

        [ServerRpc]
        public void SetAnimationStopServerRpc()
        {
            SetAnimationStopClientRpc();
        }

        [ClientRpc]
        public void SetAnimationStopClientRpc()
        {
            stoppingMovement = true;
        }

        [ServerRpc]
        public void SetAnimationGoServerRpc()
        {
            SetAnimationGoClientRpc();
        }

        [ClientRpc]
        public void SetAnimationGoClientRpc()
        {
            stoppingMovement = false;
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            base.OnCollideWithPlayer(other);
            if (!stoppingMovement && currentBehaviourStateIndex == 1 && !(timeSinceHittingPlayer >= 0f))
            {
                PlayerControllerB controller = other.gameObject.GetComponent<PlayerControllerB>();
                if (controller != null)
                {
                    timeSinceHittingPlayer = 0.2f;
                    controller.DamagePlayer(9999, hasDamageSFX: true, callRPC: true, CauseOfDeath.Mauling, 2);
                    controller.JumpToFearLevel(1f);
                }
            }
        }
    }
}