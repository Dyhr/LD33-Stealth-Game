using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Networkable : MonoBehaviour
{
    public string Name;
    public string Action;
    public int Level;
    public string Status;
    public bool Hacked;
    public List<Networkable> Neighbors;

    private void OnDrawGizmos()
    {
        Fix();

        Gizmos.color = Color.blue;
        foreach (var networkable in Neighbors)
        {
            Gizmos.DrawLine(transform.position,networkable.transform.position);
        }
    }

    private void Update()
    {
        Fix();
    }

    private void Fix()
    {
        foreach (var networkable in Neighbors)
        {
            if (!networkable.Neighbors.Contains(this)) networkable.Neighbors.Add(this);
        }
    }
}

public class Creds
{
    public string Name;
    public int Level;
    public Human Owner;

    public Creds(string name, int level, Human owner)
    {
        Name = name;
        Level = level;
        Owner = owner;
    }
}
