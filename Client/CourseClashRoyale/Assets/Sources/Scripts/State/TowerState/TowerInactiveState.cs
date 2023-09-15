public class TowerInactiveState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly BuildingsProvider _buildingsProvider;
    private readonly HealthPresenter _healthPresenter;

    public TowerInactiveState(
        StateMachine stateMachine,
        BuildingsProvider buildingsProvider,
        HealthPresenter healthPresenter)
    {
        _stateMachine = stateMachine;
        _buildingsProvider = buildingsProvider;
        _healthPresenter = healthPresenter;
    }

    public void Enter()
    {
        _buildingsProvider.FriendlyTowerDestroyed += OnFriendlyTowerDestroy;
        _healthPresenter.HealthChanged += OnHealthChange;
    }

    public void Update()
    {
    }

    public void Exit()
    {
        _buildingsProvider.FriendlyTowerDestroyed -= OnFriendlyTowerDestroy;
        _healthPresenter.HealthChanged -= OnHealthChange;
    }

    private void OnHealthChange()
    {
        Disable();
        GoToActiveState();
    }

    private void OnFriendlyTowerDestroy(ITarget target)
    {
        Disable();
        GoToActiveState();
    }

    private void Disable()
    {
        _buildingsProvider.FriendlyTowerDestroyed -= OnFriendlyTowerDestroy;
        _healthPresenter.HealthChanged -= OnHealthChange;
    }

    private void GoToActiveState()
    {
        _stateMachine.Change<TowerFindTargetState>();
    }
}
