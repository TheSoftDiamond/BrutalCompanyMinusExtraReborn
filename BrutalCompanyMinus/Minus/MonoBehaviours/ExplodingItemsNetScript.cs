#nullable enable

using System.Collections;
using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours;

internal class ExplodingItemsNetScript : NetworkBehaviour, IHittable
{
    private const float DestroyTimer = 5f;
    private const float FuseDuration = 6f;
    private const float ChainDelay = 0.2f;

    private static readonly float[] FuseLightBlinkTimes =
    [
        0.262f,
        1.267f,
        2.268f,
        3.270f,
        4.271f,
        4.399f,
        4.522f,
        4.646f,
        4.774f,
        4.897f,
        5.021f,
        5.149f,
        5.276f,
        5.400f,
        5.503f,
        5.626f,
        5.750f,
        5.878f
    ];

    private static readonly float[] FuseLightBlinkDurations =
    [
        0.092f,
        0.088f,
        0.088f,
        0.092f,
        0.092f,
        0.088f,
        0.092f,
        0.096f,
        0.092f,
        0.092f,
        0.096f,
        0.092f,
        0.088f,
        0.085f,
        0.088f,
        0.096f,
        0.096f,
        0.088f
    ];

    internal bool HasExploded { get; private set; }
    private bool hasExplosionBeenRequested;
    private bool localClientTriggeredFuse;
    private bool wasHeld;

    private GrabbableObject item = null!;
    private AudioSource mineAudio = null!;
    private AudioSource mineTickSource = null!;
    private AudioClip mineDetonate = null!;
    private AudioClip mineTrigger = null!;
    private Coroutine fuseCoroutine = null!;
    private Light brightLight = null!;
    private Light indirectLight = null!;

    private void Awake()
    {
        item = GetComponent<GrabbableObject>();

        GrabbableLandmine sourceMine = Assets.grabbableLandmine.spawnPrefab.GetComponent<GrabbableLandmine>();
        mineDetonate = sourceMine.mineDetonate;
        mineTrigger = sourceMine.mineTrigger;

        GameObject mineAudioObject = new("ExplodingItemAudio");
        mineAudioObject.transform.SetParent(transform, false);
        mineAudioObject.layer = gameObject.layer;
        mineAudio = mineAudioObject.AddComponent<AudioSource>();
        OccludeAudio mineAudioOcclude = mineAudioObject.AddComponent<OccludeAudio>();
        mineAudioOcclude.useReverb = true;
        mineAudioOcclude.overridingLowPass = false;
        mineAudioOcclude.lowPassOverride = 20000;
        mineAudioOcclude.debugLog = false;
        mineAudio.outputAudioMixerGroup = sourceMine.mineAudio.outputAudioMixerGroup;
        mineAudio.playOnAwake = false;
        mineAudio.volume = sourceMine.mineAudio.volume;
        mineAudio.pitch = sourceMine.mineAudio.pitch;
        mineAudio.spatialBlend = 1f;
        mineAudio.reverbZoneMix = sourceMine.mineAudio.reverbZoneMix;
        mineAudio.dopplerLevel = sourceMine.mineAudio.dopplerLevel;
        mineAudio.spread = sourceMine.mineAudio.spread;
        mineAudio.rolloffMode = AudioRolloffMode.Linear;
        mineAudio.minDistance = 3f;
        mineAudio.maxDistance = 25f;

        GameObject mineTickObject = new("ExplodingItemTickAudio");
        mineTickObject.transform.SetParent(transform, false);
        mineTickObject.layer = gameObject.layer;
        mineTickSource = mineTickObject.AddComponent<AudioSource>();
        OccludeAudio mineTickOcclude = mineTickObject.AddComponent<OccludeAudio>();
        mineTickOcclude.useReverb = true;
        mineTickOcclude.overridingLowPass = false;
        mineTickOcclude.lowPassOverride = 20000;
        mineTickOcclude.debugLog = false;
        mineTickSource.outputAudioMixerGroup = sourceMine.mineTickSource.outputAudioMixerGroup;
        mineTickSource.playOnAwake = false;
        mineTickSource.clip = sourceMine.mineTickSource.clip;
        mineTickSource.loop = sourceMine.mineTickSource.loop;
        mineTickSource.volume = sourceMine.mineTickSource.volume;
        mineTickSource.pitch = sourceMine.mineTickSource.pitch;
        mineTickSource.spatialBlend = 1f;
        mineTickSource.reverbZoneMix = sourceMine.mineTickSource.reverbZoneMix;
        mineTickSource.dopplerLevel = sourceMine.mineTickSource.dopplerLevel;
        mineTickSource.spread = sourceMine.mineTickSource.spread;
        mineTickSource.rolloffMode = AudioRolloffMode.Linear;
        mineTickSource.minDistance = 3f;
        mineTickSource.maxDistance = 25f;

        GameObject brightLightObject = new("BrightLight");
        brightLightObject.transform.SetParent(transform, false);
        brightLightObject.transform.localPosition = Vector3.zero;
        brightLightObject.layer = gameObject.layer;
        brightLight = brightLightObject.AddComponent<Light>();
        brightLight.type = LightType.Point;
        brightLight.color = new Color(1f, 0.33f, 0.33f, 1f);
        brightLight.intensity = 227f;
        brightLight.range = 0.2f;
        brightLight.shadows = LightShadows.None;
        brightLight.colorTemperature = 1500f;
        brightLight.useColorTemperature = true;
        brightLight.enabled = false;

        GameObject indirectLightObject = new("IndirectLight");
        indirectLightObject.transform.SetParent(transform, false);
        indirectLightObject.transform.localPosition = new Vector3(-0.03f, 0.04f, 0.81f);
        indirectLightObject.layer = gameObject.layer;
        indirectLight = indirectLightObject.AddComponent<Light>();
        indirectLight.type = LightType.Point;
        indirectLight.color = new Color(1f, 0.65f, 0.65f, 1f);
        indirectLight.intensity = 436f;
        indirectLight.range = 3f;
        indirectLight.shadows = LightShadows.Hard;
        indirectLight.colorTemperature = 1500f;
        indirectLight.useColorTemperature = true;
        indirectLight.enabled = false;
    }

    private void Update()
    {
        if (HasExploded)
            return;

        bool isHeldNow = item.isHeld || item.isHeldByEnemy;

        if (isHeldNow == wasHeld)
            return;

        wasHeld = isHeldNow;

        if (isHeldNow)
        {
            if ((item.isHeld && item.playerHeldBy == GameNetworkManager.Instance.localPlayerController) || (item.isHeldByEnemy && NetworkManager.Singleton.IsServer))
            {
                localClientTriggeredFuse = true;
                StartFuseServerRpc();
            }
        }
        else if (localClientTriggeredFuse)
        {
            localClientTriggeredFuse = false;
            StopFuseServerRpc();
        }
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void StartFuseServerRpc()
    {
        if (HasExploded || hasExplosionBeenRequested)
            return;

        StartFuseClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void StartFuseClientRpc()
    {
        if (HasExploded || hasExplosionBeenRequested)
            return;

        if (fuseCoroutine != null)
        {
            StopCoroutine(fuseCoroutine);
            fuseCoroutine = null!;
            mineTickSource.Stop();
            brightLight.enabled = false;
            indirectLight.enabled = false;
        }

        fuseCoroutine = StartCoroutine(FuseTimer());
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void StopFuseServerRpc()
    {
        StopFuseClientRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void StopFuseClientRpc()
    {
        if (fuseCoroutine == null)
            return;

        StopCoroutine(fuseCoroutine);
        fuseCoroutine = null!;
        mineTickSource.Stop();
        brightLight.enabled = false;
        indirectLight.enabled = false;
    }

    private IEnumerator FuseTimer()
    {
        float fuseTime = 0f;
        mineTickSource.time = 0f;
        mineTickSource.Play();

        for (int i = 0; i < FuseLightBlinkTimes.Length; i++)
        {
            float waitTime = FuseLightBlinkTimes[i] - fuseTime;

            if (waitTime > 0f)
            {
                yield return new WaitForSeconds(waitTime);
                fuseTime += waitTime;
            }

            brightLight.enabled = true;
            indirectLight.enabled = true;

            yield return new WaitForSeconds(FuseLightBlinkDurations[i]);
            fuseTime += FuseLightBlinkDurations[i];

            brightLight.enabled = false;
            indirectLight.enabled = false;
        }

        float remainingTime = FuseDuration - fuseTime;

        if (remainingTime > 0f)
        {
            yield return new WaitForSeconds(remainingTime);
            fuseTime += remainingTime;
        }

        fuseCoroutine = null!;
        mineTickSource.Stop();
        brightLight.enabled = false;
        indirectLight.enabled = false;

        if (IsServer)
            ExplodeClientRpc(Random.Range(0.93f, 1.07f));
    }

    internal void Explode()
    {
        if (HasExploded || hasExplosionBeenRequested)
            return;

        hasExplosionBeenRequested = true;

        if (IsServer)
            ExplodeClientRpc(Random.Range(0.93f, 1.07f));
        else
            ExplodeServerRpc();
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    private void ExplodeServerRpc()
    {
        if (HasExploded)
            return;

        ExplodeClientRpc(Random.Range(0.93f, 1.07f));
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ExplodeClientRpc(float detonatePitch)
    {
        if (HasExploded)
            return;

        hasExplosionBeenRequested = true;
        HasExploded = true;
        localClientTriggeredFuse = false;

        if (fuseCoroutine != null)
        {
            StopCoroutine(fuseCoroutine);
            fuseCoroutine = null!;
        }

        mineTickSource.Stop();
        brightLight.enabled = false;
        indirectLight.enabled = false;

        mineAudio.PlayOneShot(mineTrigger, 1f);
        mineAudio.pitch = detonatePitch;
        mineAudio.PlayOneShot(mineDetonate, 1f);
        WalkieTalkie.TransmitOneShotAudio(mineAudio, mineTrigger);
        WalkieTalkie.TransmitOneShotAudio(mineAudio, mineDetonate);
        Landmine.SpawnExplosion(transform.position + Vector3.up, spawnExplosionEffect: true, 5.7f, 6f);

        // Set item child mesh and colliders inactive after exploding
        item.EnableItemMeshes(false);
        item.EnablePhysics(false);

        if (item.isHeld && item.playerHeldBy != null)
            item.DestroyObjectInHand(item.playerHeldBy);

        if (IsServer)
            StartCoroutine(DestroyObject());
    }

    internal IEnumerator TriggerOtherMineDelayed()
    {
        if (HasExploded)
            yield break;

        yield return new WaitForSeconds(ChainDelay);

        if (IsServer)
            ExplodeClientRpc(Random.Range(0.75f, 1.07f));
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(DestroyTimer);

        if (item.NetworkObject?.IsSpawned == true)
            item.NetworkObject.Despawn(true);
    }

    bool IHittable.Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit, bool playHitSFX, int hitID)
    {
        if (item.isHeld)
            return false;

        Explode();
        return true;
    }
}
