
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WSA || UNITY_WSA_10_0 || WINDOWS_UWP
#define UNITY_WINDOWS  
#endif

using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class PluginInterface : MonoBehaviour {
    public Camera[] renderTextureCameras;

    public int renderTextureWidth;
    public int renderTextureHeight;

    public bool layeringOn;

    public bool fullscreen;
    
    public bool selfCloseExternalWindow;

    public bool showPluginInputs;

    public bool showMouseMove;

    public Button closeButton;

    IEnumerator coroutine;
    RenderTexture[] renderTextures;

    readonly List<int> windowsOn = new List<int>();
           
    void Awake() {
#if UNITY_WINDOWS
        instance = this;
#endif
    }

    // Use this for initialization
    void Start() {
        SetupDebugDelegate();
        foreach (Camera renderTextureCamera in renderTextureCameras) {
            renderTextureCamera.backgroundColor = GetComponent<Camera>().backgroundColor;
            renderTextureCamera.clearFlags = GetComponent<Camera>().clearFlags;
        }
        renderTextures = new RenderTexture[renderTextureCameras.Length];
    }

    // Update is called once per frame
    void Update() { }

    [DllImport("ExternalWindowPlugin")]
    static extern void StartWindow([MarshalAs(UnmanagedType.LPWStr)] string title, int width, int height, bool borderless = false);//if borderless is true,there's no border and title bar

    [DllImport("ExternalWindowPlugin")]
    static extern IntPtr StopWindow();

    [DllImport("ExternalWindowPlugin")]
    static extern void SetWindowRect(int left, int top, int width, int height);

    [DllImport("ExternalWindowPlugin")]
    static extern void SendTextureIdToPlugin(IntPtr texId);

    [DllImport("ExternalWindowPlugin")]
    static extern IntPtr InitGraphics();

    [DllImport("ExternalWindowPlugin")]
    static extern IntPtr GetRenderEventFunc();

#if UNITY_WINDOWS
    [DllImport("ExternalWindowPlugin")]
    static extern void StartFullscreenById(int windowId, int w, int h);

    [DllImport("ExternalWindowPlugin")]
    static extern void StartWindowById(int windowId, [MarshalAs(UnmanagedType.LPWStr)] string title, int width, int height, bool borderless = false);//if borderless is true,there's no border and title bar

    [DllImport("ExternalWindowPlugin")]
    static extern IntPtr StopWindowById(int windowId);

    [DllImport("ExternalWindowPlugin")]
    static extern void SetWindowRectById(int windowId, int left, int top, int width, int height);

    [DllImport("ExternalWindowPlugin")]
    static extern void SendTextureIdToPluginById(int windowId, IntPtr texId);

    [DllImport("ExternalWindowPlugin")]
    static extern void SetWindowAttributesById(int windowId, int color, byte alpha, int flags);
    
    [DllImport("ExternalWindowPlugin")]
    static extern void SetColorFormat(int colorFormat);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WindowMessageDelegate(uint message, uint wParam, uint lParam);

    [DllImport("ExternalWindowPlugin")]
    static extern void SetWindowMessageFunction(IntPtr fp);

    static PluginInterface instance;

#endif

    public bool ToggleShowWindow(int id) {
        if (windowsOn.Contains(id)) {
            windowsOn.Remove(id);
            CloseExternalWindow(id);            
        } else {
            ShowExternalWindow(id);
        }
        return windowsOn.Contains(id);
    }

    IEnumerator CallPluginAtEndOfFrame(int id) {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        yield return new WaitForEndOfFrame();

        RenderBuffer localRenderBuffer = renderTextures[id].colorBuffer;
        IntPtr ptr = renderTextures[id].GetNativeTexturePtr();

#if UNITY_WINDOWS
        SendTextureIdToPluginById(id, ptr);
#else
        SendTextureIdToPlugin(ptr);
#endif

        GL.IssuePluginEvent(InitGraphics(), 0);
        Application.runInBackground = true;
        
        yield return new WaitForEndOfFrame();

        if (coroutine == null) {
            coroutine = CallRenderEvent();
            StartCoroutine(coroutine);
        }
    }

    IEnumerator CallRenderEvent(){
        while (true){
			yield return new WaitForEndOfFrame();
            GL.IssuePluginEvent(GetRenderEventFunc(), 0);
        }		
    }

    void ShowExternalWindow (int id)
	{
		const string macOSX = "Mac OS";
		const string windows = "Windows";
		string ErrorMessage = "";
		const string graphicsNotSuported = "Please select the correct option at Build Settings->Player Settings->Other Settings->Auto Graphics API";
		const string restartUnity = "And restart Unity!";

		if (SystemInfo.operatingSystem.StartsWith (macOSX)) {
			//if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore) {
			//	ErrorMessage = "Only OpenGLCore is supported."+graphicsNotSuported+" for Mac."+restartUnity;
			//}
		} else if (SystemInfo.operatingSystem.StartsWith (windows)) {
			if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11) {
				ErrorMessage = "Only Direct3D11 is supported."+graphicsNotSuported+" for Windows."+restartUnity;
			}
		} else {
			ErrorMessage = "Only Windows and MacOSX are suported!";
		}

		if(ErrorMessage.Length == 0){
            //when the external window is created, it is preferable for the texture size to match the window's size
            //the size shouldn't be greater than the screen's size
            if ((renderTextures[id] == null) || (renderTextureWidth != renderTextures[id].width) || (renderTextureHeight != renderTextures[id].height)){                

                RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGB32;//choose here ARGBHalf(GraphicsFormat.R16G16B16A16_SFloat)
#if UNITY_WINDOWS
                if (renderTextureFormat == RenderTextureFormat.ARGB32) {
                    SetColorFormat(0);
                } else if (renderTextureFormat == RenderTextureFormat.ARGBHalf /*|| renderTextureFormat == GraphicsFormat.R16G16B16A16_SFloat*/) {
                    SetColorFormat(1);
                }
#endif
                renderTextures[id] = new RenderTexture(renderTextureWidth, renderTextureHeight, 24, renderTextureFormat);
            }
			renderTextureCameras[id].targetTexture = renderTextures[id];

#if UNITY_WINDOWS
            if (fullscreen) {
                try {
                    StartFullscreenById(id, renderTextureWidth, renderTextureHeight);
                } catch (Exception) {
                    fullscreen = false;
                }
            }

            if (!fullscreen) {
                StartWindowById(id, "Unity Camera " + id, renderTextureWidth, renderTextureHeight);
                if (layeringOn) {
                    if (id == 0) {
                        //SetWindowAttributes(0, 0, 0, 0);//disable any layering
                        SetWindowAttributesById(id, ColorToInt(Color.black), 0, 1);//color key transparency                
                                                                                   //SetWindowAttributes(0, 0, 196, 2);// alpha transparency whole window
                                                                                   //SetWindowAttributesById(id, ColorToInt(Color.black), 196, 3);//both color key transparency and alpha transparency
                    } else {
                        // choose any layering
                    }
                }
            }


#else
            StartWindow("Unity Camera", renderTextureWidth, renderTextureHeight, false);
#endif
            windowsOn.Add(id);
            
            StartCoroutine (CallPluginAtEndOfFrame(id));
		} else {
			Debug.LogError(ErrorMessage);			
		}
	}
    
    void CloseExternalWindow(int id){
        StartCoroutine(StopWindowCoroutine(id));
    }

    IEnumerator StopWindowCoroutine(int id) {
        yield return new WaitForEndOfFrame();
        if (windowsOn.Count == 0) {
            StopGraphicsCoroutine();
        }
        yield return new WaitForEndOfFrame();
#if UNITY_WINDOWS
        StopWindowById(id);
#else
        StopWindow();
#endif
    }

    void OnApplicationQuit (){
        if (windowsOn.Count > 0){
            StopGraphicsCoroutine();
#if UNITY_WINDOWS
            foreach (int winId in windowsOn) {
                StopWindowById(winId);
            }
#else
            StopWindow();
#endif
            windowsOn.Clear();
        }
	}

    void StopGraphicsCoroutine() {
        StopCoroutine(coroutine);
        coroutine = null;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DebugDelegate(string message);

	[DllImport ("ExternalWindowPlugin")]
	static extern void SetDebugFunction (IntPtr fp);

	static void CallbackFunction(string message){
		Debug.Log("--PluginCallback--: "+message);
	}
    
    void SetupDebugDelegate(){
		DebugDelegate callbackDelegate = new DebugDelegate(CallbackFunction);
		IntPtr intPtrDelegate = Marshal.GetFunctionPointerForDelegate(callbackDelegate);
		SetDebugFunction(intPtrDelegate);
	}

    int ColorToInt(Color32 color) {
        return (color.r << 16) + (color.g << 8) + color.b;
    }

}
