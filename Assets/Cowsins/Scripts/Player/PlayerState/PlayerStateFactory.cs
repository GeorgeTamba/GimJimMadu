namespace cowsins
{
    public class PlayerStateFactory
    {
        PlayerStates _context;

        public PlayerStateFactory(PlayerStates currentContext) { _context = currentContext; }

        public PlayerBaseState Default() { return new PlayerDefaultState(_context, this); }

        public PlayerBaseState Jump() { return new PlayerJumpState(_context, this); }

        public PlayerBaseState Crouch() { return new PlayerCrouchState(_context, this); }

        public PlayerBaseState Die() { return new PlayerDeadState(_context, this); }

        public PlayerBaseState Dash() { return new PlayerDashState(_context, this, new UnityEngine.Vector2(InputManager.x, InputManager.y)); }

        public PlayerBaseState Climb() { return new PlayerClimbState(_context, this); }

    }
}