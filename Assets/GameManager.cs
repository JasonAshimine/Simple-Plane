using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action<float> OnHealthChange;
    public static event Action<GameState> OnStateChange;

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }


    public GameState state;
    
    public GameObject Bullet;
    public GameObject Enemy;
    public GameObject Player;
    public GameObject ScoreScreen;

    public static Dictionary<Pool_Name, PoolHelper> Pools;

    public float Enemy_Growth = 1;
    public int Enemy_Growth_Rate = 5;
    public int Enemy_Split_Start = 5;
    public float Enemy_Base_HP = 1;

    public TMP_Text Score;
    public TMP_Text HighScore;
    private int _HighScore = 0;
    private int _Score = 0;

    public float life = 5;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
            return;
        }

        updateEnemyMaxHealth();

        Pools = new Dictionary<Pool_Name, PoolHelper>();

        Pools.Add(Pool_Name.Enemy, new PoolHelper(Enemy));
        Pools.Add(Pool_Name.Bullet, new PoolHelper(Bullet));

        OnStateChange += handleState;

        _HighScore = PlayerPrefs.GetInt(save.HighScore.ToString(), 0);
        updateHighScore();
    }

  
    private void OnDestroy()
    {
        OnStateChange -= handleState;
    }   

    private void handleState(GameState state)
    {
        if (state == GameState.Menu)
            PauseGame();
        else
            ResumeGame();
    }

    public void ChangeState(GameState gs)
    {
        state = gs;
        OnStateChange?.Invoke(gs);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);

        SceneManager.LoadScene("Game Over");

        int highScore = PlayerPrefs.GetInt(save.HighScore.ToString(), 0);

        PlayerPrefs.SetInt(save.Score.ToString(), _Score);

        if (_Score > highScore)
            PlayerPrefs.SetInt(save.HighScore.ToString(), _Score);
    }

    public void Restart()
    {
        ChangeState(GameState.Start);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void updatePlayerHealth(float i)
    {
        life += i;

        if (life > 5)
            life = 5;

        if (life <= 0)
        {
            SoundManager.instance.Player(AudioKey.Death);
            GameOver();
            return;
        }

        if (i < 0)
            SoundManager.instance.Player(AudioKey.Impact);

        OnHealthChange?.Invoke(life);
    }


    public void PlayerCollisionHandler(Collision2D collision)
    {
        updatePlayerHealth(-1);
    }

    public GameObject spawnEnemy(Transform transform, float maxLife = 10, bool scale = false)
    {
        GameObject obj = Pools[Pool_Name.Enemy].Get(transform, true);

        Enemy script = obj.GetComponent<Enemy>();

        script.MaxLife = maxLife;
        script.Life = maxLife;

        return obj;
    }

    public GameObject spawnBullet(Transform transform, bool scale = false)
    {
        SoundManager.instance.Bullet(AudioKey.Spawn);
        return Pools[Pool_Name.Bullet].Get(transform, scale);
    }

    public GameObject spawn(Pool_Name tag, Transform transform)
    {
        return Pools[tag].Get(transform);
    }


    public void kill(Pool_Name tag, GameObject gameObject)
    {
        if (!gameObject.activeSelf)
            return;

        if(tag == Pool_Name.Enemy)
        {
            EnemyKilled(gameObject);
        }
            

        Pools[tag].Release(gameObject);
    }


    private void EnemyKilled(GameObject gameObject)
    {
        Enemy data = gameObject.GetComponent<Enemy>();

        if (_Score > Enemy_Split_Start && data.MaxLife > 2)
            splitEnemy(data);
        else
            SoundManager.instance.Enemy(AudioKey.Death);

        updateEnemyMaxHealth();
        addScore();
    }


    private void updateEnemyMaxHealth()
    {
        Spawner.MaxLife = (_Score / Enemy_Growth_Rate) * Enemy_Growth + Enemy_Base_HP;
    }

    public void splitEnemy(Enemy data)
    {
        SoundManager.instance.Enemy(AudioKey.Split);

        Transform form = data.transform;

        form.localScale = form.localScale * 0.7f;
        spawnEnemy(form, data.MaxLife * .06f, true);

        form.position *= 1.1f;
        spawnEnemy(form, data.MaxLife * .06f, true);
    }

    public void addScore(int i = 1)
    {
        _Score += i;
        Score.text = string.Format("Score: {0}", _Score);

        if(_Score > _HighScore)
        {
            _HighScore = _Score;
            updateHighScore();
        }
        
    }

    public void updateHighScore()
    {
        HighScore.text = string.Format("High Score: {0}", _HighScore);
        PlayerPrefs.SetInt(save.HighScore.ToString(), _HighScore);
    }


    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}

public enum GameState
{ 
    Start,
    Victory,
    GameOver,
    Menu
}

public enum save
{
    HighScore,
    Score
}

public enum Pool_Name
{
    Enemy, Bullet
}

public enum LAYER
{
    Enemy,
    Bullet,
    Player
}