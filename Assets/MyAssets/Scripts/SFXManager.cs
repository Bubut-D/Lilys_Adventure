using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    public enum Clip
    {
        Success, Failure, Win
    };


    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(Clip clip)
    {
        source.clip = clips[(int)clip];
        source.Play();
    }
}
