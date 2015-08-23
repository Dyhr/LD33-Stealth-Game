using UnityEngine;
using System.Collections;

public class NetNode : MonoBehaviour
{
    private Networkable _origin;

    public bool Hover;
    public bool Hacked;

    public Networkable Origin
    {
        set { _origin = value; }
        get { return _origin; }
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,
            Vector3.one * ((Hover ? 3f : 2f) / (Hacked ? 1f : 1.5f)), 0.1f);

        Hover = false;
    }

    public void Activate(Creds creds)
    {
        if (!Hacked)
        {
            if (_origin.Level <= creds.Level) Hacked = true;
        }
        else
        {
            _origin.SendMessage("Activate", creds);
        }
    }
}
