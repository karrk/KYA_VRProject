using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Manager : MonoBehaviour
{
    private static Manager _instance = null;
    public static Manager Instance => _instance;

    //private E_Scene _curScene = E_Scene.InitScene;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        //if(_curScene != E_Scene.MainGame)
        //{
        //    this._curScene = E_Scene.MainGame;
        //    SceneManager.LoadScene((int)E_Scene.MainGame);
        //    SceneManager.sceneLoaded += SceneLoaded;
        //}

        Init();
        DOTween.Init(false, true, LogBehaviour.Verbose);
        DOTween.SetTweensCapacity(50, 200);
    }

    private EventManager _event = null;
    public EventManager Event { get { if (_event == null) _event = new EventManager(); return _event; } }

    [SerializeField] private ObjPoolManager _poolManager = null;
    public ObjPoolManager Pool { get { if (_poolManager == null) _poolManager = new ObjPoolManager(); return _poolManager; } }

    private DataManager _dataManager = null;
    public DataManager Data { get { if (_dataManager == null) _dataManager = new DataManager(); return _dataManager; } }

    [SerializeField] private SFXManager _sfxManager = null;
    public SFXManager SFX { get { if (_sfxManager == null) _sfxManager = new SFXManager(); return _sfxManager; } }

    private VFXManager _vfxManager = null;
    public VFXManager VFX { get { if (_vfxManager == null) _vfxManager = new VFXManager(); return _vfxManager; } }
    
    private void SceneLoaded(Scene m_scene, LoadSceneMode m_loadMode)
    {
        if (m_scene.buildIndex == 1)
        { Init(); }
    }

    private void Init()
    {
        Event.Init();
        Pool.Init();
        SFX.Init();
        Data.Init();
    }
}
