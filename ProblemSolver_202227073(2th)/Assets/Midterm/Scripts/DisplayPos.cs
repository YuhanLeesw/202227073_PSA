using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPos : MonoBehaviour
{
    public TextMeshProUGUI txtPos;

    // Update is called once per frame
    void Update()
    {
        if(txtPos != null)
        {
            Vector3 pos = gameObject.GetComponent<Transform>().position;
            txtPos.text = string.Format("<color=red>{0:0.00}</color>  <color=green>{1:0.00}</color>  <color=blue>{2:0.00}</color>",
                                            pos.x, pos.y, pos.z);
        }
    }
}
