using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayController : MonoBehaviour
{
    public AudioClip[] song;
    AudioSource _player;

    private void Start()
    {
        _player = GetComponent<AudioSource>();
        _player.volume = 1f;
    }

    private void Update()
    {
        if (!_player.isPlaying)
        {
            _player.clip = song[Random.Range(0, song.Length)];
            _player.Play();
        }
    }
}
