using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private bool _isHolding;
    private bool _dashInputStop;

    private float _lastDashTime;

    private Vector2 _dashDirection;
    private Vector2 _dashDirectionInput;
    private Vector2 _lastAfterImagePos;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.InputHandler.UseDashInput();

        _isHolding = true;
        _dashDirection = Vector2.right * player.FacingDirection;

        Time.timeScale = playerData.HoldTimeScale;
        startTime = Time.unscaledTime;

        player.DashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        if(player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.DashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(false == isExitingState)
        {
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", player.CurrentVelocity.x);


            if (_isHolding)
            {
                _dashDirectionInput = player.InputHandler.DashDirectionInput;
                _dashInputStop = player.InputHandler.DashInputStop;

                if (_dashDirectionInput != Vector2.zero)
                {
                    _dashDirection = _dashDirectionInput;
                    _dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
                player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45);

                if (_dashInputStop || Time.unscaledTime >= startTime + playerData.MaxHoldTime)
                {
                    _isHolding = false;
                    Time.timeScale = 1;
                    startTime = Time.time;
                    player.CheckIfshouldFlip(Mathf.RoundToInt(_dashDirection.x));
                    player.RB.drag = playerData.Drag;
                    player.SetVelocity(playerData.DashVelocity, _dashDirection);
                    player.DashDirectionIndicator.gameObject.SetActive(false);
                    PlaceAfterImage();
                }
            }
            else
            {
                player.SetVelocity(playerData.DashVelocity, _dashDirection);
                CheckIfShouldPlaceAfterImage();

                if(Time.time >= startTime + playerData.DashTime)
                {
                    player.RB.drag = 0;
                    isAbilityDone = true;
                    _lastDashTime = Time.time;
                }
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position, _lastAfterImagePos) >= playerData.DistBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        PlayerAfterImagePool.Instance.GetFromPool();
        _lastAfterImagePos = player.transform.position;
    }

    public bool CheckIfCanDash() => CanDash && Time.time >= _lastDashTime + playerData.DashCoolDown;

    public void ResetCanDash() => CanDash = true;

}
