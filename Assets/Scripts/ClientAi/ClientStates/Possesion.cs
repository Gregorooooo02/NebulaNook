using System.Collections;
using UnityEngine;

public class Possesion : ClientState
{
    private bool triggered = false;
    [Header("General")]
    public GameObject particles;
    public Rigidbody rootBoneRB;
    public GameObject Parent;

    public Transform zAxisReference;
    public Transform xAxisReference;

    private float startX;
    private float startY;

    [Header("Animation parameters: 1st stage")]
    public float TimeToPosses;

    [Header("Animation parameters: 2nd stage")]
    public float InitialFloatingForce;
    public float maxInitialVerticalOffset;


    [Header("Animation parameters: 3rd stage")]
    public float initialSuspensionTime3rdStage;
    public float sidewaysJerkForce;
    public float max1stJerkOffset;
    public float max2ndJerkOffset;
    public float firstJerkCooldown;
    public float secondJerkCooldown;
    public float returnForce;
    public float returnCooldown;


    [Header("Animation parameters: 4th stage")]
    public int jerksAmount;
    public float maxJerksOffset;
    public float jerkForce;
    public float jerkCooldown;


    [Header("Animation parameters: 5th stage")]
    public float backwardsForce;
    public float targetVelocity;
    public float destroyTime;

    private Vector3 forward;
    private Vector3 left;

    public override ClientState RunState()
    {
        if (!triggered)
        {
            ClientController controller = GetComponentInParent<ClientController>();
            controller.ToggleRagdoll(true);
            triggered = true;   
            startY = rootBoneRB.position.y;
            forward = zAxisReference.position - transform.position;
            left = xAxisReference.position - transform.position;
            rootBoneRB.useGravity = false;
            StartCoroutine("StartAnimating");
        }
        return this;
    }

    IEnumerator StartAnimating()
    {
        yield return new WaitForSeconds(TimeToPosses);
        particles.SetActive(true);
        rootBoneRB.constraints = RigidbodyConstraints.FreezeRotationY;
        while(rootBoneRB.position.y - startY < maxInitialVerticalOffset)
        {
            rootBoneRB.AddForce(Vector3.up * InitialFloatingForce, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        rootBoneRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY;
        yield return new WaitForSeconds(initialSuspensionTime3rdStage);
        rootBoneRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        startX = rootBoneRB.position.x;
        while (rootBoneRB.position.x - startX < max1stJerkOffset)
        {
            rootBoneRB.AddForce(-left * sidewaysJerkForce, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        rootBoneRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY;
        yield return new WaitForSeconds(firstJerkCooldown);
        rootBoneRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        while (startX - rootBoneRB.position.x < max2ndJerkOffset)
        {
            rootBoneRB.AddForce(left * sidewaysJerkForce, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        rootBoneRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY;
        yield return new WaitForSeconds(secondJerkCooldown);
        rootBoneRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        while (startX - rootBoneRB.position.x > 0)
        {
            rootBoneRB.AddForce(-left * returnForce, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        rootBoneRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY;
        yield return new WaitForSeconds(returnCooldown);
        rootBoneRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        float currentY = rootBoneRB.position.y; 
        for (int i = 0;i < jerksAmount; i++)
        {
            rootBoneRB.constraints = RigidbodyConstraints.FreezeRotationY;
            if(i%2 == 0)
            {
                while (rootBoneRB.position.y - currentY < maxJerksOffset)
                {
                    rootBoneRB.AddForce(Vector3.up * jerkForce, ForceMode.Acceleration);
                    yield return new WaitForFixedUpdate();
                }
            } 
            else
            {
                while (currentY - rootBoneRB.position.y < maxJerksOffset)
                {
                    rootBoneRB.AddForce(Vector3.down * (jerkForce / 2.0f), ForceMode.Acceleration);
                    yield return new WaitForFixedUpdate();
                }
            }
            rootBoneRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY;
            yield return new WaitForSeconds(jerkCooldown);
        }
        rootBoneRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        while (rootBoneRB.linearVelocity.magnitude < targetVelocity)
        {
            rootBoneRB.AddForce(-forward * backwardsForce, ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(destroyTime);
        Destroy(Parent);
    }

}
