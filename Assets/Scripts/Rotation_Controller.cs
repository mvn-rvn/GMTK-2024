using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Controller : MonoBehaviour
{
    GameObject pivot1;
    GameObject pivot2;
    public float rot_degrees;

    // Start is called before the first frame update
    void Start()
    {
        pivot1 = GameObject.Find("ScalePivot");
        pivot2 = GameObject.Find("ScalePivot2");
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, -rot_degrees);
        pivot1.transform.eulerAngles = new Vector3(0, 0, 0);
        pivot2.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
