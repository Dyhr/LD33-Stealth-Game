using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Terminal : MonoBehaviour
{
    private void Start()
    {
        //GetComponent<Networkable>().Level = Random.Range(1, 4);
    }

    public void Activate(Creds creds)
    {
        if (creds.Owner != null && creds.Owner.GetComponent<Player>())
        {
            var n = GetComponent<Networkable>();
            var player = creds.Owner.GetComponent<Player>();
            player.Hack = player.Hack != n ? n : null;
            n.Status = player.Hack == n ? "Active" : "Idle";
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(GetComponent<Networkable>().ActivateClip);
        }
    }
}
