using System;

namespace SuperQueue;

/// <summary>
/// Обработчик события окончания выполнения задачи.
/// </summary>
public delegate void JobCompletedHandler(Exception error);
