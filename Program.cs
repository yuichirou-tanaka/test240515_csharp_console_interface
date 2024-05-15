public interface IDamageable
{
    int Health { get; }
    void TakeDamage(int value);
}


/// <summary>
/// 敵
/// </summary>
public sealed class Enemy : IDamageable
{
    public int Health => _health;

    int _health = 10;

    public void TakeDamage(int value)
    {
        _health -= value;
        if (_health <= 0)
        {
            // Healthが0になった場合の処理
        }
    }
}
/// <summary>
/// Androidバックボタンを利用したいスクリプトに実装
/// </summary>
public interface IAndroidBackButtonHandler
{
    void Close();
}


/// <summary>
/// Androidバックボタンの管理/実行
/// </summary>
public class AndroidBackButtonInvoker
{
    static AndroidBackButtonInvoker s_instance;
    readonly List<IAndroidBackButtonHandler> _objs = new();

    public static void Initialize()
    {
        if (s_instance == default)
        {
            s_instance = new AndroidBackButtonInvoker();
        }
    }

    public static void Add(IAndroidBackButtonHandler androidBackButton)
    {
        if (s_instance != default)
        {
            s_instance._objs.Add(androidBackButton);
        }
    }

    public static void Remove(IAndroidBackButtonHandler androidBackButton)
    {
        if (s_instance != default)
        {
            s_instance._objs.Remove(androidBackButton);
        }
    }

    void Update()
    {
        // Android端末でバックボタンが押されたらCloseを実行する
        {
            var lastIdx = _objs.Count - 1;
            var obj = _objs[lastIdx];
            obj.Close();
            // Close後に破棄処理が実行されていない場合、Removeする
            if (_objs.Count - 1 == lastIdx)
            {
                _objs.RemoveAt(lastIdx);
            }
        }
    }
}

/// <summary>
/// ダイアログ
/// </summary>
public sealed class Dialog : IAndroidBackButtonHandler
{
    /// <summary>
    /// ダイアログを開く
    /// </summary>
    public void OpenDialog()
    {
        AndroidBackButtonInvoker.Add(this);
        // (省略)
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    public void CloseDialog()
    {
        AndroidBackButtonInvoker.Remove(this);
        // (省略)
    }

    /// <summary>
    /// Addした場合、AndroidBackButtonInvokerから呼ばれる
    /// </summary>
    public void Close() => CloseDialog();

    /// <summary>
    /// このオブジェクトがDestroyされた場合もRemoveする
    /// </summary>
    void OnDestroy() => AndroidBackButtonInvoker.Remove(this);
}
/// <summary>
/// プレイヤーのステータス(読み取り専用)
/// </summary>
public interface IReadOnlyPlayerStatus
{
    int Health { get; }
}
/// <summary>
/// プレイヤーのステータス
/// </summary>
public sealed class PlayerStatus : IReadOnlyPlayerStatus
{
    public int Health { get; private set; }

    public PlayerStatus(int health) => Health = health;

    public void AddHealth(int value) => Health += value;

    public void RemoveHealth(int value) => Health -= value;
}

class TestClass
{
    static void Main(string[] args)
    {
        IDamageable e = new Enemy();
        e.TakeDamage(1);
        Console.WriteLine(e.Health);
    }
}
