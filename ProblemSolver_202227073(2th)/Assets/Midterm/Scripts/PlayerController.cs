using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTarget; // �ٶ� ��� ������Ʈ
    public float movementSpeed = 5f; // �̵� �ӵ�
    public float rotationDuration = 1f; // ȸ�� ���� �ð�
    private Vector3[] cameraRotations; // ī�޶� ȸ�� ��ġ��
    private int rotationIndex = 0; // ���� ī�޶� ȸ�� �ε���
    private bool isRotating = false; // ī�޶� ȸ�� ������ ����

    void Start()
    {
        // ī�޶� ȸ�� ��ġ �ʱ�ȭ
        cameraRotations = new Vector3[]
        {
            new Vector3(45f, 45f, 0f),
            new Vector3(45f, -45f, 0f),
            new Vector3(45f, -135f, 0f),
            new Vector3(45f, -225f, 0f)
        };

        // �ʱ� ī�޶� ȸ�� ��ġ ����
        UpdateCameraPosition(rotationIndex);
    }

    void Update()
    {
        // �̵� �Է� �ޱ�
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // ī�޶��� ���� ���͸� �������� �̵� ���� ����
        Vector3 forwardDirection = mainCamera.transform.forward;
        forwardDirection.y = 0f; // ���� �̵� ����
        Vector3 movementDirection = forwardDirection.normalized * verticalInput + mainCamera.transform.right * horizontalInput;

        // �̵�
        if (movementDirection != Vector3.zero)
        {
            transform.position += movementDirection.normalized * movementSpeed * Time.deltaTime;

            // �̵� �������� ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // O Ű �Ǵ� P Ű�� ������ �� ī�޶� ȸ��
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isRotating) // ī�޶� ȸ�� ���� �ƴ� ��쿡�� ȸ�� ����
            {
                int newIndex = rotationIndex;
                if (Input.GetKeyDown(KeyCode.P))
                    newIndex = (rotationIndex + 1) % cameraRotations.Length; // ���� ��ġ�� �̵�
                else
                    newIndex = (rotationIndex - 1 + cameraRotations.Length) % cameraRotations.Length; // ���� ��ġ�� �̵�

                // ī�޶� ȸ�� ��ġ ����
                UpdateCameraPosition(newIndex);
            }
        }
    }

    // ī�޶��� ȸ�� ��ġ�� �����ϴ� �Լ�
    void UpdateCameraPosition(int index)
    {
        isRotating = true; // ī�޶� ȸ�� �� ���·� ����

        // ���� ��ġ���� ���� ��ġ�� ȸ��
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(cameraRotations[index]);

        // ȸ�� �ִϸ��̼� �ڷ�ƾ ����
        StartCoroutine(RotateCamera(startRotation, endRotation));

        // ���� ī�޶� ȸ�� �ε��� ����
        rotationIndex = index;
    }

    // ī�޶��� ȸ�� �ִϸ��̼��� ó���ϴ� �ڷ�ƾ
    IEnumerator RotateCamera(Quaternion startRotation, Quaternion endRotation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < rotationDuration)
        {
            // ȸ��
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȸ�� �ִϸ��̼� ���� �� ȸ�� ���� ����
        isRotating = false;
    }
}
