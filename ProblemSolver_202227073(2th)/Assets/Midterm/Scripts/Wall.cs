using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� ���ʹ̳� �÷��̾����� Ȯ��
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            // �浹�� ��ü�� ��ġ�� �̵���Ŵ
            // �̵��� ��ġ�� ���� ��ġ���� �ݴ� �������� �����Ͽ� ���� �굵�� ��
            Vector3 oppositeDirection = -other.transform.forward * 0.1f; // ���÷� 0.1f ��ŭ ������ ������ ��ġ
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
