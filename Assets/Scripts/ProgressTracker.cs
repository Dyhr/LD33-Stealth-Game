using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour
{

    public static ProgressTracker INSTANCE;

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
    }

    private void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            Label.INSTANCE = null;
            Guard._switches = null;
            Guard._player = null;
            Guard._playerr = null;
            Guard._patrols.Clear();
            Application.LoadLevel(0);
        }
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
