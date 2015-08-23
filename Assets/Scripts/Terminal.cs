using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Terminal : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Networkable>().Level = Random.Range(1, 4);
    }

    public void Activate(Creds creds)
    {
        if (creds.Owner != null && creds.Owner.GetComponent<Player>())
        {
            var player = creds.Owner.GetComponent<Player>();
            player.Hack = player.Hack != GetComponent<Networkable>() ? GetComponent<Networkable>() : null;
        }
    }
}
