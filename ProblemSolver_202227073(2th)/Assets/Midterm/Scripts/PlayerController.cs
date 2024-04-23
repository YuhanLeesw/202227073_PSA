using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTarget; // 바라볼 대상 오브젝트
    public float movementSpeed = 5f; // 이동 속도
    public float rotationDuration = 1f; // 회전 지속 시간
    private Vector3[] cameraRotations; // 카메라 회전 위치들
    private int rotationIndex = 0; // 현재 카메라 회전 인덱스
    private bool isRotating = false; // 카메라 회전 중인지 여부

    void Start()
    {
        // 카메라 회전 위치 초기화
        cameraRotations = new Vector3[]
        {
            new Vector3(45f, 45f, 0f),
            new Vector3(45f, -45f, 0f),
            new Vector3(45f, -135f, 0f),
            new Vector3(45f, -225f, 0f)
        };

        // 초기 카메라 회전 위치 설정
        UpdateCameraPosition(rotationIndex);
    }

    void Update()
    {
        // 이동 입력 받기
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 카메라의 전방 벡터를 기준으로 이동 방향 설정
        Vector3 forwardDirection = mainCamera.transform.forward;
        forwardDirection.y = 0f; // 상하 이동 제거
        Vector3 movementDirection = forwardDirection.normalized * verticalInput + mainCamera.transform.right * horizontalInput;

        // 이동
        if (movementDirection != Vector3.zero)
        {
            transform.position += movementDirection.normalized * movementSpeed * Time.deltaTime;

            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // O 키 또는 P 키를 눌렀을 때 카메라 회전
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isRotating) // 카메라가 회전 중이 아닌 경우에만 회전 수행
            {
                int newIndex = rotationIndex;
                if (Input.GetKeyDown(KeyCode.P))
                    newIndex = (rotationIndex + 1) % cameraRotations.Length; // 다음 위치로 이동
                else
                    newIndex = (rotationIndex - 1 + cameraRotations.Length) % cameraRotations.Length; // 이전 위치로 이동

                // 카메라 회전 위치 변경
                UpdateCameraPosition(newIndex);
            }
        }
    }

    // 카메라의 회전 위치를 변경하는 함수
    void UpdateCameraPosition(int index)
    {
        isRotating = true; // 카메라 회전 중 상태로 변경

        // 이전 위치에서 현재 위치로 회전
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(cameraRotations[index]);

        // 회전 애니메이션 코루틴 시작
        StartCoroutine(RotateCamera(startRotation, endRotation));

        // 현재 카메라 회전 인덱스 갱신
        rotationIndex = index;
    }

    // 카메라의 회전 애니메이션을 처리하는 코루틴
    IEnumerator RotateCamera(Quaternion startRotation, Quaternion endRotation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            // 회전
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전 애니메이션 종료 후 회전 상태 해제
        isRotating = false;
    }
}
