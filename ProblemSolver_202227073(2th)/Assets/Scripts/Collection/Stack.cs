using System.Collections.Generic;
using UnityEngine;

public class StackUsingQueues<T>
{
    private Queue<T> primaryQueue = new Queue<T>();
    private Queue<T> secondaryQueue = new Queue<T>();

    // Stack�� ��Ҹ� �߰��մϴ�. (Push ����)
    public void Push(T item)
    {
        // �� ��Ҹ� ���� ť�� �߰�
        secondaryQueue.Enqueue(item);

        // �� ť�� ��� ��Ҹ� ���� ť�� �ű�
        while (primaryQueue.Count > 0)
        {
            secondaryQueue.Enqueue(primaryQueue.Dequeue());
        }

        // ť�� ������ ��ȯ
        Queue<T> tempQueue = primaryQueue;
        primaryQueue = secondaryQueue;
        secondaryQueue = tempQueue;
    }

    // Stack���� ���� �ֱٿ� �߰��� ��Ҹ� �����ϰ� ��ȯ�մϴ�. (Pop ����)
    public T Pop()
    {
        if (primaryQueue.Count == 0)
        {
            throw new System.InvalidOperationException("Stack is empty");
        }
        return primaryQueue.Dequeue();
    }

    // Stack�� ��� �ִ��� Ȯ���մϴ�.
    public bool IsEmpty()
    {
        return primaryQueue.Count == 0;
    }
}