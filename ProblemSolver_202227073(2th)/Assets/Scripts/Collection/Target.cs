using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Shooter 스크립트의 인스턴스를 참조
    // 이를 통해 총알을 Shooter의 큐에 다시 추가할 수 있음
    // Shooter 컴포넌트를 가진 GameObject를 인스펙터에서 할당
    public Shooter shooter; // Shooter 스크립트의 인스턴스 참조
    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f); // 감지할 상자의 크기
    public Quaternion boxOrientation = Quaternion.identity; // 감지할 상자의 회전
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectBullets();
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")) // 충돌한 객체가 총알인지 확인
        {
            other.gameObject.SetActive(false); // 총알을 비활성화
            shooter.bulletQueue.Enqueue(other.gameObject); // 비활성화된 총알을 큐에 다시 추가
        }
    }*/

    void DetectBullets()
    {
        // 현재 게임 오브젝트의 위치에서 boxSize 크기의 상자 형태 영역 내의 모든 콜라이더를 찾음
        Collider[] hitColliders = Physics.OverlapBox(transform.position, boxSize / 2, boxOrientation);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Bullet")) // 감지된 객체가 총알인지 확인
            {
                hitCollider.gameObject.SetActive(false); // 총알을 비활성화
                shooter.bulletStack.queue1.Enqueue(hitCollider.gameObject); // 비활성화된 총알을 스택에 다시 추가
            }
        }
    }

    // 개발 중에 감지 영역을 시각화하기 위한 코드 (Unity 에디터에서만 보임)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation * boxOrientation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
