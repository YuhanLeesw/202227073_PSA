using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 에너미나 플레이어인지 확인
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            // 충돌한 객체의 위치를 이동시킴
            // 이동할 위치를 현재 위치에서 반대 방향으로 설정하여 벽에 닿도록 함
            Vector3 oppositeDirection = -other.transform.forward * 0.1f; // 예시로 0.1f 만큼 벽에서 떨어진 위치
            other.transform.position += oppositeDirection;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
