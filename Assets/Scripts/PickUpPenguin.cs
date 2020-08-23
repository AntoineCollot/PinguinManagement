using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPenguin : MonoBehaviour
{
    Transform heldPenguin;
    Vector3 heldPenguinVelocity;
    Camera cam;

    [SerializeField] LayerMask penguinLayer = 0;
    [SerializeField] LayerMask groundLayers = 0;
    [SerializeField] Transform cursor = null;
    [SerializeField] float penguinHeldAltitude = 1;
    [SerializeField] float penguinHeldSmooth = 0.1f;
    [SerializeField] float penguinSphereCastRadius = 0.2f;

    Vector3 refPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

        //Penguin raycast
        if (Physics.SphereCast(camRay, penguinSphereCastRadius, out hit,Mathf.Infinity, penguinLayer))
        {
            //Pick up the penguin
            if (Input.GetMouseButtonDown(0))
            {
                heldPenguin = hit.transform;
                heldPenguin.SendMessage("OnPickUp");
            }
        }

        //Ice raycast
        if (Physics.Raycast(camRay, out hit, Mathf.Infinity, groundLayers))
        {
            //Place the cursor
            cursor.position = hit.point;
        }

        //Release the penguin
        if(Input.GetMouseButtonUp(0))
        {
            if (heldPenguin != null)
            {
                heldPenguin.SendMessage("OnRelease");
                heldPenguin.GetComponent<Rigidbody>().velocity = heldPenguinVelocity * 0.25f;
                heldPenguin = null;
                refPosition = Vector3.zero;
            }
        }

        //Move the held penguin if any
        if (heldPenguin != null)
        {
            Vector3 lastPos = heldPenguin.position;
            heldPenguin.position = Vector3.SmoothDamp(heldPenguin.position, cursor.position + Vector3.up * penguinHeldAltitude, ref refPosition, penguinHeldSmooth);

            heldPenguinVelocity = (heldPenguin.position - lastPos) / Time.smoothDeltaTime;
        }
    }

    bool IsPartOfLayerMask(int layer, LayerMask mask)
    {
        return (mask & (1 << layer)) != 0;
    }
}
