using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    public Transform Player;

    private void Update()
    {
        if (Player != null && Vector3.Distance(transform.position, Player.position) < 0.5f)
        {
            FindObjectOfType<Teleport>().Finished = true;
        }
    }
}
