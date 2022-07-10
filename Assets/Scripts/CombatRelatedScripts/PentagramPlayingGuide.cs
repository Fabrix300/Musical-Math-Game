using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagramPlayingGuide : MonoBehaviour
{
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public void MoveToPos(Vector2 position, float timeToMove)
    {
        StartCoroutine(MoveToPosition(position, timeToMove));
    }

    public IEnumerator MoveToPosition(Vector2 position, float timeToMove)
    {
        Vector2 currentPos = rectTransform.anchoredPosition;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            //transform.position = Vector3.Lerp(currentPos, position, t);
            rectTransform.anchoredPosition = Vector2.Lerp(currentPos, position, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        SetActiveGameObject(false);
        rectTransform.anchoredPosition = new Vector2(-480, 0);
    }

    public void SetActiveGameObject(bool value)
    {
        gameObject.SetActive(value);
    }
}
