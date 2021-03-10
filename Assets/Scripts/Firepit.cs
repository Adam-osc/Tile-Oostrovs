using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firepit : MonoBehaviour
{
    public Animator animator;
    private bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Clicked", clicked);

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    PrintName(hit.transform.gameObject);

                    if (clicked == false)
                    {
                        clicked = true;
                        
                    }
                    else if (clicked == true)
                    {
                        clicked = false;
                    }
                }
            }

        }

        
    }
    private void PrintName(GameObject go)
    {
        print(go.name);
    }

}
