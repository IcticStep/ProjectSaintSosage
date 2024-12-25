using Code.Runtime.Infrastructure.GameStates.Api;
using Code.Runtime.Infrastructure.Services.Camera;
using Code.Runtime.Infrastructure.Services.CleanUp;
using Code.Runtime.Infrastructure.Services.SceneMenegment;
using Code.Runtime.Infrastructure.Services.StaticData;
using Code.Runtime.Infrastructure.Services.UiHud;
using Code.Runtime.Services.Customers.Registry;
using Code.Runtime.Services.GlobalGoals.Visualization;
using Code.Runtime.Services.GlobalRocket;
using Code.Runtime.Services.InputService;
using Code.Runtime.Services.Library;
using Code.Runtime.Services.Loading;
using Code.Runtime.Services.Player.CutsceneCopyProvider;
using Cysharp.Threading.Tasks;

namespace Code.Runtime.Infrastructure.GameStates.States
{
    internal sealed class FinishGlobalGoalState : IGameState
    {
        private readonly IInputService _inputService;
        private readonly ICameraProvider _cameraProvider;
        private readonly IGlobalGoalsVisualizationService _globalGoalsVisualizationService;
        private readonly IHudProviderService _hudProviderService;
        private readonly ICustomersRegistryService _customersRegistryService;
        private readonly ILibraryService _libraryService;
        private readonly IPlayerCutsceneCopyProvider _playerCutsceneCopyProvider;
        private readonly IRocketProvider _rocketProvider;
        private readonly ILevelCleanUpService _levelCleanUpService;
        private readonly ILoadingCurtainService _loadingCurtainService;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;

        public FinishGlobalGoalState(IInputService inputService, ICameraProvider cameraProvider, IGlobalGoalsVisualizationService globalGoalsVisualizationService,
            IHudProviderService hudProviderService, ICustomersRegistryService customersRegistryService, ILibraryService libraryService,
            IPlayerCutsceneCopyProvider playerCutsceneCopyProvider, IRocketProvider rocketProvider, ILevelCleanUpService levelCleanUpService,
            ILoadingCurtainService loadingCurtainService, GameStateMachine gameStateMachine, ISceneLoader sceneLoader,
            IStaticDataService staticDataService)
        {
            _inputService = inputService;
            _cameraProvider = cameraProvider;
            _globalGoalsVisualizationService = globalGoalsVisualizationService;
            _hudProviderService = hudProviderService;
            _customersRegistryService = customersRegistryService;
            _libraryService = libraryService;
            _playerCutsceneCopyProvider = playerCutsceneCopyProvider;
            _rocketProvider = rocketProvider;
            _levelCleanUpService = levelCleanUpService;
            _loadingCurtainService = loadingCurtainService;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
        }

        public void Start()
        {
            StopGameplay();
            SetUpFinal();
            PlayFinal().Forget();
        }

        public void Exit() { }

        private void StopGameplay()
        {
            _cameraProvider.DisableFollow();
            _cameraProvider.EnableAnimator();
            _inputService.Disable();
            _hudProviderService.Hide();
            _customersRegistryService.ForceStopAllCustomers();
        }

        private void SetUpFinal()
        {
            _libraryService.ShowSecondFloor();
            _playerCutsceneCopyProvider.GameObject.SetActive(true);
        }

        private async UniTaskVoid PlayFinal()
        {
            await _globalGoalsVisualizationService.PlayFinishCutscene();
            
            _levelCleanUpService.CleanUp();
            _cameraProvider.StopLookingAfter();
            
            await _loadingCurtainService.ShowBlackAsync();
            _gameStateMachine.EnterState<MenuGameState>();
            await _sceneLoader.LoadSceneAsync(_staticDataService.ScenesRouting.AuthorsScene);
        }
    }
}