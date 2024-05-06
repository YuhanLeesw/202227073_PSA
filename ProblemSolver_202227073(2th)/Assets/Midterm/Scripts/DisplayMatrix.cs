using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMatrix : MonoBehaviour
{
    public TextMeshProUGUI txtMx;
    

    // Update is called once per frame
    void Update()
    {
      Matrix4x4 mx = gameObject.GetComponent<Transform>().localToWorldMatrix;
        string text;
        text = string.Format("<color=red>{0:0.00}</color>  <color=green>{1:0.00}</color>  <color=blue>{2:0.00}</color>  <color=black>{3:0.00}</color> \n", mx.m00, mx.m01, mx.m02, mx.m03);
        text += string.Format("<color=red>{0:0.00}</color>  <color=green>{1:0.00}</color>  <color=blue>{2:0.00}</color>  <color=black>{3:0.00}</color> \n", mx.m10, mx.m11, mx.m12, mx.m13);
        text += string.Format("<color=red>{0:0.00}</color>  <color=green>{1:0.00}</color>  <color=blue>{2:0.00}</color>  <color=black>{3:0.00}</color> \n", mx.m20, mx.m21, mx.m22, mx.m23);
        text += string.Format("<color=red>{0:0.00}</color>  <color=green>{1:0.00}</color>  <color=blue>{2:0.00}</color>  <color=black>{3:0.00}</color>", mx.m30, mx.m31, mx.m32, mx.m33);
        txtMx.text =text;
    }
}
