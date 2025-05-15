using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshPro textMeshPro;
    public RectTransform Image;

    public float CharDelay = 0.05f;
    private float _currentCharTime = 0;
    private int characterIndex = 0;

    private string _currentText;
    private bool _startWriting = false;

    private Vector2 TextPadding = new Vector2(25.0f, 25.0f);

    // Update is called once per frame
    void Update()
    {
        if (_startWriting)
        {
            _currentCharTime += Time.deltaTime;
            if(_currentCharTime > CharDelay)
            {
                _currentCharTime = 0;
                characterIndex++;
                textMeshPro.text = _currentText.Substring(0, characterIndex);
                textMeshPro.ForceMeshUpdate();
                Vector2 textSize = textMeshPro.GetRenderedValues(false);
                Image.sizeDelta = textSize + TextPadding;
                if (characterIndex == _currentText.Length - 1)
                {
                    _startWriting = false;
                    characterIndex = 0;
                }
            }
        } 
    }

    public void SetText(string text)
    {
        _currentText = text;
        _startWriting = true;
    }
}
