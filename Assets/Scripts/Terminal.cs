using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Terminal : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Networkable>().Level = Random.Range(1, 4);
    }

    public void Activate(Human human)
    {
        var player = human.GetComponent<Player>();
        if (player != null) player.Hack = player.Hack != GetComponent<Networkable>() ? GetComponent<Networkable>() : null;
    }
}
