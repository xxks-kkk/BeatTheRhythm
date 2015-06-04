/****************************************************
 *  (c) 2012 narayana games UG (haftungsbeschränkt) *
 *  All rights reserved                             *
 ****************************************************/

using System.Collections;
using System.Collections.Generic;

namespace NarayanaGames.Common {
    /// <summary>
    ///     A simple Queue implementation based on the generic List.
    /// </summary>
    /// <remarks>
    ///     While .NET/Mono does have its own generic Queue, it resides in 
    ///     System.dll which you usually don't want to include in your builds
    ///     (it adds quite a bit of "weight"). To prevent having a dependency
    ///     on System.dll, this class was implemented.
    /// </remarks>
    /// <typeparam name="T">whatever you want to put into your queue</typeparam>
    public class NGQueue<T> : List<T> {

        /// <summary>
        ///     Adds <c>item</c> to the end of this queue.
        /// </summary>
        /// <param name="item">the item to add</param>
        public void Enqueue(T item) {
            base.Insert(0, item);
        }

        /// <summary>
        ///     Removes and returns the item that was first put into the queue.
        /// </summary>
        /// <returns>the item that was first put into the queue</returns>
        public T Dequeue() {
            T item = base[base.Count - 1];
            base.RemoveAt(base.Count - 1);
            return item;
        }

        /// <summary>
        ///     Returns the item that was first put into the queue (without
        ///     removing it from the queue).
        /// </summary>
        /// <returns>the item that was first put into the queue</returns>
        public T Peek() {
            return base[base.Count - 1];
        }

        /// <summary>
        ///     Returns the item that was last put into the queue.
        /// </summary>
        /// <returns>the item that was last put into the queue</returns>
        public T PeekLastInLine() {
            return base[0];
        }

        /// <summary>
        ///     Gets all the items in the list in the order they will be
        ///     dequeued (item first put into the list is the first item
        ///     in the list).
        /// </summary>
        public List<T> QueueOrderedList {
            get {
                List<T> result = new List<T>();
                for (int i = Count - 1; i >= 0; i--) {
                    result.Add(this[i]);
                }
                return result;
            }
        }
    }
}