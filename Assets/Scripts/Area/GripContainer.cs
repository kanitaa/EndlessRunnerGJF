using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripContainer : MonoBehaviour
{
    [SerializeField] private Area gripArea;

    private PlayerMovement _movement;

    [SerializeField] private GameObject[] _gripTriggers;
    private GameObject _gripEnd;

    [SerializeField] private List<Grip> _grips = new();

    private bool _checkSequence = false;

    [SerializeField] private List<KeyCode> _gripKeyOrder = new();

    private bool _reverseOrder = false;

    // Use this if you want the user to press the series of keys in a given times interval
    public float DelayBeforeFail;

    // The index in the array of the next key to press in order to continue the series
    private int _gripIndex;

    // The time (in seconds) the last correct key has been pressed
    private float lastKeyPressTime;


    private void Start()
    {
        foreach(Grip grip in _grips)
        {
            _gripKeyOrder.Add(grip.KeyCode);
        }
    }

    public void StartGripSequence(PlayerMovement movement, GameObject startTrigger)
    {
        _movement = movement;
        if (startTrigger == _gripTriggers[0])
        {
            _gripEnd = _gripTriggers[1];
            _gripIndex = 0;

        } 
        else
        {
            _gripEnd = _gripTriggers[0];
            _gripIndex = _grips.Count-1;
            _reverseOrder = true;
        }
        

        Debug.Log("Player entered start area");
        _checkSequence = true;
        lastKeyPressTime = Time.time;
        
        StartCoroutine(GripSequence());
    }

    IEnumerator GripSequence()
    {
        while (_checkSequence)
        {
            // Make sure some keys have been specified in the inspector 
            if (_gripKeyOrder.Count == 0)
            {
                Debug.Log("No grips in container!");
                _checkSequence = false;
                yield return null;
            }
            // Check if the user pressed the key before the end of the timer
            if (Time.time - lastKeyPressTime > DelayBeforeFail)
            {
          
                _checkSequence = false;
                _movement.MoveToGrip(_movement.gameObject.transform, true);
                Debug.Log("Timer ran out");
            }

            // Correct key pressed!
            if (Input.GetKeyDown(_gripKeyOrder[_gripIndex]))
            {
                _movement.MoveToGrip(_grips[_gripIndex].gameObject.transform);
                
                lastKeyPressTime = Time.time;

                if (!_reverseOrder)
                    _gripIndex++;
                else 
                    _gripIndex--;

                // Series completed!
                if (!_reverseOrder && _gripIndex >= _gripKeyOrder.Count || _reverseOrder && _gripIndex <= -1)
                {
                    yield return new WaitForSeconds(_movement.GripSpeed);
                    _movement.MoveToGrip(_gripEnd.transform);
                    _movement.Walk();

                    _checkSequence = false;

                    yield return new WaitForSeconds(2);
                    _movement.GripSequenceStarted = false;
                }
            }
            // Wrong key pressed!
            else if (Input.anyKeyDown)
            {

                KeyCode keyPressed = KeyCode.None;

                if (Input.GetKeyDown(KeyCode.Q)) keyPressed = KeyCode.Q;
                else if (Input.GetKeyDown(KeyCode.W)) keyPressed = KeyCode.W;
                else if (Input.GetKeyDown(KeyCode.E)) keyPressed = KeyCode.E;
          

                if (keyPressed != KeyCode.None)
                {
                    for (int i = _gripIndex; i < _grips.Count; i++)
                    {
                        if (_grips[i].KeyCode == keyPressed)
                        {
                            _movement.MoveToGrip(_grips[i].gameObject.transform, true);
                            break;
                        }
                    }
                }
               

                _checkSequence = false;

            }
            yield return null;
        }
    }
}
