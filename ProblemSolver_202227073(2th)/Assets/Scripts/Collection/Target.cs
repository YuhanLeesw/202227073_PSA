using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Shooter ��ũ��Ʈ�� �ν��Ͻ��� ����
    // �̸� ���� �Ѿ��� Shooter�� ť�� �ٽ� �߰��� �� ����
    // Shooter ������Ʈ�� ���� GameObject�� �ν����Ϳ��� �Ҵ�
    public Shooter shooter; // Shooter ��ũ��Ʈ�� �ν��Ͻ� ����
    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f); // ������ ������ ũ��
    public Quaternion boxOrientation = Quaternion.identity; // ������ ������ ȸ��
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
        if (other.gameObject.CompareTag("Bullet")) // �浹�� ��ü�� �Ѿ����� Ȯ��
        {
            other.gameObject.SetActive(false); // �Ѿ��� ��Ȱ��ȭ
            shooter.bulletQueue.Enqueue(other.gameObject); // ��Ȱ��ȭ�� �Ѿ��� ť�� �ٽ� �߰�
        }
    }*/

    void DetectBullets()
    {
        // ���� ���� ������Ʈ�� ��ġ���� boxSize ũ���� ���� ���� ���� ���� ��� �ݶ��̴��� ã��
        Collider[] hitColliders = Physics.OverlapBox(transform.position, boxSize / 2, boxOrientation);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Bullet")) // ������ ��ü�� �Ѿ����� Ȯ��
            {
                hitCollider.gameObject.SetActive(false); // �Ѿ��� ��Ȱ��ȭ
                shooter.bulletStack.queue1.Enqueue(hitCollider.gameObject); // ��Ȱ��ȭ�� �Ѿ��� ���ÿ� �ٽ� �߰�
            }
        }
    }

    // ���� �߿� ���� ������ �ð�ȭ�ϱ� ���� �ڵ� (Unity �����Ϳ����� ����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation * boxOrientation, transform.localScale);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
