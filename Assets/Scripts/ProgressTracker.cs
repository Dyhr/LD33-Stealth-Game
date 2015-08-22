﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour
{

    public static ProgressTracker INSTANCE;

    public int Level = 0;

    private void Start()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }
        INSTANCE = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StatusCheck());

        Rebuild();
    }

    private void Rebuild()
    {
        SoftReset();
        Random.seed = 1337;
        FindObjectOfType<Level>().Remake();
        FindObjectOfType<AstarPath>().Scan();
        foreach (var guard in FindObjectsOfType<Guard>())
            guard.Interrupt();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            HardReset();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Rebuild();
    }

    private void HardReset()
    {
        SoftReset();
        Label.INSTANCE = null;
        Application.LoadLevel(0);
    }
    private void SoftReset()
    {
        Guard._switches = null;
        Guard._player = null;
        Guard._playerr = null;
        Guard._patrols.Clear();
    }

    IEnumerator StatusCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);
            // TODO show death screen
        }
    }
}
