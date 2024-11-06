using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class TemporarySoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public string ClipName
    {
        get
        {
            return audioSource.clip.name;
        }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup _audioMixer, float _delay, bool _isLoop)
    {
        audioSource.outputAudioMixerGroup = _audioMixer;
        audioSource.loop = _isLoop;
        audioSource.Play();

        // 루프가 아니면 지우기.
        if (!_isLoop) { StartCoroutine(COR_DestroyWhenFinish(audioSource.clip.length)); }
    }

    public void InitSound2D(AudioClip _clip)
    {
        audioSource.clip = _clip;
    }

    public void InitSound3D(AudioClip _clip, float _minDistance, float _maxDistance)
    {
        audioSource.clip = _clip;
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = _minDistance;
        audioSource.maxDistance = _maxDistance;
    }

    private IEnumerator COR_DestroyWhenFinish(float _clipLength)
    {
        yield return new WaitForSeconds(_clipLength);

        Destroy(gameObject);
    }
}
