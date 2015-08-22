using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Networkable))]
public class Door : MonoBehaviour
{
    public AnimationCurve Curve;
    public bool Open;

    public int Level
    {
        get { return GetComponent<Networkable>().Level; }
    }

    private float time;
    private bool guarded;
    private bool protect;

    private void Start()
    {
        time = 1;
    }

    private void Update()
    {
        time += Open ? -Time.deltaTime : Time.deltaTime;
        time = Mathf.Clamp01(time);

        var s = Mathf.Clamp(Curve.Evaluate(time)/2,0.05f,0.5f);
        transform.GetChild(0).localScale = new Vector3(s, 1, 1);
        transform.GetChild(1).localScale = new Vector3(s, 1, 1);
        transform.GetChild(0).localPosition = Vector3.right * (0.5f - s/2);
        transform.GetChild(1).localPosition = -Vector3.right * (0.5f - s/2);
    }

    public void Activate(string ID)
    {
        if(guarded) return;
        guarded = ID.Contains("GUARD");

        var level = IntParseFast(ID[0].ToString());
        if (level < Level) return;

        Open = !Open || guarded;

        if (guarded)
            StartCoroutine(Unguard());
    }

    IEnumerator Unguard()
    {
        yield return new WaitForSeconds(2);
        Open = false;
        guarded = false;
    }

    int IntParseFast(string value)
    {
        var result = 0;
        for (var i = 0; i < value.Length; i++)
        {
            var letter = value[i];
            result = 10 * result + (int)char.GetNumericValue(letter);
        }
        return result;
    }
}
