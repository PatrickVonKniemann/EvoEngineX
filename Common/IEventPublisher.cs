namespace Common;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent eventMessage, string eventQueue) where TEvent : class;
}