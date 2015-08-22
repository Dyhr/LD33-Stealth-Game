using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HackMode : MonoBehaviour
{
    private int Level = 0;
    public Networkable Origin
    {
        set
        {
            Level = value.Level;
            DoLines(value);
        }
    }

    private Networkable[] items;
    private readonly HashSet<Networkable> Log = new HashSet<Networkable>();

    private void DoLines(Networkable n)
    {
        if (Log.Contains(n)) return;
        Log.Add(n);
        foreach (var neighbor in n.Neighbors)
        {
            MakeLine(n,neighbor);
            if(neighbor.Level <= Level)
                DoLines(neighbor);
        }
    }

    private void MakeLine(Networkable a, Networkable b)
    {
        var dir = -Camera.main.transform.forward;

        var line = new GameObject("Line").AddComponent<LineRenderer>();
        line.transform.parent = transform;
        line.SetWidth(0.4f, 0.4f);
        line.SetPosition(0, a.transform.position + dir * 4);
        line.SetPosition(1, b.transform.position + dir * 4);
    }

    private void Update()
    {

    }
}
