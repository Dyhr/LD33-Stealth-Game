using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class MergeMesh : MonoBehaviour
{
    private Dictionary<Material, Mesh> Arr = new Dictionary<Material, Mesh>();

    public void Merge(Transform[] transforms)
    {
        var comb = new Dictionary<Material,List<CombineInstance>>();
        for (int i = 0; i < transforms.Length; ++i)
        {
            var t = transforms[i];
            for (int j = 0; j < t.childCount; ++j)
            {
                Merge(new[] { t.GetChild(j) });
            }

            var rend = t.GetComponent<MeshRenderer>();
            var filt = t.GetComponent<MeshFilter>();
            if (rend == null) continue;

            if (!Arr.ContainsKey(rend.sharedMaterial))
            {
                var go = new GameObject("MergedMesh");
                go.AddComponent<MeshRenderer>().sharedMaterial = rend.sharedMaterial;
                var m = new Mesh();
                m.name = "MergedMesh";
                go.AddComponent<MeshFilter>().sharedMesh = m;
                go.transform.parent = transform;
                Arr.Add(rend.sharedMaterial, m);
            }
            var c = new CombineInstance();
            c.mesh = filt.sharedMesh;
            c.transform = t.localToWorldMatrix;
            
            if(!comb.ContainsKey(rend.sharedMaterial))
                comb.Add(rend.sharedMaterial,new List<CombineInstance>());
            comb[rend.sharedMaterial].Add(c);

            rend.enabled = false;
        }
        foreach (var com in comb.Keys)
        {
            Arr[com].CombineMeshes(comb[com].ToArray());
        }
    }

    private S[] Select<T,S>(T[] arr, Func<T,S> selector)
    {
        var result = new S[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            result[i] = selector(arr[i]);
        }
        return result;
    }

    private T[] Concat<T>(T[] a, T[] b)
    {
        var result = new T[a.Length+b.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i];
        }
        for (int i = 0; i < b.Length; i++)
        {
            result[i+a.Length] = b[i];
        }
        return result;
    }
}
