using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SuperQueue;

[TestFixture]
public class QueueTest
{
    [Test]
    public async void TestQueue()
    {
        ISuperQueue queue = null;
        queue.Enqueue(new TestJob { Id = 1, Owner = 1, Priority = 1, Delay = 100, WillSucceedOnTry = 1 }); // От пользователя 1
        queue.Enqueue(new TestJob { Id = 2, Owner = 2, Priority = 1, Delay = 100, WillSucceedOnTry = 1 }); // От пользователя 2
        queue.Enqueue(new TestJob { Id = 3, Owner = 3, Priority = 1, Delay = 100, WillSucceedOnTry = 1 }); // От пользователя 3
        queue.Enqueue(new TestJob { Id = 4, Owner = 1, Priority = 1, Delay = 100, WillSucceedOnTry = 2 }); // От пользователя 1, выполняется со второго раза
        queue.Enqueue(new TestJob { Id = 5, Owner = 1, Priority = 2, Delay = 100, WillSucceedOnTry = 1 }); // От пользователя 1, приоритетная
        queue.Enqueue(new TestJob { Id = 6, Owner = 1, Priority = 1, Delay = 100, WillSucceedOnTry = 1 }); // От пользователя 1
        var ranJobs = new List<int>();
        for (var currentJob = await queue.Dequeue(); queue.Count > 0; currentJob = await queue.Dequeue())
        {
            if (currentJob == null)
            {
                await Task.Delay(50);
                continue;
            }

            ranJobs.Add(currentJob.Id);
            currentJob.Run(); // await не нужен, так как задачи выполняется параллельно
        }

        //Сначала будет выполнена приоритетная задача №5, потом №2 и 3 (но не №1 так как у неё тот же владелец что и у №5),
        //потом №1, 4 и 6, потом ещё раз №4, так как с первого раза она не выполняется
        Assert.True(Enumerable.SequenceEqual(new[] { 5, 2, 3, 1, 4, 6, 4 }, ranJobs));
    }
}