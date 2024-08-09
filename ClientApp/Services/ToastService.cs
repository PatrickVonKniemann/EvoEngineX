// Services/ToastService.cs

namespace ClientApp.Services;

public class ToastService
{
    public event Action<string, string> OnShow;
    public event Action OnHide;

    public void ShowToast(string message, string type)
    {
        OnShow?.Invoke(message, type);
    }

    public void HideToast()
    {
        OnHide?.Invoke();
    }
}