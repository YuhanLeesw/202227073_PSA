using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; //�ʱ� �Ѿ� �ӵ�
    public Vector3 dir = Vector3.right; //�ʱ� �Ѿ� ��ġ ������ ����

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�Ѿ� ������ ����� �ӵ��� �̵�
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�Ѿ��� ������ ť�꿡 ������,������ ť�꿡 'Target' �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Target"))
        {
            Debug.Log("�浹�Ͽ���");
            /*Destroy(gameObject);//�Ѿ��� �ı�*/
            gameObject.SetActive(false);
        }
    }
}
