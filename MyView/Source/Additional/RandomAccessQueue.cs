using System;
using System.Collections.Generic;

namespace MyView.Additional
{
	/// <summary>
	/// A simple queue object that allows random access to its contents.
	/// </summary>
	public class RandomAccessQueue<T>
	{
		#region PROPERTIES
		/// The item currently being pointed to by the internal read head index.
		/// This can be adjusted by calling <see cref="Next"/> and <see cref="Previous"/>.
		public T CurrentItem { get { return m_Queue[m_Index]; } }
		#endregion
		
		
		#region VARIABLES
		private List<T> m_Queue;
		private int m_Index;
		private int m_Size;
		#endregion
		
		
		#region CONSTRUCTOR
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MyView.Additional.RandomAccessQueue`1"/> class.
		/// </summary>
		/// <param name="size">The number of items that can be stored in the queue before the are replaced.</param>
		public RandomAccessQueue(int size = 10)
		{
			m_Size = size;
			m_Queue = new List<T>(size);
		}
		#endregion
		
		
		#region PUBLIC API
		/// <summary>
		/// Adds a new item into the queue.
		/// This adjusts the current index position accordingl, maintaining the pointed-to element.
		/// </summary>
		/// <param name="item">Item to insert.</param>
		public void Enqueue(T item, bool refocusPointer = false)
		{
			if (m_Queue.Count == m_Size)
			{
				Dequeue();
			}
			
			m_Queue.Insert(0, item);
			
			if (!refocusPointer)
			{
				Next();
			}
			else
			{
				m_Index  = 0;
			}
		}
		
		/// <summary>
		/// Removed the oldest item from the queue.
		/// This adjusts the current index position accordingly, maintaining the pointed-to element (if this was not the element removed, otherwise the next oldest is pointed to).
		/// </summary>
		public void Dequeue()
		{
			m_Queue.RemoveAt(m_Queue.Count - 1);
			if (m_Index == m_Queue.Count - 1)
			{
				Previous();
			}
		}
		
		/// <summary>
		/// Moves the pointer by one index toward more recent items and returns the element at this index.
		/// </summary>
		/// <returns>The next item.</returns>
		public T Next()
		{
			Clamp(++m_Index, 0, m_Size);
			return m_Queue[m_Index];
		}
		
		/// <summary>
		/// Moves the pointer by one index toward older items and returns the element at this index.
		/// </summary>
		/// <returns>The previous item.</returns>
		public T Previous()
		{
			Clamp(--m_Index, 0, m_Size);
			return m_Queue[m_Index];
		}
		#endregion
		
		
		#region HELPERS
		/// <summary>
		/// Clamps the provided value within the range specified.
		/// </summary>
		/// <returns>The clamped value.</returns>
		/// <param name="value">Value to clamp.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		int Clamp(int value, int min, int max)
		{
			value = Math.Min(value, max);
			value = Math.Max(value, min);
			
			return value;
		}
		#endregion
	}
}
