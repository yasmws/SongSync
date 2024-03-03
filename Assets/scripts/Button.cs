using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    private Button button;
    private Vector3 originalSize;

    void Start()
    {
        button = GetComponent<Button>();
        originalSize = transform.localScale;
    }

    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), Input.mousePosition))
        {
            transform.localScale = originalSize * 1.1f;
        }
        else
        {
            transform.localScale = originalSize;
        }
    }
}
