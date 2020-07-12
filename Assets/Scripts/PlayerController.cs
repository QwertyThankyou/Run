using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float forceJump = 7; // req 7
    public float speedWalk = 6; // req 6

    public Animation roll;

    private int _jumpQuantity = 0; // считает колличетво прыжков

    private float _startPos; // Позиция начала касания
    private float _endPos; // Позиция конца касания

    private bool _isJump = false;
    private bool _isRoll = false;

    private Rigidbody _rigid;
    private Animator _animator;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _startPos = Input.mousePosition.y;
        else if (Input.GetMouseButtonUp(0))
        {
            _endPos = Input.mousePosition.y;

            if (Math.Abs(_startPos - _endPos) > 40)
            {
                if (_startPos < _endPos && _isRoll != true)
                {
                    _isJump = true;
                    
                    _jumpQuantity++;
                    if (_jumpQuantity <= 1)
                    {
                        _animator.SetBool("Jump", true);
                        _rigid.velocity = new Vector2(_rigid.velocity.x, 0);
                        _rigid.AddForce(Vector2.up * forceJump, ForceMode.Impulse);
                    }
                }
                else if (_startPos > _endPos && _isJump != true)
                {
                    StartCoroutine(DoRoll());
                    
                    // addforce down;
                }
            }
        }

        _rigid.velocity = new Vector2(speedWalk, _rigid.velocity.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        _animator.SetBool("Jump", false);
        _jumpQuantity = 0;
        _isJump = false;
    }

    IEnumerator DoRoll()
    {
        _isRoll = true;
        _animator.SetBool("Roll", true);
        yield return new WaitForSeconds(1.0f);
        _isRoll = false;
        _animator.SetBool("Roll", false);
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     _animator.SetBool("isGround", false);
    // }
}