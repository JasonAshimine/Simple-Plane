using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject target = null;
    public float moveSpeed = 2f;
    public float MaxLife = 5f;
    public float Life;

    private SpriteRenderer Sprite;

    // Start is called before the first frame updatea
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag(LAYER.Player.ToString());
        Sprite = GetComponent<SpriteRenderer>();
        Life = MaxLife;
    }

    private void OnEnable()
    {
        Life = MaxLife;
        Sprite.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == LAYER.Enemy.ToString())
        {
            OnHit();
        }
    }

    public void OnHit()
    {
        Life -= 1;

        Color color = Color.white;
        color.g *= (Life / MaxLife);
        color.b *= (Life / MaxLife);

        Sprite.color = color;

        SoundManager.instance.Enemy(AudioKey.Impact);

        if (Life <= 0)
            kill();
    }

    public void kill()
    {
        GameManager.instance.kill(Pool_Name.Enemy, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }
}
