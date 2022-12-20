using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] GameObject handle;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        RectTransform rtHandle;

        float xMax = rt.rect.width * 0.97f;
        float x = rt.rect.width * 0.025f;
        float y = rt.rect.height * 0.2f;
        Vector2 size = new Vector2(rt.rect.width * 0.01f, rt.rect.height * 0.6f);
        float step = 2f;

        while (x <= xMax)
        {
            rtHandle = Instantiate(handle, transform).GetComponent<RectTransform>();
            rtHandle.sizeDelta = size;
            rtHandle.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, size.x);
            rtHandle.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,  y, size.y);
            x += size.x + step;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
