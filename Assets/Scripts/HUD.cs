using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private void Update()
    {

    }

    public void SetCardName(Text text)
    {
        text.text = "Access Card " + NetNode.Roman(GameObject.FindGameObjectWithTag("Player").GetComponent<Human>().Level);
    }
}
