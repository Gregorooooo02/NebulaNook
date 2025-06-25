using UnityEngine;

public class TableHandle : MonoBehaviour
{
    public static TableHandle Instance;
    private Animator animator;
    public TableGlitch activator;
    private Rigidbody rb;

    public Vector3 yeetDirection;

    void Start()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void TriggerAnimation(TableGlitch activator)
    {
        this.activator = activator;
        StartAnim();
        activator.TableAudioSource.PlayOneShot(activator.audioClips[0]);
    }

    public void TriggerYeet()
    {
        activator.TriggerYeet(yeetDirection);
        activator.TableAudioSource.PlayOneShot(activator.audioClips[1]);
    }

    private void StartAnim()
    {
        rb.isKinematic = true;
        animator.SetBool("Yeet", true);
        animator.SetBool("Back", false);
    }

    public void Restore()
    {
        rb.isKinematic = false;
        activator = null;
        animator.SetBool("Yeet", false);
        animator.SetBool("Back", true);
    }

    public void TriggerBoom()
    {
        GetComponentInChildren<Boom>().Explode();
    }
}
