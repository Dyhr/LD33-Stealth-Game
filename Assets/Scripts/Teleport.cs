using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    public Player Player;

    private bool On = false;
    private Vector3 target;
    private int i = 0;

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, target, 0.05f);
    }

    private void Start()
    {
        target.y = transform.localScale.y;
        StartCoroutine(Init());
        StartCoroutine(Cycle());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(4);
        On = true;
        Instantiate(Player, transform.position, Quaternion.identity);
    }
    IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            target = Vector3.one * (On ? (++i%2 == 0 ? 0.2f : 0.5f) : 0);
            target.y = transform.localScale.y;
        }
    }
}
