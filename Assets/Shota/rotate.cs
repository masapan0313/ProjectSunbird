using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    public float speed;

    public float tateyure;

    public float tatespeed;

    public float max, min;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, speed* Time.deltaTime, 0);


        tateyure += tatespeed * Time.deltaTime;

        if (tateyure < min)
        {
            tatespeed *= -1;
        }
        if(tateyure > max)
        {
            tatespeed *= -1;
        }
	}
}
