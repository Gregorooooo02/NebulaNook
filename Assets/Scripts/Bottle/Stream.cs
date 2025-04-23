using System.Collections;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ParticleSystem splashParticle;

    private Coroutine pourRoutine;
    private Vector3 targetPosition = Vector3.zero;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        MoveToTarget(0, transform.position);
        MoveToTarget(1, transform.position);
    }

    public void BeginStream() {
        StartCoroutine(UpdateParticleCoroutine());
        pourRoutine = StartCoroutine(PourCoroutine());
    }

    private IEnumerator PourCoroutine() {
        while (gameObject.activeSelf) {
            targetPosition = FindEndPoint();

            MoveToTarget(0, transform.position);
            AnimateToTarget(1, targetPosition);

            yield return null;
        }
    }

    public void EndStream() {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPourCoroutine());
    }

    private IEnumerator EndPourCoroutine() {
        while (!HasReachedTarget(0, targetPosition)) {
            AnimateToTarget(0, targetPosition);
            AnimateToTarget(1, targetPosition);

            yield return null;
        }

        Destroy(gameObject);
    }

    private Vector3 FindEndPoint() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, Mathf.Infinity);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2f);

        return endPoint;
    }

    private void MoveToTarget(int index, Vector3 targetPosition) {
        lineRenderer.SetPosition(index, targetPosition);
    }

    private void AnimateToTarget(int index, Vector3 targetPosition) {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPoint = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 3.5f);

        lineRenderer.SetPosition(index, newPoint);
    }

    private bool HasReachedTarget(int index, Vector3 targetPosition) {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        
        return currentPoint == targetPosition;
    }

    private IEnumerator UpdateParticleCoroutine() {
        while (gameObject.activeSelf) {
            splashParticle.gameObject.transform.position = targetPosition + Vector3.up * 0.001f;
            // Lock rotation on the X and Z axes
            splashParticle.gameObject.transform.rotation = Quaternion.Euler(-90, transform.rotation.y, 0);


            bool isHitting = HasReachedTarget(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);

            yield return null;
        }
    }
}
