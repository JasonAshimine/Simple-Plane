using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5;

    private Renderer Renderer;

    private void Start()
    {
        Renderer = GetComponent<Renderer>();
        gameObject.name = "Bullet";
    }

    void OnBecameInvisible()
    {
        kill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == LAYER.Enemy.ToString())
        {
            collision.gameObject.GetComponent<Enemy>().OnHit();
            kill();
        }

    }

    public void kill()
    {
        GameManager.instance.kill(Pool_Name.Bullet, gameObject);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
