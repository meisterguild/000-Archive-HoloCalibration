// Copyright(c) 2017 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Input;

/// <summary>
///     対角線上に３Dオブジェクトを表示域いっぱいに等間隔に表示するコンポーネント
/// </summary>
public class ViewCalibration : MonoBehaviour
{
    
    /// <summary>
    ///     AirTapすると消えるオブジェクトをランダムに生成するチュートリアルを実施するかどうか設定します。
    /// </summary>
    public bool EnabledTutorial;


    /// <summary>
    ///     目印となる3Dオブジェクトを設定します。
    /// </summary>
    public GameObject[] MarkObjectSets;

    /// <summary>
    ///     AirTap射撃用の3Dオブジェクトを設定します。
    /// </summary>
    public GameObject Target;

    /// <summary>
    ///     AirTap射撃終了用の3Dオブジェクトを設定します。
    /// </summary>
    public GameObject ExitTarget;

    /// <summary>
    /// 背景を白塗りでふさぐかどうか設定します。
    /// </summary>
    public bool IsBackGround;

    /// <summary>
    ///     調整終了後に表示する本来のアプリケーションのシーン名を設定します。
    /// </summary>
    public string SceneName;

    private int marksCount;
    private bool isInitialized = false;
    private GameObject[] _bottomLeftLines;
    private GameObject[] _bottomRightLines;
    private GestureRecognizer _gestureRecognizer;
    private GameObject[] _topLeftLines;
    private GameObject[] _topRightLines;
    private GameObject _exitTarget;
    private GameObject _backGround;
    private float _inteval;

    // Use this for initialization
    private void Start()
    {
        marksCount = MarkObjectSets.Length;
        _bottomRightLines = new GameObject[marksCount];
        _bottomLeftLines = new GameObject[marksCount];
        _topRightLines = new GameObject[marksCount];
        _topLeftLines = new GameObject[marksCount];
        for (var i = 0; i < marksCount; i++)
        {
            _bottomRightLines[i] = Instantiate(MarkObjectSets[i]);
            _bottomLeftLines[i] = Instantiate(MarkObjectSets[i]);
            _topRightLines[i] = Instantiate(MarkObjectSets[i]);
            _topLeftLines[i] = Instantiate(MarkObjectSets[i]);
            _bottomRightLines[i].transform.parent = Camera.main.transform;
            _bottomLeftLines[i].transform.parent = Camera.main.transform;
            _topRightLines[i].transform.parent = Camera.main.transform;
            _topLeftLines[i].transform.parent = Camera.main.transform;
        }
        SetLineObjects(Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 2f)), _bottomRightLines, -1, 1);
        SetLineObjects(Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 2f)), _bottomLeftLines, 1, 1);
        SetLineObjects(Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 2f)), _topRightLines, -1, -1);
        SetLineObjects(Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 2f)), _topLeftLines, 1, -1);

        if (IsBackGround)
        {
            _backGround = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _backGround.transform.position = Camera.main.transform.forward * 3f;
            _backGround.transform.SetParent(Camera.main.transform);

            var width = Vector3.Distance(Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 3f)),
                Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 3f)));
            var height = Vector3.Distance(Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, 3f)),
                Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 3f)));
            _backGround.transform.localScale = new Vector3(width, height, 1f);
            _backGround.transform.rotation= new Quaternion(0f,0f,0f,0f);
        }
        
        _gestureRecognizer = new GestureRecognizer();
        _gestureRecognizer.TappedEvent += _gestureRecognizer_TappedEvent;
        _gestureRecognizer.StartCapturingGestures();
    }


    private void Update()
    {
        // マウスクリックを検出
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            OnClickHandler(ray);
        }
        if (isInitialized)
        {
            _inteval += Time.deltaTime;
        }
    }

    /// <summary>
    ///     不要になった目印などをすべて削除する
    /// </summary>
    private void DestroyCalibration()
    {
        for (var i = 0; i < marksCount; i++)
        {
            Destroy(_bottomRightLines[i]);
            Destroy(_bottomLeftLines[i]);
            Destroy(_topRightLines[i]);
            Destroy(_topLeftLines[i]);
        }


        if (_backGround != null)
        {
            Destroy(_backGround);
            _backGround = null;
        }

        _bottomRightLines = new GameObject[0];
        _bottomLeftLines = new GameObject[0];
        _topRightLines = new GameObject[0];
        _topLeftLines = new GameObject[0];
        marksCount = 0;
    }

    private void OnClickHandler(Ray ray)
    {
        DestroyCalibration();
        if (EnabledTutorial)
        {

            if (!isInitialized)
            {
                CreateTarget();

                _exitTarget = GameObject.Instantiate(ExitTarget);
                _exitTarget.transform.position = Camera.main.transform.forward * 2.5f;
                _exitTarget.transform.name = "ExitTarget";
                isInitialized = true;
            }
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if ( _inteval < 1.0)
                {
                    return;
                }
                if ("Target".Equals(hit.transform.name))
                {
                    Destroy(hit.transform.gameObject);

                    CreateTarget();
                }
                else if("ExitTarget".Equals(hit.transform.name))
                {
                    SceneManager.LoadScene(SceneName);
                }
            }
        }
        else
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    private  void CreateTarget()
    {
        var primitive = GameObject.Instantiate(Target);
        primitive.transform.position =
            Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 2f))*0.5f;
        primitive.name = "Target";
    }

    private void _gestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        OnClickHandler(headRay);
    }


    private void SetLineObjects(Vector3 viewportToWorldPoint, GameObject[] markObjects, int dirX, int dirY)
    {
        viewportToWorldPoint = new Vector3(viewportToWorldPoint.x, viewportToWorldPoint.y, viewportToWorldPoint.z);
        viewportToWorldPoint = Camera.main.transform.InverseTransformPoint(viewportToWorldPoint);
        var pos2 = viewportToWorldPoint * 1f / (marksCount-1);
        for (var i = 1; i <= marksCount; i++)
        {
            var transform1 = markObjects[i - 1].transform;
            transform1.localPosition = new Vector3(pos2.x * i + transform1.localScale.x / 2f * dirX,
                pos2.y * i + transform1.localScale.y / 2f * dirY, transform1.localScale.z * i);
            transform1.position += Camera.main.transform.forward * 2f;
            transform1.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }
}