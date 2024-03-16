using System;

namespace ProblemSolver_2thtest.Collection
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class Queue<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public Queue()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Enqueue(T data)
        {
            Node<T> node = new Node<T>(data);
            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail.Next = node;
                tail = node;
            }
            count++;
        }

        public T Dequeue()
        {
            if (head == null) throw new InvalidOperationException("Queue is empty");

            T output = head.Data;
            head = head.Next;
            if (head == null) tail = null;
            count--;

            return output;
        }

        public int Count()
        {
            return count;
        }
    }
}
