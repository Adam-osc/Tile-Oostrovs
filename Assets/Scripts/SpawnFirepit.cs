using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnFirepit : MonoBehaviour
{
    private bool placeClick = false;
    [SerializeField] private GameObject FirePit;
    public TileAutoamta secondScript;
    public Toggle toggle1;
    private int count;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && placeClick == true && count < 10)
        {
            Vector2 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);


            mousePos.x = (float)Mathf.Floor(mousePos.x) + (float)0.5;
            mousePos.y = (float)Mathf.Floor(mousePos.y) + (float)0.5;
            
            //mousePos.z = Mathf.Round(mousePos.z);

            Instantiate(FirePit, mousePos, transform.rotation);
            count++;
        }
    }

    public void click()
    {
        placeClick = true;
        if (toggle1.isOn)
        {
            toggle1.isOn = !toggle1.isOn;
        }
        else
        {
            toggle1.isOn = toggle1.isOn;
        }
    }
}
