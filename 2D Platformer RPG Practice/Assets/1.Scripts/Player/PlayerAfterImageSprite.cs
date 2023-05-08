using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float _activeTime = 0.1f;
    private float _timeActivated;
    private float _alpha;
    [SerializeField]
    private float _alphaSet = 0.8f;
    [SerializeField]
    private float _alphaDecay = 0.85f;

    private Transform _player;

    private SpriteRenderer _sr;
    private SpriteRenderer _playerSR;

    private Color _color;

    private void OnEnable()
    {
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerSR = _player.GetComponent<SpriteRenderer>();

        _alpha = _alphaSet;
        _sr.sprite = _playerSR.sprite;
        transform.position = _player.transform.position;
        transform.rotation = _player.transform.rotation;
        _timeActivated = Time.time;
    }

    private void Update()
    {
        _alpha *= _alphaDecay;
        _color = new Color(1, 1, 1, _alpha);
        _sr.color = _color;

        if(Time.time >= (_timeActivated + _activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
