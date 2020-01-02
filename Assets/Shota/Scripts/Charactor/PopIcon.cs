using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopIcon : MonoBehaviour {

    bool isSite = false;

    [SerializeField]
    float MaxScale_X, MaxScale_Y, MaxScale_Z;

    [SerializeField]
    float Scaling_Speed_x, Scaling_Speed_y, Scaling_Speed_z;

    Vector3 scaling_speed;

    [SerializeField]
    List<GameObject> ChildList;

    private void Start()
    {
        scaling_speed = new Vector3(Scaling_Speed_x, Scaling_Speed_y, Scaling_Speed_z);
    }

    private void Update()
    {
        if (isSite)
        {
            if(transform.localScale.x < MaxScale_X)
            {
                scaling_speed.x = Scaling_Speed_x;
            }
            else
            {
                scaling_speed.x = 0;
            }
            if (transform.localScale.y < MaxScale_Y)
            {
                scaling_speed.y = Scaling_Speed_y;
            }
            else
            {
                scaling_speed.y = 0;
            }
            if (transform.localScale.z < MaxScale_Z)
            {
                scaling_speed.z = Scaling_Speed_z;
            }
            else
            {
                scaling_speed.z = 0;
            }
            transform.localScale = 
                new Vector3(transform.localScale.x + scaling_speed.x * Time.deltaTime,
                transform.localScale.y + scaling_speed.y * Time.deltaTime,
                transform.localScale.z + scaling_speed.z * Time.deltaTime);
        }
        else
        {
            if (transform.localScale.x <= 0)
            {
                scaling_speed.x = 0;
            }
            else
            {
                scaling_speed.x = Scaling_Speed_x;
            }
            if (transform.localScale.y <= 0)
            {
                scaling_speed.y = 0;
            }
            else
            {
                scaling_speed.y = Scaling_Speed_y;
            }
            if (transform.localScale.z <= 0)
            {
                scaling_speed.z = 0;
            }
            else
            {
                scaling_speed.z = Scaling_Speed_z;
            }

            if (transform.localScale.x <= 0 && transform.localScale.y <= 0 && transform.localScale.z <= 0)
            {
                foreach (GameObject a in ChildList)
                {
                    a.SetActive(false);
                }
            }
            transform.localScale =
                new Vector3(transform.localScale.x - scaling_speed.x * Time.deltaTime,
                transform.localScale.y - scaling_speed.y * Time.deltaTime,
                transform.localScale.z - scaling_speed.z * Time.deltaTime);
        }
        

        
    }


    public void Pop()
    {
        foreach(GameObject a in ChildList)
        {
            a.SetActive(true);
        }

        isSite = true;
    }
    public void nonPop()
    {
        isSite = false;
    }
}
