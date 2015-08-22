using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Label : MonoBehaviour
{
    public static Label INSTANCE;
    public Text Text;

    public Transform Target
    {
        set
        {
            gameObject.SetActive(value != null);
            if (value != null)
                transform.position = value.position - transform.forward * 5;
        }
    }

    private void Start()
    {
        INSTANCE = this;
        transform.rotation = Camera.main.transform.rotation;
        Text = GetComponentInChildren<Text>();
    }
}
