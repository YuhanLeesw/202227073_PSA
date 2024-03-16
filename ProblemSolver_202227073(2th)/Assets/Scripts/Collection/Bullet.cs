using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; //초기 총알 속도
    public Vector3 dir = Vector3.right; //초기 총알 위치 오른쪽 설정

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //총알 지정된 방향과 속도로 이동
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //총알이 빨간색 큐브에 닿을시,빨간색 큐브에 'Target' 태그를 가지고 있는지 확인
        if (other.CompareTag("Target"))
        {
            Debug.Log("충돌하였음");
            /*Destroy(gameObject);//총알을 파괴*/
            gameObject.SetActive(false);
        }
    }
}
