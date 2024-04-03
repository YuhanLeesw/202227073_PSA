using System.Collections.Generic;
using UnityEngine;

public class StackUsingQueues<T>
{
    public Queue<T> queue1 = new Queue<T>();
    private Queue<T> queue2 = new Queue<T>();

    // Stack에 요소를 추가합니다. (Push 연산)
    public void Push(T item)
    {
        // 새 요소를 보조 큐에 추가
        queue2.Enqueue(item);

        // 주 큐의 모든 요소를 보조 큐로 옮김
        while (queue1.Count > 0)
        {
            queue2.Enqueue(queue1.Dequeue());
        }

        // 큐의 역할을 교환
        Queue<T> tempQueue = queue1;
        queue1 = queue2;
        queue2 = tempQueue;
    }

    // Stack에서 가장 최근에 추가된 요소를 제거하고 반환합니다. (Pop 연산)
    public T Pop()
    {
        if (queue1.Count == 0)
        {
            throw new System.InvalidOperationException("스택 비어있어");
        }
        return queue1.Dequeue();
    }

    // Stack이 비어 있는지 확인합니다.
    public bool IsEmpty()
    {
        return queue1.Count == 0;
    }
}