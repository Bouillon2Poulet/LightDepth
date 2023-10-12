using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    float radius;
    public float speed;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        radius = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float angleRadiant = ((Mathf.Cos(Time.time)*speed/2f)+0.5f)*Mathf.PI;
        this.transform.position=new Vector3(Mathf.Cos(angleRadiant),Mathf.Sin(angleRadiant),0f)*radius;
        this.transform.rotation = Quaternion.LookRotation(target.transform.position - this.transform.position, new Vector3(0,0,1));
    }
}
