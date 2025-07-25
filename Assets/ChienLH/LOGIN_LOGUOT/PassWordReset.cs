
using Firebase;
using Firebase.Auth; // Quan trọng: Phải thêm namespace này!
using System.Collections; // Để tương tác với UI như InputField, Text
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordResetManager : MonoBehaviour
{
    // Kéo thả InputField từ Hierarchy vào đây trong Inspector
    public TMP_InputField emailInputField;
    // Kéo thả Text (hoặc TMP_Text) để hiển thị thông báo


    private FirebaseAuth auth;

    #region thông báo đăng nhập đăng xuất  
    public GameObject notification;
    public TextMeshProUGUI messageText;
    public Button closeNotification;
   

    #endregion
    void Start()
    {
      
        // Khởi tạo Firebase SDK
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase đã sẵn sàng để sử dụng
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth đã khởi tạo.");
            }
            else
            {
                Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
    private void Update()
    {
        debug11();
    }

    public void debug11()
    {
        string email = emailInputField.text;
        Debug.Log(email);
    }
    // Phương thức này sẽ được gọi khi người chơi nhấn nút "Quên Mật khẩu"
    public void OnResetPasswordButtonClicked()
    {
        string email = emailInputField.text;

        if (string.IsNullOrEmpty(email))
        {
            messageText.text = "Vui lòng nhập địa chỉ email của bạn.";
           // StartCoroutine(ShowMessageForSecond(messageText.text, 1f));
           
        }

        // Đặt lại text thông báo
        messageText.text = "Đang gửi yêu cầu...";

        // Gửi yêu cầu đặt lại mật khẩu
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("SendPasswordResetEmailAsync was canceled.");
                messageText.text = "Yêu cầu đã bị hủy.";
               // StartCoroutine(ShowMessageForSecond(messageText.text, 1f));
               
            }
            if (task.IsFaulted)
            {
                Debug.Log($"SendPasswordResetEmailAsync encountered an error: {task.Exception}");
                // Hiển thị lỗi cụ thể cho người dùng
                string errorMessage = task.Exception.InnerExceptions[0].Message;
                if (task.Exception.InnerExceptions[0] is Firebase.FirebaseException firebaseEx)
                {
                    // Lấy mã lỗi Firebase để cung cấp thông báo chính xác hơn
                    // Ví dụ: AuthError.InvalidEmail, AuthError.UserNotFound
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    switch (errorCode)
                    {
                        case AuthError.InvalidEmail:
                            errorMessage = "Địa chỉ email không hợp lệ.";
                         //   StartCoroutine(ShowMessageForSecond(errorMessage, 1f));
                            break;
                        case AuthError.UserNotFound:
                            errorMessage = "Không tìm thấy người dùng với địa chỉ email này.";
                           // StartCoroutine(ShowMessageForSecond(errorMessage, 1f));
                            break;
                        default:
                            errorMessage = "Có lỗi xảy ra khi gửi email đặt lại mật khẩu.";
                           // StartCoroutine(ShowMessageForSecond(errorMessage, 1f));
                            break;
                    }
                }
                messageText.text = "Lỗi: " + errorMessage;
            //    StartCoroutine(ShowMessageForSecond(messageText.text, 1f));
            }

            // Gửi thành công!
            messageText.text = "Vui lòng kiểm tra email của bạn để đặt lại mật khẩu.";
           // StartCoroutine(ShowMessageForSecond(messageText.text,1f));
           Debug.Log("Password reset email sent successfully to " + email);
        } ,TaskScheduler.FromCurrentSynchronizationContext());
      
    }
    private IEnumerator ShowMessageForSecond(string message, float duration)  // show panel sau vài giây
    {
        yield return new WaitForSeconds(duration);
        notification.SetActive(true);
        messageText.text = message;
    }
    public void ClosePanel(GameObject gg)
    {
        gg.SetActive(false);

    }
}
