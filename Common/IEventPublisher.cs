namespace Common;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent eventMessage) where TEvent : class;
}