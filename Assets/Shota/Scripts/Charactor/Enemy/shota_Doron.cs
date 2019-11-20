using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shota_Doron : MonoBehaviour {

    [SerializeField] shota_HeadQuarters HQ;


    GameObject hit_target;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit_info = new RaycastHit();
            float max_distance = 100f;

            bool is_hit = Physics.Raycast(ray, out hit_info, max_distance);

            if (is_hit)
            {
                if (hit_info.collider.gameObject.tag == "Node")
                {
                    HQ.Alart(hit_info.collider.gameObject.GetComponent<shota_NodePoint>());
                }
            }
        }
    }
}
