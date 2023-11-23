using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveVal = Vector3.zero;    

    public float FireCD = 0.5f;
    private bool canFire = true;

    public float InvulnerabilityTime = 0.5f;

    public float Regen = 0.5f;
    public float RegenRate = 0.5f;
    private float time = 0;

    public static Action<GameObject> OnDamage;

    private PlayerContols inputActions;

    private void Awake()
    {
        inputActions = new PlayerContols();
    }


    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += onMoveCanceled;

        inputActions.Player.Shoot.performed += OnFirePerformed;
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= onMoveCanceled;

        inputActions.Player.Shoot.performed -= OnFirePerformed;

        inputActions.Player.Jump.performed -= OnJump;
    }

    void OnMovePerformed(InputAction.CallbackContext value)
    {
        moveVal = value.ReadValue<Vector2>() * moveSpeed;
    }
    
    void onMoveCanceled(InputAction.CallbackContext value)
    {
        moveVal = Vector3.zero;
    }

    void OnFirePerformed(InputAction.CallbackContext context)
    {
        if(GameManager.instance.state != GameState.Menu)
            Fire();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        //GameManager.instance.spawnEnemy(transform);
    }


    private bool canTakeDamage = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canTakeDamage || GameManager.instance.state == GameState.Menu)
            return;

        if(collision.gameObject.tag == LAYER.Enemy.ToString())
        {
            canTakeDamage = false;
            GameManager.instance.PlayerCollisionHandler(collision);

            if(gameObject.activeSelf)
                StartCoroutine(ThrottleRate(InvulnerabilityTime, () => canTakeDamage = true));
            else
                Debug.Log("Take Damage StartCoroutine Error");            
        }
    }

    private void Fire()
    {
        if (!canFire)
            return;
  
        canFire = false;
        GameManager.instance.spawnBullet(transform);

        if (gameObject.activeSelf)
            StartCoroutine(ThrottleRate(FireCD, () => canFire = true));
        else
            Debug.Log("Shoot StartCoroutine Error");
    }   

    private IEnumerator ThrottleRate(float rate, Action cb)
    {
        yield return new WaitForSeconds(rate);
        cb();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameState.Menu)
            return;

        transform.position += moveVal * Time.deltaTime;
        clampPlayerMovement();

        RotateToMouse();

        if(canTakeDamage)
            time += Time.deltaTime;

        if(time > RegenRate)
        {
            GameManager.instance.updatePlayerHealth(Regen);
            time = 0;
        }
    }

    void clampPlayerMovement()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }


    private void RotateToMouse()
    {
        Vector3 direction = Mouse.current.position.ReadValue();
        direction.z = -Camera.main.transform.position.z;
        direction = Camera.main.ScreenToWorldPoint(direction) - transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
