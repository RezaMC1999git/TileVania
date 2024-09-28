﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int startIndex;
    private void Awake()
    {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        startIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        int ThisLoadIndex = SceneManager.GetActiveScene().buildIndex;

        if (ThisLoadIndex != startIndex)
            Destroy(gameObject);
    }
    public void DestroyME() 
    {
        Destroy(gameObject);
    }
}
