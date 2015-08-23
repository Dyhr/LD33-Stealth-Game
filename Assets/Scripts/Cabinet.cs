using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Cabinet : MonoBehaviour
{
    public Transform Card;
    private bool Opened;

    public int Level
    {
        get { return GetComponent<Networkable>().Level; }
    }

    public void Activate(Creds creds)
    {
        if (creds.Level < Level || !GetComponent<Networkable>().Hacked) return;

        var card = Instantiate(Card);
    }
}
