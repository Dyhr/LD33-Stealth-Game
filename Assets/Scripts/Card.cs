using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public int Level;
    public Transform Player;

    private void Update()
    {
        if (Player != null && Vector3.Distance(transform.position, Player.position) < 0.5)
        {
            Player.GetComponent<Human>().Level = Mathf.Max(Player.GetComponent<Human>().Level, Level);
            Destroy(gameObject);
        }
    }
}
