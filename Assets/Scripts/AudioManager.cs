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
    /// ����� �ͼ�, ������� Ÿ�Ժ��� ���带 ����.
    /// </summary>
    [SerializeField] private AudioMixer audioMixer;

    /// <summary>
    // �ɼǿ��� ������ ���� ������ǰ� ȿ�� ������ �ҷ�
    /// </summary>
    private float currentBGMVolume, currentEffectVolume;

    /// <summary>
    /// Ŭ������ ��� ��ųʸ�
    /// </summary>
    private Dictionary<string, AudioClip> clipsDictionary;

    /// <summary>
    /// ������ �̸� �ε��Ͽ� ����� Ŭ����
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
    /// ������� �̸��� ������� ã��.
    /// </summary>
    /// <param name="clipName">������� �̸�(���� �̸� ����)</param>
    /// <returns></returns>
    private AudioClip GetClip(string clipName)
    {
        AudioClip clip = clipsDictionary[clipName];

        if (clip == null) { Debug.LogError(clipName + "�� �������� �ʽ��ϴ�."); }

        return clip;
    }

    /// <summary>
    /// ���带 ����� ��, ���� ���·� ����Ȱ�쿡�� ���߿� �����ϱ����� ����Ʈ�� ����.
    /// </summary>
    /// <param name="soundPlayer"></param>
    private void AddToList(TemporarySoundPlayer soundPlayer)
    {
        instantiatedAudios.Add(soundPlayer);
    }

    /// <summary>
    /// ���� ���� �� ����Ʈ�� �ִ� ������Ʈ�� �̸����� ã�� ����.
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

        Debug.LogWarning(clipName + "�� ã�� �� �����ϴ�.");
    }

    /// <summary>
    /// 2D ����� ����Ѵ�. ( �Ÿ��� ��� ���� ���� �Ҹ� ũ�� )
    /// </summary>
    /// <param name="clipName">����� Ŭ�� �̸�</param>
    /// <param name="type">����� ����(BGM, EFFECT ��.)</param>
    public void PlaySound2D(string _clipName, float _delay = 0f, bool _isLoop = false, AudioType _type = AudioType.SFX)
    {
        GameObject obj = new GameObject("TemporarySoundPlayer 2D");
        TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

        //������ ����ϴ°�� ���带 �����Ѵ�.
        if (_isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(_clipName));
        soundPlayer.Play(audioMixer.FindMatchingGroups(_type.ToString())[0], _delay, _isLoop);
    }

    /// <summary>
    /// 3D ����� ���.
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

        //������ ����ϴ°�� ���带 �����Ѵ�.
        if (_isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound3D(GetClip(_clipName), _minDistance, _maxDistance);
        soundPlayer.Play(audioMixer.FindMatchingGroups(_type.ToString())[0], _delay, _isLoop);
    }

    // ���� �ε�� �� �ɼ� �Ŵ��������� ��� ���� �ҷ��� ����� �ɼ��� ũ��� �ʱ�ȭ.
    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(AudioType.BGM, bgm);
        SetVolume(AudioType.SFX, effect);
    }

    // �ɼ��� ������ �� �Ҹ��� �ҷ��� ����.
    public void SetVolume(AudioType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }

    /// <summary>
    /// ������ ���带 �����ϱ����� ���� ���� ���� (included)
    /// </summary>
    /// <param name="from">�����ϴ� �ε��� ��ȣ</param>
    /// <param name="includedTo">������ �ε��� ��ȣ(����)</param>
    /// <param name="isStartZero">���ڸ��ϰ�� 0���� �����ϴ°�? ��)01</param>
    /// <returns></returns>
    public static string Range(int _from, int _includedTo, bool _isStartZero = false)
    {
        if (_includedTo > 100 && _isStartZero) { Debug.LogWarning("0�� ������ ���ڸ��� �������� �ʽ��ϴ�."); }

        int value = UnityEngine.Random.Range(_from, _includedTo + 1);

        return value < 10 && _isStartZero ? '0' + value.ToString() : value.ToString();
    }
}
