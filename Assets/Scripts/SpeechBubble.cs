using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshPro textMeshPro;
    public RectTransform Background;

    public float CharDelay = 0.05f;
    private float _currentCharTime = 0;
    private int characterIndex = 0;

    private string _currentText;
    private bool _startWriting = false;

    private Vector2 TextPadding = new Vector2(50.0f, 25.0f);

    public GameObject textObject;
    public GameObject iconObject;

    private Action NotifyTextEnded;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

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
                Background.sizeDelta = textSize + TextPadding;
                if (characterIndex == _currentText.Length - 1)
                {
                    _startWriting = false;
                    characterIndex = 0;
                    NotifyTextEnded?.Invoke();
                }
                if(characterIndex % 3 == 0) audioSource.Play();
            }
        } 
    }

    public void SetText(string text)
    {
        _currentText = text + " ";
        textObject.SetActive(true);
        _startWriting = true; 
    }

    public void SetNotifyAction(Action action)
    {
        NotifyTextEnded = action;
    }

    public void SetIcon(Texture2D icon)
    {
        iconObject.SetActive(true);
        Image image = iconObject.GetComponent<Image>();
        image.sprite = Sprite.Create(icon,new Rect(0,0,icon.width,icon.height), new Vector2(0, 0));
        
        RectTransform rectTransform = iconObject.GetComponent<RectTransform>();
        Background.sizeDelta = rectTransform.sizeDelta + TextPadding;
    }
}
