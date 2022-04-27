using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UVL
{
    /// <summary>
    /// Alternative to .NET Task<TResult> that works with Unity's coroutines.
    /// </summary>
    
    /// <remarks>
    /// Coroutines are required to synchronise certain main-thread-only operations
    /// (e.g. Texture2D manipulations) with Unity's frame tick rate.
    /// </remarks>
    public class AsyncResult<T> : AsyncResult
    {
        public T Result { get; private set; }

        public AsyncResult() : base()
        {
            Result = default(T);
        }

        public void Complete(T result)
        {
            Report(1f);
            Result = result;
            stopTime = DateTime.Now;

            Status = TaskStatus.RanToCompletion;
            OnResultComplete?.Invoke(this, Status);
        }
    }

    public class AsyncResult
    {
        public EventHandler<TaskStatus> OnResultComplete;

        public readonly Progress<float> Progress;
        public float ProgressValue { get; protected set; }
        public TaskStatus Status { get; protected set; }

        public double Elapsed => GetElapsedTime();
        protected DateTime? startTime, stopTime;

        public bool inProgress =>
            Status != TaskStatus.Canceled &&
            Status != TaskStatus.Faulted &&
            Status != TaskStatus.RanToCompletion;

        public AsyncResult()
        {
            Progress = new Progress<float>();
            Status = TaskStatus.Created;
        }

        public void Queue()
        {
            Status = TaskStatus.WaitingToRun;
        }

        public void Start()
        {
            startTime = DateTime.Now;
            Status = TaskStatus.Running;
        }

        public void Report(float progress)
        {
            ProgressValue = progress;
            ((IProgress<float>)Progress).Report(progress);
        }

        public void Cancel()
        {
            Status = TaskStatus.Canceled;
            stopTime = DateTime.Now;
            OnResultComplete?.Invoke(this, Status);
        }

        public void Throw(Exception e)
        {
            Debug.LogError($"Async task failed with exception: {e.Message}");

            Status = TaskStatus.Faulted;
            stopTime = DateTime.Now;
            OnResultComplete?.Invoke(this, Status);
        }

        private double GetElapsedTime()
        {
            DateTime start = startTime != null ? (DateTime)startTime : DateTime.Now;
            DateTime stop = stopTime != null ? (DateTime)stopTime : DateTime.Now;

            return (stop - start).TotalMilliseconds;
        }
    }
}