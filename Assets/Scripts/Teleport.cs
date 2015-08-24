using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    public Player Player;
    public bool Finished;
    public AudioClip TeleClip;

    private Transform player;

    private bool On = false;
    private Vector3 target;
    private int i = 0;

    private void Update()
    {
        transform.FindChild("Cylinder").localScale = Vector3.Lerp(transform.FindChild("Cylinder").localScale, target, 0.05f);
        if (Finished && player != null && Vector3.Distance(transform.position, player.position) < 0.5f)
        {
            On = false;
            Destroy(player.gameObject);
            FindObjectOfType<ProgressTracker>().Finish();
            Player.GetComponent<AudioSource>().PlayOneShot(TeleClip, 0.8f);
        }
    }

    private void Start()
    {
        target.y = transform.FindChild("Cylinder").localScale.y;
        StartCoroutine(Init());
        StartCoroutine(Cycle());
        Camera.main.transform.position = transform.position - Camera.main.transform.forward * 100;
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(3);
        On = true;
        player = ((Player) Instantiate(Player, transform.position, Quaternion.identity)).transform;
        FindObjectOfType<Goal>().Player = player;
    }
    IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            target = Vector3.one * (On ? (++i%2 == 0 ? 0.2f : 0.5f) : 0);
            target.y = transform.FindChild("Cylinder").localScale.y;
        }
    }
}
