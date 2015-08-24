using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    public float Speed;
    public float Speed2;
    public Transform Player;
    public AudioClip PickupClip;

    private Transform Price;
    private Vector3 target;
    private int i;

    private void Start()
    {
        Price = transform.FindChild("Price");
        StartCoroutine(Cycle());
    }

    private void Update()
    {
        if (Price == null) return;
        Price.Rotate(0, Speed*Time.deltaTime, 0);
        Price.localPosition = Vector3.Lerp(Price.localPosition, target, 0.05f);

        if (Player != null && Vector3.Distance(transform.position, Player.position) < 1.5f)
        {
            Price.parent = Player;
            Price.localPosition = -Vector3.forward*0.5f;
            Price.localEulerAngles = new Vector3(90, 0, 0);
            Price.Rotate(0,45,0,Space.Self);
            Price = null;
            FindObjectOfType<Teleport>().Finished = true;
            Player.GetComponent<AudioSource>().PlayOneShot(PickupClip,0.8f);

            Alert();
            var doors = FindObjectsOfType<Door>();
            foreach (var door in doors)
            {
                door.StopAllCoroutines();
                door.Open = true;
            }
        }
    }
    private void Alert()
    {
        if (FindObjectOfType<AstarPath>() == null || Guard._guards == null) return;
        var p = Player;
        for (int i = 0; i < Guard._guards.Length; ++i)
        {
            if (Guard._guards[i] == null) continue;

            var g = Guard._guards[i].GetComponent<Guard>();
            g.path = null;
            g.targetPosition = p.transform.position;
            g._awaitingPath = true;
            g.seeker.StartPath(g.transform.position, g.targetPosition);
            g.alert = 2;
        }
        p.GetComponent<Player>().Hack = null;
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Speed2);
            target = Vector3.up*(++i%2 == 0 ? 0.2f : 1.5f);
        }
    }
}