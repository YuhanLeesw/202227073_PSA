using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform bulletSpawnPoint; // 총알이 생성될 위치
    public Queue<GameObject> bulletQueue = new Queue<GameObject>(); // 총알을 관리할 큐
    // Start is called before the first frame update
    void Start()
    {
        InitializeBullets();
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자가 마우스 왼쪽 버튼을 클릭했는지 체크
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }
    }
    void InitializeBullets()
    {
        for (int i = 0; i < 10; i++) // 10개의 총알을 초기화
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.SetActive(false); // 총알을 비활성화 상태로 시작
            bulletQueue.Enqueue(bullet); // 큐에 총알을 추가
        }
    }

    void FireBullet()
    {
        if (bulletQueue.Count > 0) // 큐에 총알이 있을 경우에만 발사
        {
            GameObject bullet = bulletQueue.Dequeue(); // 큐에서 총알 하나를 소환
            bullet.transform.position = bulletSpawnPoint.position; // 총알의 위치를 발사 지점으로 설정
            bullet.SetActive(true); // 총알을 활성화 상태로 변경

           
        }
    }
}
