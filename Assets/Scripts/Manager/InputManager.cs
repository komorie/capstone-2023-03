
//��ǲ �׼ǵ��� ����
public class InputManager : Singleton<InputManager>
{
    public GameActions KeyActions { get; set; }

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        KeyActions = new GameActions();
        KeyActions.Enable();
    }
}
