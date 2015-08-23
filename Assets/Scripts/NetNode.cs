using UnityEngine;
using System.Collections;

public class NetNode : MonoBehaviour
{
    private Networkable _origin;

    public bool Hover;

    public Networkable Origin
    {
        set { _origin = value; }
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Hover ? Vector3.one * 3 : Vector3.one * 2, 0.1f);

        Hover = false;
    }
}
