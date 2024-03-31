using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform bulletSpawnPoint; // �Ѿ��� ������ ��ġ
    /*public Queue<GameObject> bulletQueue = new Queue<GameObject>(); // �Ѿ��� ������ ť*/
    public StackUsingQueues<GameObject> bulletStack = new StackUsingQueues<GameObject>(); //�Ѿ��� ������ ����

    void Start()
    {
        InitializeBullets();
    }

    
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
            bulletStack.Push(bullet); // ���ÿ� �Ѿ��� �߰�
        }
    }

    void FireBullet()
    {
        if (!bulletStack.IsEmpty()) // ���ÿ� �Ѿ��� ���� ��쿡�� �߻�
        {
            GameObject bullet = bulletStack.Pop(); // ���ÿ��� �Ѿ� �ϳ��� ����
            bullet.transform.position = bulletSpawnPoint.position; // �Ѿ��� ��ġ�� �߻� �������� ����
            bullet.SetActive(true); // �Ѿ��� Ȱ��ȭ ���·� ����
           

        }
        

    }
   
}
