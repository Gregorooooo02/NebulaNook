using UnityEngine;

public class TutorialGlass : MonoBehaviour
{
    private GlassFiller filler;
    void Start()
    {
        filler = GetComponent<GlassFiller>();
    }

    private void FixedUpdate()
    {
        if(filler.currentFillAmount < 0.1f)
        {
            TutorialManager.Instance?.NotifyGlassSpilled();
        }
    }

    private void OnDestroy()
    {
        TutorialManager.Instance?.NotifyGlassDestroyed();
    }

}
