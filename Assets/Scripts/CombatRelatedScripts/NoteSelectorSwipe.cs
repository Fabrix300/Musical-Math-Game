using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteSelectorSwipe : MonoBehaviour
{
    public Scrollbar scrollBar;
    public ScrollRect scrollRect;
    public Toggle toggle;

    private float scrollPos = 0;
    private float[] pos;
    private float distance;
    private int selected;

    // Start is called before the first frame update
    void Start()
    {
        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollBar.value;
        } 
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
                {
                    scrollBar.value = Mathf.Lerp(scrollBar.value, pos[i], 0.1f);
                    selected = i;
                    //Debug.Log(selected + " " + transform.GetChild((transform.childCount - 1) - selected).name);
                    transform.GetChild(transform.childCount - 1 - selected).localScale = Vector2.Lerp(
                    transform.GetChild(transform.childCount - 1 - selected).localScale,
                    new Vector2(1f, 1f),
                    0.1f
                    );
                }
                if (i != selected)
                {
                    transform.GetChild(transform.childCount - 1 - i).localScale = Vector2.Lerp(
                            transform.GetChild(transform.childCount - 1 - i).localScale,
                            new Vector2(0.8f, 0.8f),
                            0.1f
                            );
                }
            }
        }

        /*for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(
                    transform.GetChild(i).localScale,
                    new Vector2(1f, 1f),
                    0.1f
                    );
            }
            else
            {
                transform.GetChild(i).localScale = Vector2.Lerp(
                            transform.GetChild(i).localScale,
                            new Vector2(0.85f, 0.85f),
                            0.1f
                            );
            }
        }*/
    }

    public int GetSelected()
    {
        return selected;
    }

    public void DisableScroll(bool value)
    {
        scrollRect.enabled = !value;
        scrollBar.interactable = !value;
    }

    public void EnableScroll()
    {
        scrollRect.enabled = true;
        scrollBar.interactable = true;
        toggle.isOn = false;
    }
}
