using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Bullet Bullet;
    public float Recharge;
    public float Inaccuracy;
    public AudioClip ShootClip;

    private float time;
    private AudioSource player;

    private void Update()
    {
        if (player == null)
            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        time -= Time.deltaTime;
    }

    public void Fire()
    {
        if (time > 0) return;

        if(player != null)
            player.PlayOneShot(ShootClip,0.4f);

        // TODO pool this
        var bullet = Instantiate(Bullet).transform;
        bullet.GetComponent<Bullet>().Owner = transform.parent;
        bullet.GetComponent<Bullet>().Forward = (transform.up + Random.insideUnitSphere*Inaccuracy).normalized;
        bullet.transform.position = transform.position;
        time = Recharge;
    }
}
