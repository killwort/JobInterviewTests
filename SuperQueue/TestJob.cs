using System;
using System.Threading.Tasks;

namespace SuperQueue;

/// <summary>
/// Тестовая реализация задачи.
/// </summary>
public class TestJob : IJob
{
    /// <inheritdoc "/>
    public int Id { get; set; }

    /// <inheritdoc "/>
    public int Priority { get; set; }

    /// <inheritdoc "/>
    public int Owner { get; set; }

    /// <inheritdoc "/>
    public event JobCompletedHandler Completed;

    /// <inheritdoc "/>
    public async Task Run()
    {
        await Task.Delay(Delay);
        OnJobCompleted(--WillSucceedOnTry <= 0 ? null : new Exception("This job has failed!"));
    }

    /// <summary>
    /// Сколько времени в миллисекундах должна выполняться задача.
    /// </summary>
    public int Delay { get; set; }

    /// <summary>
    /// С какой попытки задача будет выполнена успешно.
    /// </summary>
    public int WillSucceedOnTry { get; set; }

    private void OnJobCompleted(Exception e)
    {
        Completed?.Invoke(e);
    }
}
