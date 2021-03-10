using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float panSpeed = 20f;
    public Vector2 panLimit;

    public TileAutoamta secondScript;

    private Camera cam;
    private float targetZoom;
    [SerializeField]private float zoomFactor = 3f;
    [SerializeField] private float zoomLerpSpeed = 10;


    void Start()
    {
        panLimit.x = secondScript.tmapSize.x / 2;
        panLimit.y = secondScript.tmapSize.y / 2;
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        

        if (Input.GetKey("w"))
        {
            pos.y += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom = targetZoom - scrollData * zoomFactor;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
