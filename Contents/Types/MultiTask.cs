
namespace Eggington.Contents.Types;

/// <summary>
/// An extremely simple class that collects multiple tasks easily and allows for a wait all method
/// </summary>
public class MultiTask
{
    /// <summary>
    /// The collected tasks
    /// </summary>
    private List<Task> Tasks { get; } = [];


    /// <summary>
    /// Creates a new task and adds it to this multitask object
    /// </summary>
    /// <param name="taskFunction">The function to return a task</param>
    public void Run(Func<Task?> taskFunction) =>
        Tasks.Add(taskFunction() ?? Task.CompletedTask);


    /// <summary>
    /// Subscribes a task to the list
    /// </summary>
    /// <param name="multiTask">This object to subscribe the task to</param>
    /// <param name="task">The task to subscribe</param>
    /// <returns>The same object with the task subscribed to</returns>
    public static MultiTask operator <<(MultiTask multiTask, Task task)
    {
        multiTask.Tasks.Add(task);
        return multiTask;
    }

    /// <summary>
    /// Simple addition of one multitask to the other. All of b goes to a
    /// </summary>
    /// <param name="a">The first multitask that also gets returned</param>
    /// <param name="b">The other multitask to merge with</param>
    /// <returns>ZMultitask a with b's tasks</returns>
    public static MultiTask operator <<(MultiTask a, MultiTask b)
    {
        a.Tasks.AddRange(b.Tasks);
        return a;
    }

    /// <summary>
    /// Awaits all of the tasks stored within this object. All tasks are then cleared.
    /// </summary>
    /// <returns>An awaitable task</returns>
    public async Task WaitAllAsync()
    {
        await Task.WhenAll(Tasks);
        Tasks.Clear();
    }

    /// <summary>
    /// Waits all of the tasks stored within this object. All tasks are then cleared. This is a thread-blocking call.
    /// </summary>
    public void WaitAll()
    {
        Task.WhenAll(Tasks).Wait();
        Tasks.Clear();
    }
}
