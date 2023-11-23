using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public float Life = 5;

    public GameObject[] test;

    private void Start()
    {
       
    }

    private void OnEnable()
    {
        GameManager.OnHealthChange += OnHealthChange;
    }

    private void OnDisable()
    {
        GameManager.OnHealthChange -= OnHealthChange;
    }

    private void OnHealthChange(float change) {
        if (Mathf.Floor(Life) == Mathf.Floor(change))
            return;

        Life = change;
        
        for (int i = 0; i < gameObject.transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i < Life);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
