using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Cabinet : MonoBehaviour
{
    public Card Card;
    private bool Opened;

    public int Level
    {
        get { return GetComponent<Networkable>().Level; }
    }

    public void Activate(Creds creds)
    {
        if (Opened || creds.Level < Level || !GetComponent<Networkable>().Hacked) return;

        Opened = true;
        var card = Instantiate(Card);
        card.transform.position = transform.position + Vector3.up;
        card.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere*5);
        card.Level = GetComponent<Networkable>().Level + Random.Range(1, 3);
        card.Player = Guard._player;
        GetComponent<Light>().enabled = false;
    }
}
