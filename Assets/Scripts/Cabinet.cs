using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Cabinet : MonoBehaviour
{
    public Card Card;
    private bool Opened;
    private Networkable net;

    private void Start()
    {
        net = GetComponent<Networkable>();
    }

    public int Level
    {
        get { return GetComponent<Networkable>().Level; }
    }

    private void Update()
    {
        if (Guard._player != null && Guard._player.GetComponent<Player>() != null)
            net.Action = Guard._player.GetComponent<Player>().Hack == null ? "" : "Unlock";
    }

    public void Activate(Creds creds)
    {
        if (Opened || creds.Level < Level || !GetComponent<Networkable>().Hacked) return;

        net.Status = "Unlocked";
        Opened = true;
        var card = Instantiate(Card);
        card.transform.position = transform.position + Vector3.up;
        card.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere*5);
        card.Level = net.Level + Random.Range(1, 3);
        card.Player = Guard._player;
        GetComponent<Light>().enabled = false; GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(GetComponent<Networkable>().ActivateClip);
    }
}
