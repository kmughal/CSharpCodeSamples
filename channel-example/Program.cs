using System.Threading;
using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace SimpleRawChannelExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await ChannelExample();
            await ChannelExample1();
        }

        static async Task ChannelExample1()
        {
            var ch = Channel.CreateBounded<int>(2);

            _ = Task.Run(async () =>
            {
               // int i = 0;
                for (int i = 0; i < 10 ; i++)
                {
                    await Task.Delay(100);
                    await ch.Writer.WriteAsync(i);
                }

                ch.Writer.Complete();

                // while(await ch.Writer.WaitToWriteAsync()) {
                //     if(ch.Writer.TryWrite(i)) {
                //         i++;
                //     }
                //     await Task.Delay(500);
                // }
            });
            // while (true)
            // {
            //     Console.WriteLine(await ch.Reader.ReadAsync());
            // }
            await foreach (var item in ch.Reader.ReadAllAsync())
            {
                Console.WriteLine("Read value: {0}", item);
            }
            Console.WriteLine("DONE");
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
