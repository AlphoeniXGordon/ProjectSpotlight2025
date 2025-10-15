using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(startPos, startPos + new Vector3(9.0f,0.0f,0.0f), Mathf.Sin(Time.time) * 0.5f + 0.5f);
    }
}
