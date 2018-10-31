using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Hymperia
{
  public sealed class AsyncLock : IDisposable
  {
    private object Target { get; }

    private AsyncLock(object target)
    {
      Target = target;
    }

    public void Dispose() => Release(Target);

    [NotNull]
    public static async Task<AsyncLock> Lock(object target)
    {
      if (target is null)
        throw new ArgumentNullException(nameof(target));

      var _lock = AcquireLock(target);
      await _lock.Wait(Thread.CurrentThread.ManagedThreadId, Task.CurrentId);

      // In semaphore, just gotta keep track of it until Lock can be disposed.
      return new AsyncLock(target);
    }

    [NotNull]
    private static InnerLock AcquireLock([NotNull] object key)
    {
      lock (Locks)
      {
        if (!Locks.ContainsKey(key))
          Locks[key] = new InnerLock();

        return Locks[key];
      }
    }

    private static void Release([NotNull] object key)
    {
      InnerLock _lock;

      lock (Locks)
      {
        lock (_lock = Locks[key])
        {
          _lock.Release();
          if (_lock.CanDispose)
          {
            Locks.Remove(key);
            _lock.Dispose();
          }
        }
      }
    }

    [NotNull]
    [ItemNotNull]
    private static Dictionary<object, InnerLock> Locks { get; } = new Dictionary<object, InnerLock>();

    private class InnerLock : IDisposable
    {
      private SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(0, 1);
      private int CurrentCount;
      private int AwaiterCount;

      public int ThreadId { get; private set; }
      public int? TaskId { get; private set; }

      public bool CanDispose => AwaiterCount == 0 && Semaphore.CurrentCount == 0;
      public void Dispose() => Semaphore.Dispose();

      public void Release()
      {
        if (Interlocked.Decrement(ref CurrentCount) == 0)
          Semaphore.Release();
      }

      public async Task Wait(int thread, int? task)
      {
        if (ThreadId == thread && TaskId is int && TaskId == task)
          Interlocked.Increment(ref CurrentCount);
        else
        {
          Interlocked.Increment(ref AwaiterCount);
          await Semaphore.WaitAsync();

          lock (this)
          {
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            TaskId = Task.CurrentId;
            Interlocked.Decrement(ref AwaiterCount);
            Interlocked.Increment(ref CurrentCount);
          }
        }
      }
    }
  }
}
