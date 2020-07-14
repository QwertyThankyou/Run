using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float forceJump = 7; // req 7
    public float speedWalk = 6; // req 6

    public UnityEvent onScoreChange;
    public UnityEvent onDeath;

    private int _jumpQuantity = 0; // считает колличетво прыжков

    private float _forceJumpLose = 5f;

    private float _startPos; // Позиция начала касания
    private float _endPos; // Позиция конца касания

    private bool _isJump = false;
    private bool _isRoll = false;

    private Vector3 _ccNormalP = new Vector3(0f, 0.9f, 0f);
    private float _ccNormalH = 2f;
    
    private Vector3 _ccRollP = new Vector3(0f, 0.15f, 0f);
    private float _ccRollH = 0.5f;

    private Rigidbody _rigid;
    private Animator _animator;
    private CapsuleCollider _capsuleCollider;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
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
                        StartCoroutine(DoJump());
                    }
                }
                else if (_startPos > _endPos && _isJump != true)
                {
                    StartCoroutine(DoRoll());
                }
            }
        }
        _rigid.velocity = new Vector2(speedWalk, _rigid.velocity.y);
    }
    
    private IEnumerator DoJump()
    {
        _animator.SetBool("Jump", true);
        _rigid.velocity = new Vector2(_rigid.velocity.x, 0);
        _rigid.AddForce(Vector2.up * forceJump, ForceMode.Impulse);
        yield return new WaitForSeconds(1.1f);                      // req 1.1f
        _animator.SetBool("Jump", false);
        _jumpQuantity = 0;
        _isJump = false;
        
        onScoreChange.Invoke();
    }

    private IEnumerator DoRoll()
    {
        _isRoll = true;
        _capsuleCollider.height = _ccRollH;
        _capsuleCollider.center = _ccRollP;
        _animator.SetBool("Roll", true);
        yield return new WaitForSeconds(0.8f);
        _isRoll = false;
        _animator.SetBool("Roll", false);
        _capsuleCollider.height = _ccNormalH;
        _capsuleCollider.center = _ccNormalP;
        
        onScoreChange.Invoke();
    }

    private void OnCollisionEnter(Collision other)              // Проигрыш
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _rigid.AddForce(Vector2.up * _forceJumpLose, ForceMode.Impulse);
            speedWalk = 0f;
            _animator.SetBool("Lose", true);
            onDeath.Invoke();
        }
    }
}