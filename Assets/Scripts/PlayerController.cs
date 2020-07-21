using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float forceJump = 7; // req 7
    public float speedWalk = 6; // req 6

    public UnityEvent onScoreChange;
    public UnityEvent onDeath;

    public AudioManager audioManager;

    private int _jumpQuantity = 0; // считает колличетво прыжков

    private float _forceJumpLose = 5f; // Сила прыжка при проигрыше

    private float _startPos; // Позиция начала касания
    private float _endPos; // Позиция конца касания

    private bool _isJump = false;
    private bool _isRoll = false;

    private Vector3 _ccNormalP = new Vector3(0f, 0.9f, 0f);
    private float _ccNormalH = 2f;

    private Vector3 _ccRollP = new Vector3(0f, 0.15f, 0f);
    private float _ccRollH = 0.5f;

    private Rigidbody _rigid;
    [NonSerialized] public Animator animator;
    private CapsuleCollider _capsuleCollider;

    [NonSerialized] public bool isDeath = true;

    private static int _deathAdv = 0;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("3723809");
        }
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
        if (!isDeath)
        {
            animator.SetBool("Jump", true);
            _rigid.velocity = new Vector2(_rigid.velocity.x, 0);
            _rigid.AddForce(Vector2.up * forceJump, ForceMode.Impulse);

            if (PlayerPrefs.GetInt("Volume") == 1) audioManager.Play("Jump");

            yield return new WaitForSeconds(1.1f); // req 1.1f
            animator.SetBool("Jump", false);
            _jumpQuantity = 0;
            _isJump = false;

            onScoreChange.Invoke();
        }
    }

    private IEnumerator DoRoll()
    {
        if (!isDeath)
        {
            _isRoll = true;
            _capsuleCollider.height = _ccRollH;
            _capsuleCollider.center = _ccRollP;
            animator.SetBool("Roll", true);

            if (PlayerPrefs.GetInt("Volume") == 1) audioManager.Play("Roll");

            yield return new WaitForSeconds(0.8f);
            _isRoll = false;
            animator.SetBool("Roll", false);
            _capsuleCollider.height = _ccNormalH;
            _capsuleCollider.center = _ccNormalP;

            onScoreChange.Invoke();
        }
    }

    private void OnCollisionEnter(Collision other) // Проигрыш
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (_deathAdv == 1)
            {
                if (Advertisement.IsReady())
                {
                    Advertisement.Show("video");
                }

                _deathAdv = 0;
            }
            else _deathAdv++;


            other.collider.isTrigger = true;

            isDeath = true;

            if (PlayerPrefs.GetInt("Volume") == 1)
            {
                audioManager.Stop("MainTheme");
                audioManager.Play("Lose");
            }

            speedWalk = 0f;
            _rigid.AddForce(Vector2.up * _forceJumpLose, ForceMode.Impulse);
            animator.SetBool("Lose", true);
            onDeath.Invoke();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}