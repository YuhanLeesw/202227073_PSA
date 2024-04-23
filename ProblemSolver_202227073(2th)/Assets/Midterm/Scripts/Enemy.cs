using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 xRange = new Vector2(-10, 13);
    public Vector2 zRange = new Vector2(-12, 13);
    public float speed = 5.0f;
    public Transform player;

    private Vector3 nextPosition;
    private Camera enemyCamera;
    private bool isChasing = false;
    private bool isSearching = false;
    private float searchDuration = 6.0f; // 검색 시간
    private float searchTimer = 0;

    void Start()
    {
        nextPosition = GetRandomPositionInRange();
        enemyCamera = gameObject.AddComponent<Camera>();
        enemyCamera.enabled = false;
        enemyCamera.fieldOfView = 45;
        enemyCamera.nearClipPlane = 0.1f;
        enemyCamera.farClipPlane = GetCapsuleCameraDistance();
        enemyCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (!isSearching)
        {
            Patrol();
            CheckForPlayer();
        }
        else // 검색 중이면
        {
            Search();
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
        {
            nextPosition = GetRandomPositionInRange();
        }
        MoveTowards(nextPosition);
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= enemyCamera.farClipPlane)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit))
            {
                if (hit.transform == player)
                {
                    isChasing = true;
                    enemyCamera.backgroundColor = Color.red; // Indicate chasing
                }
            }
        }
    }

    void ChasePlayer()
    {
        MoveTowards(player.position);
        if (Vector3.Distance(transform.position, player.position) > enemyCamera.farClipPlane)
        {
            isChasing = false;
            StartSearch();
        }
    }

    void StartSearch()
    {
        isSearching = true;
        searchTimer = 0;
        StartCoroutine(PerformSearch());
    }

    void Search()
    {
        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            isSearching = false;
            nextPosition = GetRandomPositionInRange();
            enemyCamera.transform.localRotation = Quaternion.Euler(0, 0, 0); // 검색이 끝난 후 카메라 회전 설정
        }
    }

    IEnumerator PerformSearch()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion leftRotation = startRotation * Quaternion.Euler(0, 90, 0);
        Quaternion rightRotation = leftRotation * Quaternion.Euler(0, 180, 0);

        float duration = 3.0f; // 각 회전에 걸리는 시간

        // 좌측으로 90도 회전
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, leftRotation, t / duration);
            yield return null;
        }

        // 우측으로 180도 회전
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(leftRotation, rightRotation, t / duration);
            yield return null;
        }

        isSearching = false;
        nextPosition = GetRandomPositionInRange();
    }

    void MoveTowards(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        // 카메라가 에너미의 방향을 향하도록 설정
        enemyCamera.transform.rotation = Quaternion.LookRotation(direction);
    }

    float GetCapsuleCameraDistance()
    {
        float capsuleHeight = 2.0f; // Assuming a typical capsule size
        return 3 * Mathf.Sqrt(2 * (capsuleHeight / 2) * (capsuleHeight / 2));
    }

    private Vector3 GetRandomPositionInRange()
    {
        float randomX = Random.Range(xRange.x, xRange.y);
        float randomZ = Random.Range(zRange.x, zRange.y);
        return new Vector3(randomX, transform.position.y, randomZ);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow box representing the patrol area in the editor
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((xRange.x + xRange.y) / 2, transform.position.y, (zRange.x + zRange.y) / 2);
        Vector3 size = new Vector3(xRange.y - xRange.x, 1, zRange.y - zRange.x);
        Gizmos.DrawWireCube(center, size);
    }
}
