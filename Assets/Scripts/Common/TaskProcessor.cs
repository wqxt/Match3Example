using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class TaskProcessor : MonoBehaviour
    {
        private Queue<IEnumerator> _taskQueue = new Queue<IEnumerator>();
        private bool _isProcessing = false;

        public void AddTask(IEnumerator task)
        {
            _taskQueue.Enqueue(task);
            if (!_isProcessing)
            {
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            _isProcessing = true;

            while (_taskQueue.Count > 0)
            {
                yield return StartCoroutine(_taskQueue.Dequeue());
            }

            _isProcessing = false;
        }
    }
}