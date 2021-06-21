#nullable enable

using UnityEngine;
using System;

public class MinHeap<T> where T: IComparable<T>
{
    private T[] Items;
    public int ItemCount { get; private set; } = 0;

    public MinHeap(int maxSize)
    {
        Items = new T[maxSize];
    }

    public void Add(T item)
    {
        if (ItemCount >= Items.Length) throw new InvalidOperationException();

        Items[ItemCount] = item;
        ItemCount++;

        HeapifyUp();
    }

    public T Peek()
    {
        if (ItemCount < 1) throw new InvalidOperationException();

        return Items[0];
    }

    public T Remove()
    {
        if (ItemCount < 1) throw new InvalidOperationException();

        var value = Items[0];

        Items[0] = Items[ItemCount - 1];
        ItemCount--;
        HeapifyDown();

        return value;
    }

    private void HeapifyUp()
    {
        var i = ItemCount - 1;
        var parent = Parent(i);
        while (i > 0 && Items[i].CompareTo(Items[parent]) < 0)
        {
            var aux = Items[i];
            Items[i] = Items[parent];
            Items[parent] = aux;

            i = parent;
            parent = Parent(i);
        }
    }

    private void HeapifyDown()
    {
        var i = 0;
        var inOrder = false;
        while (!inOrder)
        {
            var min = i;
            var left = LeftChild(i);
            var right = RightChild(i);

            if (left < ItemCount && Items[left].CompareTo(Items[min]) < 0) min = left;
            if (right < ItemCount && Items[right].CompareTo(Items[min]) < 0) min = right;

            if (min != i)
            {
                var aux = Items[i];
                Items[i] = Items[min];
                Items[min] = aux;

                i = min;
            }
            else inOrder = true;
        }
    }

    private int Parent(int index)
    {
        return (index - 1) / 2;
    }

    private int LeftChild(int index)
    {
        return (index * 2) + 1;
    }

    private int RightChild(int index)
    {
        return (index * 2) + 2;
    }
}
