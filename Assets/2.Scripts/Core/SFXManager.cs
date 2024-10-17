using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXManager : CoreComponent
{
    private AudioSource _bgmSource = null;
    public List<AudioClip> _bgmClips = null;

    private Transform _directory = null;

    protected override void InitOptions()
    {
        _directory = new GameObject().transform;
        _directory.SetParent(Manager.Instance.transform);
        _directory.name = "SFX";

        _bgmSource = new GameObject().AddComponent<AudioSource>();
        _bgmSource.transform.SetParent(_directory);
        _bgmSource.playOnAwake = false;
        _bgmSource.name = "BGM";
    }

    public void PlayFX()
    {

    }
}
