using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform bulletSpawnPoint; // �Ѿ��� ������ ��ġ
    public Queue<GameObject> bulletQueue = new Queue<GameObject>(); // �Ѿ��� ������ ť
    // Start is called before the first frame update
    void Start()
    {
        InitializeBullets();
    }

    // Update is called once per frame
    void Update()
    {
        // ����ڰ� ���콺 ���� ��ư�� Ŭ���ߴ��� üũ
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }
    }
    void InitializeBullets()
    {
        for (int i = 0; i < 10; i++) // 10���� �Ѿ��� �ʱ�ȭ
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.SetActive(false); // �Ѿ��� ��Ȱ��ȭ ���·� ����
            bulletQueue.Enqueue(bullet); // ť�� �Ѿ��� �߰�
        }
    }

    void FireBullet()
    {
        if (bulletQueue.Count > 0) // ť�� �Ѿ��� ���� ��쿡�� �߻�
        {
            GameObject bullet = bulletQueue.Dequeue(); // ť���� �Ѿ� �ϳ��� ��ȯ
            bullet.transform.position = bulletSpawnPoint.position; // �Ѿ��� ��ġ�� �߻� �������� ����
            bullet.SetActive(true); // �Ѿ��� Ȱ��ȭ ���·� ����

           
        }
    }
}
