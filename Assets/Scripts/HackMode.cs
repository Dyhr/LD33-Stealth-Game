using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class HackMode : MonoBehaviour
{
    private int Level = 0;

    public Material NodeMaterial;
    public Material PlaneMaterial;
    public Transform NodeLabel;

    public Networkable Origin
    {
        set
        {
            MakePlane();
            Level = value.Level;
            DrawNode(value, true);
            DoLines(value);
        }
    }

    private Networkable[] items;
    private readonly HashSet<Networkable> LogA = new HashSet<Networkable>();
    private readonly HashSet<Networkable> LogB = new HashSet<Networkable>();

    private void DoLines(Networkable n)
    {
        if (LogA.Contains(n)) return;
        LogA.Add(n);
        DrawNode(n);
        foreach (var neighbor in n.Neighbors)
        {
            DrawNode(neighbor);
            MakeLine(n,neighbor);
            if (neighbor.Level <= Level && neighbor.Hacked)
                DoLines(neighbor);
        }
    }

    private void DrawNode(Networkable n, bool hacked = false)
    {
        if (LogB.Contains(n)) return;
        LogB.Add(n);
        var dir = -Camera.main.transform.forward;

        var circle = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        circle.position = n.transform.position + dir * 8;
        circle.parent = transform;
        circle.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        circle.GetComponent<MeshRenderer>().receiveShadows = false;
        circle.GetComponent<MeshRenderer>().sharedMaterial = NodeMaterial;
        circle.gameObject.AddComponent<NetNode>().Label = NodeLabel;
        circle.GetComponent<NetNode>().Origin = n;
        circle.GetComponent<NetNode>().CanHack = n.Level <= Level;
        if (hacked) circle.GetComponent<NetNode>().Hacked = true;
    }

    private void MakePlane()
    {
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
        plane.position = FindObjectOfType<Level>().transform.position + Vector3.up*2.5f;
        plane.localScale = Vector3.one*40;
        plane.parent = transform;
        plane.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
        plane.GetComponent<MeshRenderer>().receiveShadows = false;
        plane.GetComponent<MeshRenderer>().sharedMaterial = PlaneMaterial;
    }

    private void MakeLine(Networkable a, Networkable b)
    {
        var dir = -Camera.main.transform.forward;

        var line = new GameObject("Line").AddComponent<LineRenderer>();
        line.transform.parent = transform;
        line.SetWidth(0.4f, 0.4f);
        line.SetPosition(0, a.transform.position + dir * 7);
        line.SetPosition(1, b.transform.position + dir * 7);
        line.sharedMaterial = NodeMaterial;
    }

    private void Update()
    {
        var cam = Camera.main;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            var node = hit.transform.GetComponent<NetNode>();
            if (node != null)
            {
                node.Hover = true;
                if (Input.GetButtonDown("Fire1"))
                {
                    node.Activate(new Creds("Hack",Level, GameObject.FindGameObjectWithTag("Player").GetComponent<Human>()));
                    if (node.Hacked)
                    {
                        DoLines(node.Origin);
                    }
                }
            }
        }
    }
}
