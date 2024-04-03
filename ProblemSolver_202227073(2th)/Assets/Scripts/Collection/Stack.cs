using System.Collections.Generic;
using UnityEngine;

public class StackUsingQueues<T>
{
    public Queue<T> queue1 = new Queue<T>();
    private Queue<T> queue2 = new Queue<T>();

    // Stack�� ��Ҹ� �߰��մϴ�. (Push ����)
    public void Push(T item)
    {
        // �� ��Ҹ� ���� ť�� �߰�
        queue2.Enqueue(item);

        // �� ť�� ��� ��Ҹ� ���� ť�� �ű�
        while (queue1.Count > 0)
        {
            queue2.Enqueue(queue1.Dequeue());
        }

        // ť�� ������ ��ȯ
        Queue<T> tempQueue = queue1;
        queue1 = queue2;
        queue2 = tempQueue;
    }

    // Stack���� ���� �ֱٿ� �߰��� ��Ҹ� �����ϰ� ��ȯ�մϴ�. (Pop ����)
    public T Pop()
    {
        if (queue1.Count == 0)
        {
            throw new System.InvalidOperationException("���� ����־�");
        }
        return queue1.Dequeue();
    }

    // Stack�� ��� �ִ��� Ȯ���մϴ�.
    public bool IsEmpty()
    {
        return queue1.Count == 0;
    }
}