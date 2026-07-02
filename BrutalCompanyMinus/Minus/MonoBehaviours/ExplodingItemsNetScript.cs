#nullable enable

using System.Collections;
using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace BrutalCompanyMinus.Minus.MonoBehaviours;

public class ExplodingItemsNetScript : MonoBehaviour, IHittable
{
    public const float DestroyTimer = 5f;
    public const float FuseDuration = 6f;
    public const float ChainDelay = 0.2f;
    public const float FuseLightBlinkInterval = 0.5f;
    public const float FuseLightOnDuration = 0.07f;

    public bool HasExploded { get; private set; }
    public bool wasHeld;

    public GrabbableObject item = null!;
    public AudioSource mineAudio = null!;
    public AudioSource mineTickSource = null!;
    public AudioClip mineDetonate = null!;
    public AudioClip mineTrigger = null!;
    public Coroutine fuseCoroutine = null!;
    public Light brightLight = null!;
    public Light indirectLight = null!;

    public void Awake()
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
        mineAudio.spatialBlend = sourceMine.mineAudio.spatialBlend;
        mineAudio.reverbZoneMix = sourceMine.mineAudio.reverbZoneMix;
        mineAudio.dopplerLevel = sourceMine.mineAudio.dopplerLevel;
        mineAudio.spread = sourceMine.mineAudio.spread;
        mineAudio.rolloffMode = sourceMine.mineAudio.rolloffMode;
        mineAudio.minDistance = sourceMine.mineAudio.minDistance;
        mineAudio.maxDistance = sourceMine.mineAudio.maxDistance;

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
        mineTickSource.spatialBlend = sourceMine.mineTickSource.spatialBlend;
        mineTickSource.reverbZoneMix = sourceMine.mineTickSource.reverbZoneMix;
        mineTickSource.dopplerLevel = sourceMine.mineTickSource.dopplerLevel;
        mineTickSource.spread = sourceMine.mineTickSource.spread;
        mineTickSource.rolloffMode = sourceMine.mineTickSource.rolloffMode;
        mineTickSource.minDistance = sourceMine.mineTickSource.minDistance;
        mineTickSource.maxDistance = sourceMine.mineTickSource.maxDistance;

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

    public void Update()
    {
        if (HasExploded)
            return;

        bool isHeldNow = item.isHeld || item.isHeldByEnemy;

        if (isHeldNow == wasHeld)
            return;

        wasHeld = isHeldNow;

        if (isHeldNow)
        {
            // On grab
            if (HasExploded)
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
        else
        {
            // On discard
            if (fuseCoroutine == null)
                return;

            StopCoroutine(fuseCoroutine);
            fuseCoroutine = null!;
            mineTickSource.Stop();
            brightLight.enabled = false;
            indirectLight.enabled = false;
        }
    }

    public IEnumerator FuseTimer()
    {
        float fuseTime = 0f;
        mineTickSource.time = 0f;
        mineTickSource.Play();

        // Landmine "animator"
        while (fuseTime < FuseDuration)
        {
            brightLight.enabled = true;
            indirectLight.enabled = true;

            yield return new WaitForSeconds(FuseLightOnDuration);
            fuseTime += FuseLightOnDuration;

            brightLight.enabled = false;
            indirectLight.enabled = false;

            float tickDelay = Mathf.Min(Mathf.Max(0f, FuseLightBlinkInterval - FuseLightOnDuration), FuseDuration - fuseTime);
            yield return new WaitForSeconds(tickDelay);
            fuseTime += tickDelay;
        }

        fuseCoroutine = null!;
        mineTickSource.Stop();
        brightLight.enabled = false;
        indirectLight.enabled = false;
        ExplodeServerRpc();
    }

    public void Explode()
    {
        if (HasExploded)
            return;

        HasExploded = true;

        if (fuseCoroutine != null)
        {
            StopCoroutine(fuseCoroutine);
            fuseCoroutine = null!;
        }

        mineTickSource.Stop();
        brightLight.enabled = false;
        indirectLight.enabled = false;

        mineAudio.PlayOneShot(mineTrigger, 1f);
        mineAudio.pitch = Random.Range(0.93f, 1.07f);
        mineAudio.PlayOneShot(mineDetonate, 1f);
        WalkieTalkie.TransmitOneShotAudio(mineAudio, mineTrigger);
        WalkieTalkie.TransmitOneShotAudio(mineAudio, mineDetonate);
        Landmine.SpawnExplosion(transform.position + Vector3.up, spawnExplosionEffect: true, 5.7f, 6f);

        // Set item child mesh and colliders inactive after exploding
        item.EnableItemMeshes(false);
        item.EnablePhysics(false);

        if (item.isHeld && item.playerHeldBy != null)
            item.DestroyObjectInHand(item.playerHeldBy);

        if (NetworkManager.Singleton.IsServer)
            StartCoroutine(DestroyObject());
    }

    public IEnumerator TriggerOtherMineDelayed()
    {
        if (HasExploded)
            yield break;

        mineAudio.pitch = Random.Range(0.75f, 1.07f);
        yield return new WaitForSeconds(ChainDelay);
        ExplodeServerRpc();
    }

    public IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(DestroyTimer);

        if (item.NetworkObject?.IsSpawned == true)
            item.NetworkObject.Despawn(true);
    }

    bool IHittable.Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit, bool playHitSFX, int hitID)
    {
        if (item.isHeld)
            return false;

        ExplodeServerRpc();
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ExplodeServerRpc()
    {
        ExplodeClientRpc();
    }

    [ClientRpc]
    public void ExplodeClientRpc()
    {
        Explode();
    }
}
