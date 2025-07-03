using UnityEngine;

public class Main : MonoBehaviour
{
    private RectTransform _rectTransform;

    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f); // ’†‰›‚ÉŒÅ’è
        rt.pivot = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        
    }
}
