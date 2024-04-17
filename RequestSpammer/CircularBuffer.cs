using System.Collections;

namespace RequestSpammer;

public class CircularBuffer<T> : IEnumerable<T>
{
    private readonly T[] _buffer;
    private int _head;
    private int _count;
    private readonly CancellationToken _cancellationToken;

    public CircularBuffer(int capacity, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        _buffer = new T[capacity];
        _head = 0;
        _count = 0;
    }

    public int Capacity => _buffer.Length;

    public int Count => _count;

    public void Add(T item)
    {
        _buffer[_head] = item;
        _head = (_head + 1) % Capacity;
        if (_count < Capacity)
        {
            _count++;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var i = 0;
        while (!_cancellationToken.IsCancellationRequested)
        {
            if (i >= _count)
            {
                i = 0;
            }
            
            yield return _buffer[i];
            i++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}