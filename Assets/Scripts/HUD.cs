using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Transform Health;
    public Text Level;
    private float Wid;

    private void Start()
    {
    }

    private void Update()
    {
        if(Guard._player != null)
            Health.localScale = new Vector3(Guard._player.GetComponent<Human>().HP/100,1,1);
        Level.text = "Level "+(ProgressTracker.INSTANCE.Level+1)+"/3";
    }

    public void SetCardName(Text text)
    {
        text.text = "Access Card " + NetNode.Roman(GameObject.FindGameObjectWithTag("Player").GetComponent<Human>().Level);
    }
}
