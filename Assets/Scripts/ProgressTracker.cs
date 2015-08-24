using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ProgressTracker : MonoBehaviour
{

    public static ProgressTracker INSTANCE;

    public int Level = 0;
    public static int[] Seeds;

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

        var now = System.DateTime.Now;
        Random.seed = now.Millisecond * now.Minute + now.Second * now.Hour;

        if(Seeds == null)
            Seeds = new[]
            {
                Random.Range(int.MinValue, int.MaxValue),
                Random.Range(int.MinValue, int.MaxValue),
                Random.Range(int.MinValue, int.MaxValue),
            };

        Rebuild();
    }

    public void Finish()
    {
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        Level++;
        FindObjectOfType<Level>().Rooms += 6;
        if (Level < Seeds.Length)
            Rebuild();
        else
            Winner();
    }

    private void Winner()
    {
        // TODO win the game
    }

    private void Rebuild()
    {
        if (Seeds.Length < Level) return;
        SoftReset();
        Random.seed = Seeds[Level];
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
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
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
        Guard._guards = null;
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
