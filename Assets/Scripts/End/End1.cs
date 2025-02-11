using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollingText : MonoBehaviour
{
    public Text text; 
    public float scrollSpeed = 50f; 
    public float endYPosition = 1000f; 

    private RectTransform textRectTransform;
    private Vector2 startPosition; 

    private void Start()
    {
        
        textRectTransform = text.GetComponent<RectTransform>();

        
        startPosition = textRectTransform.anchoredPosition;
    }

    private void Update()
    {
      
        textRectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

       
        if (textRectTransform.anchoredPosition.y >= endYPosition)
        {
            Application.Quit();
        }
    }
}
