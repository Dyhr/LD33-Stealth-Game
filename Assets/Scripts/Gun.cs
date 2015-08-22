using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public void Fire()
    {
        // TODO pool this
        var bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.AddComponent<Rigidbody>().useGravity = false;
        bullet.AddComponent<Bullet>().Owner = transform.parent;
        bullet.AddComponent<Bullet>().Forward = transform.up;
        bullet.transform.position = transform.position;
        bullet.transform.localScale = Vector3.one * 0.1f;
    }
}
