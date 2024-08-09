namespace ClientApp.Services;

public class NotificationService
{
    public event Action OnChange;
    public string Message { get; private set; }

    public void ShowMessage(string message)
    {
        Message = message;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}