using System.Collections.Generic;
using UnityEngine;

public class StackUsingQueues<T>
{
    private Queue<T> primaryQueue = new Queue<T>();
    private Queue<T> secondaryQueue = new Queue<T>();

    // Stack에 요소를 추가합니다. (Push 연산)
    public void Push(T item)
    {
        // 새 요소를 보조 큐에 추가
        secondaryQueue.Enqueue(item);

        // 주 큐의 모든 요소를 보조 큐로 옮김
        while (primaryQueue.Count > 0)
        {
            secondaryQueue.Enqueue(primaryQueue.Dequeue());
        }

        // 큐의 역할을 교환
        Queue<T> tempQueue = primaryQueue;
        primaryQueue = secondaryQueue;
        secondaryQueue = tempQueue;
    }

    // Stack에서 가장 최근에 추가된 요소를 제거하고 반환합니다. (Pop 연산)
    public T Pop()
    {
        if (primaryQueue.Count == 0)
        {
            throw new System.InvalidOperationException("Stack is empty");
        }
        return primaryQueue.Dequeue();
    }

    // Stack이 비어 있는지 확인합니다.
    public bool IsEmpty()
    {
        return primaryQueue.Count == 0;
    }
}