using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SuSiLoFIREBASE : MonoBehaviour
{
    FirebaseApp app;
    FirebaseAuth auth;
    DatabaseReference db;

    public FirebaseUser user;

    #region thông báo đăng nhập đăng xuất  
    public GameObject notification;
    public TextMeshProUGUI _textNotification;
    public Button closeNotification;

    #endregion
    public void Start()
    {
        closeNotification.onClick.AddListener(() => ClosePanel(notification));  // tắt thông báo lỗi hoặc thành công , nút X

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;         

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance.RootReference;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    //private bool ValidatePassword(string password) // điều kiện của mật khẩu   có chữ in hoa , chữ thường , số , kí tự đặc biệt , từ 8-25 ký tự
    //{
    //    // Yêu cầu: 8-25 ký tự, có chữ in hoa, chữ thường, số, ký tự đặc biệt
    //    string passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,25}$";
    //    return Regex.IsMatch(password, passwordPattern);
    //}


    public void DKTK(string email, string password)      // đăng ký tài khoản
    {
        //if (!ValidatePassword(password)) {

        //    Debug.Log("Mật khẩu phải dài từ 8-25 ký tự , có kí tự đặc biệt , in hoa , thường , số");
        //    return;
        //}
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {   // tạo đăng ký theo email
            if (task.IsCanceled)
            {
                Debug.Log("Không thể tạo tài khoản.");
                StartCoroutine(ShowMessageForSecond("Có lỗi gì đó , chắc do lỗi mạng", 1f));

            }
            else if (task.IsFaulted)
            {
                StartCoroutine(ShowMessageForSecond("Mật khẩu từ 8-25 ký tự , có đủ chữ in hoa , chữ thường ,số , ký tự đặc biệt", 2f));
                Debug.Log("Có lỗi xảy ra : " + task.Exception);

            }
            else
            {
                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                StartCoroutine(ShowMessageForSecond("ĐĂNG KÝ THÀNH CÔNG", 1f));
                StartCoroutine(LoadSceneAfterLogin("1", 3));
            }
        },
        TaskScheduler.FromCurrentSynchronizationContext());
    }


    public void DNTK(string email, string password)   // đăng nhập tài khoản
    {

        auth.SignInWithEmailAndPasswordAsync(email, password).
        ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                StartCoroutine(ShowMessageForSecond("Có lỗi gì đó , chắc do lỗi mạng", 1f));
                Debug.Log("tiến trình đăng nhập bị hủy.");
                return;
            }
            else if (task.IsFaulted)
            {
                Debug.Log("sai tên đăng nhập hoặc mật khẩu: " + task.Exception);
                StartCoroutine(ShowMessageForSecond("Sai tài khoản hoặc mật khẩu , lưu ý mật khẩu từ 8-25 ký tự , có đủ chữ in hoa , chữ thường ,số , ký tự đặc biệt", 1f));
                return;
            }
            else if (task.IsCompletedSuccessfully)
            {
                StartCoroutine(ShowMessageForSecond("Đăng nhập thành công", 1f));
                StartCoroutine(LoadSceneAfterLogin("1", 3f));
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                        result.User.DisplayName, result.User.UserId);
                // vào chọn bản lưu sau khi đăng nhập thành công

            }

        },
           TaskScheduler.FromCurrentSynchronizationContext());
    }
    IEnumerator LoadSceneAfterLogin(string sceneName, float time)
    {
        Debug.Log("chuan bi chuyen scene");
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);

    }
    private IEnumerator ShowMessageForSecond(string message, float duration)  // show panel sau vài giây
    {
        yield return new WaitForSeconds(duration);
        notification.SetActive(true);
        _textNotification.text = message;
    }
    public void ClosePanel(GameObject gg)
    {
        gg.SetActive(false);

    }


}
