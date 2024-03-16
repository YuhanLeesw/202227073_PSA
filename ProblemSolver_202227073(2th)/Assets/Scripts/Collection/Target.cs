using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Shooter ��ũ��Ʈ�� �ν��Ͻ��� ����
    // �̸� ���� �Ѿ��� Shooter�� ť�� �ٽ� �߰��� �� ����
    // Shooter ������Ʈ�� ���� GameObject�� �ν����Ϳ��� �Ҵ�
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
        if (other.gameObject.CompareTag("Bullet")) // �浹�� ��ü�� �Ѿ����� Ȯ��
        {
            other.gameObject.SetActive(false); // �Ѿ��� ��Ȱ��ȭ
            shooter.bulletQueue.Enqueue(other.gameObject); // ��Ȱ��ȭ�� �Ѿ��� ť�� �ٽ� �߰�
        }
    }
}
