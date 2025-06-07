using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ParticleSystem[] splashParticles;
    [SerializeField] private Transform laserTrigger;
    [SerializeField] private LayerMask ignoreLayer;

    private Coroutine laserRoutine;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 lastHitNormal = Vector3.up;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticles = GetComponentsInChildren<ParticleSystem>();
    }

    private void Start()
    {
        MoveToTarget(0, transform.position);
        MoveToTarget(1, transform.position);
    }

    public void BeginLaser()
    {
        StartCoroutine(UpdateTriggerCoroutine());
        StartCoroutine(UpdateParticleCoroutine());
        laserRoutine = StartCoroutine(LaserCoroutine());
    }

    private IEnumerator LaserCoroutine()
    {
        while (gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToTarget(0, transform.position);
            AnimateToTarget(1, targetPosition);

            yield return null;
        }
    }

    public void EndLaser()
    {
        StopCoroutine(laserRoutine);
        laserRoutine = StartCoroutine(EndLaserCoroutine());
    }

    private IEnumerator EndLaserCoroutine()
    {
        while (!HasReachedTarget(0, targetPosition))
        {
            AnimateToTarget(0, targetPosition);
            AnimateToTarget(1, targetPosition);

            yield return null;
        }

        Destroy(gameObject);
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ignoreLayer))
        {
            lastHitNormal = hit.normal;
            return hit.point;
        }
        else
        {
            lastHitNormal = transform.forward;
            return ray.GetPoint(50f);
        }
    }

    private void MoveToTarget(int index, Vector3 position)
    {
        lineRenderer.SetPosition(index, position);
    }

    private void AnimateToTarget(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPoint = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 10f);
        lineRenderer.SetPosition(index, newPoint);
    }

    private bool HasReachedTarget(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        return currentPoint == targetPosition;
    }

    private IEnumerator UpdateParticleCoroutine()
    {
        while (gameObject.activeSelf)
        {
            foreach (var particle in splashParticles)
            {
                particle.gameObject.transform.position = targetPosition + Vector3.up * 0.001f;
                particle.gameObject.transform.rotation = Quaternion.LookRotation(lastHitNormal);

                bool isHitting = HasReachedTarget(1, targetPosition);
                particle.gameObject.SetActive(isHitting);
            }

            yield return null;
        }
    }

    private IEnumerator UpdateTriggerCoroutine()
    {
        while (gameObject.activeSelf)
        {
            laserTrigger.position = targetPosition;
            yield return null;
        }
    }
}
