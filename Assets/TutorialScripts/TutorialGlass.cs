using UnityEngine;

public class TutorialGlass : MonoBehaviour
{
    private GlassController controller;
    void Start()
    {
        controller = GetComponent<GlassController>();
    }

    private void FixedUpdate()
    {
        if(controller.currentFillAmount < 0.1f)
        {
            TutorialManager.Instance?.NotifyGlassSpilled();
        }
    }

    private void OnDestroy()
    {
        TutorialManager.Instance?.NotifyGlassDestroyed();
    }

}
