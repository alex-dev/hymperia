using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.Threading
{
  public sealed class AsyncLock : IDisposable
  {
    [NotNull]
    private object Target { get; }

    private AsyncLock([NotNull] object target)
    {
      Target = target;
    }

    public void Dispose() => Release(Target);

    [NotNull]
    public static async Task<AsyncLock> Lock(object target, CancellationToken token = default)
    {
      if (target is null)
        throw new ArgumentNullException(nameof(target));

      var _lock = AcquireLock(target);
      await _lock.Wait(token);

      // In semaphore, just gotta keep track of it until Lock can be released.
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
      private SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1);
      private volatile int Count;

      public bool CanDispose => Count == 0;
      public void Dispose() => Semaphore.Dispose();

      public void Release()
      {
        Semaphore.Release();
        Interlocked.Decrement(ref Count);
      }

      public async Task Wait(CancellationToken token = default)
      {
        Interlocked.Increment(ref Count);
        token.Register(() => Interlocked.Decrement(ref Count));
        await Semaphore.WaitAsync(token);
      }
    }
  }
}
