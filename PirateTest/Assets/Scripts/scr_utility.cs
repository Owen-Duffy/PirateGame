using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class scr_utility : MonoBehaviour
{
    public bool ScrolledUp = false;
    public bool ScrolledDown = false;
    public float LastScroll = 0.0f;
    public float NewScroll = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        ScrolledUp = false;
        ScrolledDown = false;
        NewScroll = 0f;
        LastScroll = Input.GetAxis("Mouse ScrollWheel");
    }

    // Update is called once per frame
    void Update()
    {
        ScrolledUp = false;
        ScrolledDown = false;

        NewScroll = Input.GetAxis("Mouse ScrollWheel");

        if (NewScroll > LastScroll)
        {
            ScrolledDown = true;
            ScrolledUp = false;
        }
        else if (NewScroll < LastScroll)
        {
            ScrolledUp = true;
            ScrolledDown = false;
        }

        LastScroll = NewScroll;

        /*if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            ScrolledUp = true;
            ScrolledDown = false;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            ScrolledDown = true;
            ScrolledUp = false;
        }*/
    }

    public void ChangeOutline(GameObject target, Color color, float width)
    {
        var outline = target.GetComponent<Outline>();
        if (outline == null)
        {
            outline = target.AddComponent<Outline>();
        }

        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = color;
        outline.OutlineWidth = width;
    }

    public void RemoveOutline(GameObject target)
    {
        if (target == null)
            return;

        var outline = target.GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
        }
    }
}
