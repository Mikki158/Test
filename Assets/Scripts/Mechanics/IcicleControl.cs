using Platformer.Gameplay;
using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Platformer.Core.Simulation;

public class IcicleControl : MonoBehaviour
{
    public AudioClip ouch;
    public Icicle icicle;
    public SpriteRenderer sprite;

    internal Collider2D _collider;
    internal AudioSource _audio;

    public Bounds Bounds => _collider.bounds;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Попадание");
            //var icicle = GetComponent<Icicle>();
            if (icicle != null)
            {
                sprite.enabled = false;
                StartCoroutine(RespawnIcicle(icicle, 1.5f));
            }
            Schedule<PlayerDeath>();
        }
    }
    IEnumerator RespawnIcicle(Icicle icicle, float delay)
    {
        yield return new WaitForSeconds(delay);

        sprite.enabled = true;
        icicle.ResetIcicle();
    }
}
