using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [SerializeField] private Button button;
  
    protected virtual void Reset()
    {
        LoadButtonComponent();
    }
    protected virtual void Awake()
    {
        LoadButtonComponent();
    }
    protected virtual void Start()
    {
        OnButtonEnvent();
    }

    protected virtual void LoadButtonComponent()
    {
        if (button != null) return;
        button = GetComponent<Button>();

    }

    protected virtual void OnButtonEnvent()
    {
        button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
}
