using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class scr_itemHandler : MonoBehaviour
{
    [SerializeField] public AudioClip SND_REMOVE;

    public bool grabbed = false;
    public bool hovered = false;

    private float camDist;
    public float currentDist;

    public scr_utility Utility;

    public Rigidbody RB;
    public GameObject CarrierPrefab;
    GameObject child;

    Vector3 currentPos;
    Vector3 prevPos;
   

    // Start is called before the first frame update
    void Start()
    {
        camDist = 2.0f;
        currentDist = camDist;
        Utility = Camera.main.GetComponent<scr_utility>();
        RB = GetComponent<Rigidbody>();
    }

    public void ManageGrab(bool held, Vector3 point)
    {
        if (held && !grabbed)
        {
            GrabItem(point);
        }
        else if (grabbed && !held)
        {
            DropItem();
        }

        grabbed = held;

        if (grabbed)
        {
            MoveItem();
        }
    }

    public void ManageHover()
    {
        if (grabbed)
        {
            hovered = false;
        }
        else
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 12f))
            {
                hovered = hit.transform.gameObject == gameObject;
            }
            else
            {
                hovered = false;
            }
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        bool DoOutline = false;
        Color outline = new Color(0, 0, 0);

        if (grabbed)
        {
            outline = Color.cyan;
            DoOutline = true;
        }
        else if (hovered)
        {
            outline = Color.blue;
            DoOutline = true;
        }

        if (DoOutline)
        {
            Utility.ChangeOutline(gameObject, outline, 5.0f);
        }
        else
        {
            Utility.RemoveOutline(gameObject);
        }
    }

    public void GrabItem(Vector3 SpawnLoc)
    {
        RB.useGravity = false;
        Debug.Log("Grabbed object.");
    }

    public void DropItem()
    {
        RB.useGravity = true;
        Debug.Log("Dropped object.");
    }

    public void MoveItem()
    {
       if (Input.GetAxis("Mouse ScrollWheel") > 0f)
       {
           currentDist += 0.5f;
           if (currentDist > 12.0f)
           {
               currentDist = 12.0f;
           }

       }

       if (Input.GetAxis("Mouse ScrollWheel") < 0f)
       {
           currentDist -= 0.5f;
           if (currentDist < 2.0f)
           {
               currentDist = 2.0f;
           }
       }

       Vector3 startPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, currentDist);
       currentPos = Camera.main.ScreenToWorldPoint(startPos);

       float dist = Vector3.Distance(currentPos, transform.position);

       RB.velocity = transform.forward * dist * 5f;
       var rotation = Quaternion.LookRotation(currentPos - transform.position);
       RB.MoveRotation(Quaternion.RotateTowards(RB.transform.rotation, rotation, 360f));

       RB.transform.LookAt(currentPos, Vector3.up);
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(SND_REMOVE, transform.position);
    }
}
