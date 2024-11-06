using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum AudioType
    {
        BGM,
        SFX,
    }

    public static AudioManager Instance;

    /// <summary>
    /// 오디오 믹서, 오디오의 타입별로 사운드를 조절.
    /// </summary>
    [SerializeField] private AudioMixer audioMixer;

    /// <summary>
    // 옵션에서 설정된 현재 배경음악과 효과 사운드의 불륨
    /// </summary>
    private float currentBGMVolume, currentEffectVolume;

    /// <summary>
    /// 클립들을 담는 딕셔너리
    /// </summary>
    private Dictionary<string, AudioClip> clipsDictionary;

    /// <summary>
    /// 사전에 미리 로드하여 사용할 클립들
    /// </summary>
    [SerializeField] private AudioClip[] mPreloadClips;

    private List<TemporarySoundPlayer> instantiatedAudios;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        clipsDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in mPreloadClips)
        {
            clipsDictionary.Add(clip.name, clip);
        }

        instantiatedAudios = new List<TemporarySoundPlayer>();
    }

    /// <summary>
    /// 오디오의 이름을 기반으로 찾음.
    /// </summary>
    /// <param name="clipName">오디오의 이름(파일 이름 기준)</param>
    /// <returns></returns>
    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDictionary[clipName];

        if (clip == null) { Debug.LogError(clipName + "이 존재하지 않습니다."); }

        return clip;
    }

    /// <summary>
    /// 사운드를 재생할 때, 루프 형태로 재생된경우에는 나중에 제거하기위해 리스트에 저장.
    /// </summary>
    /// <param name="soundPlayer"></param>
    private void AddToList(TemporarySoundPlayer soundPlayer)
    {
        instantiatedAudios.Add(soundPlayer);
    }

    /// <summary>
    /// 루프 사운드 중 리스트에 있는 오브젝트를 이름으로 찾아 제거.
    /// </summary>
    /// <param name="clipName"></param>
    public void StopLoopSound(string clipName)
    {
        foreach (TemporarySoundPlayer audioPlayer in instantiatedAudios)
        {
            if (audioPlayer.ClipName == clipName)
            {
                instantiatedAudios.Remove(audioPlayer);
                Destroy(audioPlayer.gameObject);
                return;
            }
        }

        Debug.LogWarning(clipName + "을 찾을 수 없습니다.");
    }

    /// <summary>
    /// 2D 사운드로 재생한다. ( 거리에 상관 없이 같은 소리 크기 )
    /// </summary>
    /// <param name="clipName">오디오 클립 이름</param>
    /// <param name="type">오디오 유형(BGM, EFFECT 등.)</param>
    public void PlaySound2D(string _clipName, float _delay = 0f, bool _isLoop = false, AudioType _type = AudioType.SFX)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 2D");
        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //루프를 사용하는경우 사운드를 저장한다.
        if (_isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(_clipName));
        soundPlayer.Play(audioMixer.FindMatchingGroups(_type.ToString())[0], _delay, _isLoop);
    }

    /// <summary>
    /// 3D 사운드로 재생.
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="audioTarget"></param>
    /// <param name="type"></param>
    /// <param name="attachToTarget"></param>
    /// <param name="minDistance"></param>
    /// <param name="maxDistance"></param>
    public void PlaySound3D(string _clipName, Transform _audioTarget, float _delay = 0f, bool _isLoop = false, AudioType _type = AudioType.SFX, bool _attachToTarget = true, float _minDistance = 0.0f, float _maxDistance = 50.0f)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 3D");
        obj.transform.localPosition = _audioTarget.transform.position;
        if (_attachToTarget) { obj.transform.parent = _audioTarget; }

        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //루프를 사용하는경우 사운드를 저장한다.
        if (_isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound3D(GetClip(_clipName), _minDistance, _maxDistance);
        soundPlayer.Play(audioMixer.FindMatchingGroups(_type.ToString())[0], _delay, _isLoop);
    }

    // 씬이 로드될 때 옵션 매니저에의해 모든 사운드 불륨을 저장된 옵션의 크기로 초기화.
    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(AudioType.BGM, bgm);
        SetVolume(AudioType.SFX, effect);
    }

    // 옵션을 변경할 때 소리의 불륨을 조절.
    public void SetVolume(AudioType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }

    /// <summary>
    /// 무작위 사운드를 실행하기위해 랜덤 값을 리턴 (included)
    /// </summary>
    /// <param name="from">시작하는 인덱스 번호</param>
    /// <param name="includedTo">끝나는 인덱스 번호(포함)</param>
    /// <param name="isStartZero">한자리일경우 0으로 시작하는가? 예)01</param>
    /// <returns></returns>
    public static string Range(int _from, int _includedTo, bool _isStartZero = false)
    {
        if (_includedTo > 100 && _isStartZero) { Debug.LogWarning("0을 포함한 세자리는 지원하지 않습니다."); }

        int value = UnityEngine.Random.Range(_from, _includedTo + 1);

        return value < 10 && _isStartZero ? '0' + value.ToString() : value.ToString();
    }
}
