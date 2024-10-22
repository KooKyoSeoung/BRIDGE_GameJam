using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    [SerializeField] private SoundManager sound;

    static Managers s_instance;
    static Managers Instance { get {
        if (s_instance == null)
            Init();
        return s_instance;
    } }
    
    public static InputManager Input { get { return Instance.input; } }
    public static SoundManager Sound { get { return Instance.sound; } }
    // public static UIManager UI { get { return Instance._ui; } }
    
    InputManager input = new InputManager();
    // SoundManager sound = new SoundManager();
    // UIManager _ui = new UIManager();

    static void Init()
    {
        //@Managers라는 오브젝트가 없다면 자동으로 생성해준다.
        if(s_instance == null)
        {
            GameObject go = GameObject.FindWithTag("Manager");

            if (go == null)
            {
                go = new GameObject { name = "@Managers", tag = "Manager" };
                go.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            //s_instance._sound.Init();
        }   
    }

    public static void Clear()
    {
        //Input.Clear();
        //Sound.Clear();
    }

    void Update()
    {
        input.OnUpdate();
    }
}