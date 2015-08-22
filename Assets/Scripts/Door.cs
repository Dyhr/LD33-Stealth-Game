using System.Linq;
using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public AnimationCurve Curve;

    private Vector3 Pre;
    private Vector3 Position;
    private Vector3 Origin;
    private float time;
    private bool guarded;
    private bool protect;
    
    private void Start()
    {
        Ready();
    }

    public void Ready()
    {
        Origin = Pre = Position = transform.localPosition;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time <= 1)
            transform.localPosition = Vector3.Lerp(Pre, Position, Curve.Evaluate(time));
    }

    public void Activate(string ID)
    {
        if(guarded) return;
        guarded = ID.Contains("GUARD");

        Pre = Position;
        if (Position == Origin || guarded)
            Position = Origin - Vector3.up*3;
        else
            Position = Origin;
        time = 0;

        if (guarded)
            StartCoroutine(Unguard());
    }

    IEnumerator Unguard()
    {
        yield return new WaitForSeconds(2);
        Pre = Position;
        Position = Origin;
        time = 0;
    }
}
