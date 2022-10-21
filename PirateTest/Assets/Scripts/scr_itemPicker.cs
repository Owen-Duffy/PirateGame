using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class scr_itemPicker : MonoBehaviour
{
    [SerializeField] public GameObject shape;

    [SerializeField] public AudioClip SND_SPAWN;


    scr_itemHandler handler;

    private GameObject recentlySelected;
    private GameObject currentlySelected;

    public scr_utility Utility;

    bool Holding = false;

    // Start is called before the first frame update
    void Start()
    {
        Utility = Camera.main.GetComponent<scr_utility>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageSpawns();
        Holding = Input.GetKey(KeyCode.Mouse0);

        if (recentlySelected != null)
        {
            handler.ManageGrab(Holding, GetGrabPoint());
            handler.ManageHover();
        }
        
        if (!Holding)
        {
            recentlySelected = GetCurrentSelection();
        }
    }

    Vector3 GetGrabPoint()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 12f))
        {
            return hit.point;
        }

        return new Vector3(0, 0, 0);
    }

    void ManageSpawns()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(shape, hit.point, Quaternion.identity);
                AudioSource.PlayClipAtPoint(SND_SPAWN, hit.point);
            }
        }
    }

    GameObject GetCurrentSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 12f))
        {
            var selection = hit.transform.gameObject;

            if (!selection.CompareTag("selectable"))
                return null;

            handler = selection.GetComponent<scr_itemHandler>();

            if (Input.GetKey(KeyCode.V))
            {
                handler.DestroyItem();
                return null;
            }

            return selection;
        }

        return null;
    }
}