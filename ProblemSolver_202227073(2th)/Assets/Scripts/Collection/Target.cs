using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Shooter 스크립트의 인스턴스를 참조
    // 이를 통해 총알을 Shooter의 큐에 다시 추가할 수 있음
    // Shooter 컴포넌트를 가진 GameObject를 인스펙터에서 할당
    public Shooter shooter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) // 충돌한 객체가 총알인지 확인
        {
            other.gameObject.SetActive(false); // 총알을 비활성화
            shooter.bulletQueue.Enqueue(other.gameObject); // 비활성화된 총알을 큐에 다시 추가
        }
    }
}
