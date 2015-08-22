using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Bullet Bullet;

    public void Fire()
    {
        // TODO pool this
        var bullet = Instantiate(Bullet).transform;
        bullet.GetComponent<Bullet>().Owner = transform.parent;
        bullet.GetComponent<Bullet>().Forward = transform.up;
        bullet.transform.position = transform.position;
    }
}
