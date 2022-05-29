public class LossFertility : IState
{
    PlayerInput playerInput;
    LossFertilityControl lossFertilityControl;

    public LossFertility (LossFertilityControl lossFertilityControl, PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        this.lossFertilityControl = lossFertilityControl;
    }

    public void Enter()
    {
        lossFertilityControl.InitControl(playerInput);
    }

    public void Exit()
    {
        lossFertilityControl.CancleControl();
    }

    public void Update()
    {
    }
}
