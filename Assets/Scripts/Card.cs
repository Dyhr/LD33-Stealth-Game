using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public int Level;
    public Transform Player;
    public AudioClip PickupClip;

    private void Update()
    {
        if (Player != null && Vector3.Distance(transform.position, Player.position) < 1.5)
        {
            Player.GetComponent<Human>().Level = Mathf.Max(Player.GetComponent<Human>().Level, Level);
            Player.GetComponent<AudioSource>().PlayOneShot(PickupClip);
            Destroy(gameObject);
        }
    }
}
