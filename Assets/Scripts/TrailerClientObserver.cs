using System.Collections;
using UnityEngine;

public class TrailerClientObserver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ClientSpawner clientSpawner;
    [SerializeField] private ClientController staticClient;
    [SerializeField] private Transform flyAwayDirection;
    [SerializeField] private Transform playerPosition;

    [Header("Timing")]
    [SerializeField] private float lookAtFlyAwayDuration = 2f;
    [SerializeField] private float turnToPlayerDuration = 3f;
    [SerializeField] private float lookAtPlayerDuration = 5f;
    [SerializeField] private float rotationSpeed = 90f;

    private ClientController spawnedClient;
    private bool hasObservedDrinking = false;
    private DrinkEffect observedEffect = DrinkEffect.EMPTY;

    private void Start()
    {
        StartCoroutine(ObserveSpawnedClient());
    }

    private IEnumerator ObserveSpawnedClient()
    {
        yield return new WaitUntil(() => GetSpawnedClient() != null);
        spawnedClient = GetSpawnedClient();
        observedEffect = spawnedClient.DesiredDrinkEffect;

        yield return new WaitUntil(() => spawnedClient.IsWaiting);
        yield return new WaitUntil(() => !spawnedClient.IsWaiting);

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(StaticClientReactionSequence());
    }

    private ClientController GetSpawnedClient()
    {
        if (clientSpawner == null) return null;

        for (int i = 0; i < clientSpawner.transform.childCount; i++)
        {
            Transform child = clientSpawner.transform.GetChild(i);
            ClientController client = child.GetComponent<ClientController>();
            if (client != null)
            {
                return client;
            }
        }

        return null;
    }

    private IEnumerator StaticClientReactionSequence()
    {
        if (staticClient == null) yield break;

        if (flyAwayDirection != null)
        {
            yield return StartCoroutine(SmoothLookAt(staticClient.transform, flyAwayDirection.position, lookAtFlyAwayDuration));

            yield return new WaitForSeconds(lookAtFlyAwayDuration);
        }

        if (playerPosition != null)
        {
            yield return StartCoroutine(SmoothLookAt(staticClient.transform, playerPosition.position, turnToPlayerDuration));
        }

        yield return new WaitForSeconds(lookAtPlayerDuration);

        ShowSpeechBubble();
    }

    private IEnumerator SmoothLookAt(Transform client, Vector3 targetPosition, float duration)
    {
        Vector3 direction = (targetPosition - client.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion startRotation = client.rotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            t = Mathf.SmoothStep(0f, 1f, t);
            client.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        client.rotation = targetRotation;
    }

    private void ShowSpeechBubble()
    {
        if (staticClient.bubble != null)
        {
            staticClient.DesiredDrinkEffect = observedEffect;
            staticClient.IsWaiting = true;

            staticClient.bubble.gameObject.SetActive(true);

            if (staticClient.useIcons)
            {
                staticClient.bubble.SetIcon(DrinkEffectMap.Instance.effectIcons[(int)observedEffect]);
            }
        }
    }

    [ContextMenu("Test static client reaction")]
    private void TestReaction()
    {
        if (Application.isPlaying)
        {
            observedEffect = DrinkEffect.EXPLOSION;
            StartCoroutine(StaticClientReactionSequence());
        }
    }
}
