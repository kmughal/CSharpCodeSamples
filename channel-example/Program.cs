using System.Threading;
using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;

namespace SimpleRawChannelExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await ChannelExample();
        }

        static async Task ChannelExample()
        {
            var ch = new Channel<int>();
            var _ = Task.Run(() =>
            {
                for (var i = 0; i < 1000000; i++)
                {
                    Task.Delay(1000);
                    ch.Write(i);
                }
            });

            while (true)
            {
                Console.WriteLine(await ch.ReadAsync());
            }
        }
    }



    class Channel<T>
    {
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(0);

        public void Write(T item)
        {
            _queue.Enqueue(item);
            _sem.Release();
        }

        public async Task<T> ReadAsync()
        {
            await _sem.WaitAsync();
            _queue.TryDequeue(out T result);
            return result;
        }
    }
}
