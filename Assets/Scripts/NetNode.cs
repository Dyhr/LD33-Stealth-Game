using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetNode : MonoBehaviour
{
    private Networkable _origin;
    private Transform canvas;
    private Text text;

    public bool Hacked
    {
        get { return _origin.Hacked; }
        set { _origin.Hacked = true; }
    }

    public bool CanHack;
    public bool Hover;
    public Transform Label;

    public Networkable Origin
    {
        set
        {
            _origin = value;
            canvas = Instantiate(Label);
            canvas.SetParent(transform, true);
            canvas.position = transform.position;
            canvas.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            canvas.SetParent(transform.parent, true);
            text = canvas.GetComponentInChildren<Text>();

            text.text = Roman(value.Level) + " HACK " + Garble(value.Name + " - " + value.Status);
        }
        get { return _origin; }
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,
            Vector3.one * ((Hover ? 3f : 2f) / (Hacked ? 1f : 1.5f)), 0.1f);

        Hover = false;
        text.text = Roman(_origin.Level) + " " + (!Hacked
            ? (CanHack 
                ? "HACK " + Garble(_origin.Name + " - " + _origin.Status)
                : Garble("HACK " + _origin.Name + " - " + _origin.Status))
            : _origin.Action + " " + _origin.Name + " - " + _origin.Status);
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

    public static string Garble(string s)
    {
        return new string(RandomizeArray(s.ToCharArray()));
    }
    public static T[] RandomizeArray<T>(T[] arr)
    {
        for (var i = arr.Length - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
        return arr;
    }

    public static string Roman(int value)
    {
        if (value == 0) return "0";

        var arabic = new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        var roman = new[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        int i;
        var result = "";
        for (i = 0; i < 13; i++)
        {
            while (value >= arabic[i])
            {
                result = result + roman[i];
                value = value - arabic[i];
            }
        }
        return result;
    }
}
