using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRate = 2;
    public static float MaxLife = 2;

    private float time = 0;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(string.Format("{0} {1}", Screen.width, Screen.height));

        GameManager.OnStateChange += handleState;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= handleState;
    }

    private void handleState(GameState state) {
        gameObject.SetActive(state != GameState.GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > spawnRate)
        {
            spawn();
            time = 0;
        }
    }

    void spawn()
    {
        transform.position = randomPos();
        GameManager.instance.spawnEnemy(transform, MaxLife);
    }

    private Vector3 randomPos()
    {
        Vector3 direction = Camera.main.ScreenToViewportPoint(Vector3.zero);

        bool RanBool = Random.value > 0.5f;
        int side = Random.value > 0.5f ? 0 : 1;
        
        direction.x = RanBool ? Random.value : side;
        direction.y = !RanBool ? Random.value : side;


        Vector2 final = Camera.main.ViewportToWorldPoint(direction);

        return final;
    }
}
