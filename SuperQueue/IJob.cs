using System.Threading.Tasks;

namespace SuperQueue;

/// <summary>
///     Задача в очереди.
/// </summary>
public interface IJob
{
    /// <summary>
    /// Идентификатор задачи. С ним ничего делать не надо, он для тестов.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Приоритет задачи. Чем выше - тем раньше она должна быть выполнена.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Владелец (пользователь) задачи.
    /// </summary>
    int Owner { get; }

    /// <summary>
    /// Событие, информирующее об окончании выполнения задачи.
    /// </summary>
    event JobCompletedHandler Completed;

    /// <summary>
    /// Запуск задачи на выполнение.
    /// </summary>
    /// <returns></returns>
    Task Run();
}
