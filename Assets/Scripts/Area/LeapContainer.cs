using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapContainer : MonoBehaviour
{
    [SerializeField] private Area _leapArea;

    private PlayerMovement _movement;

    [SerializeField] private GameObject[] _leapTriggers;
    private GameObject _leapEnd;

    [SerializeField] private List<Leap> _leaps = new();

    private bool _checkSequence = false;

    private bool _firstMove = true;

    [SerializeField] private List<KeyCode> _leapKeyOrder = new();

    private bool _reverseOrder = false;

    // Use this if you want the user to press the series of keys in a given times interval
    public float DelayBeforeFail;

    // The index in the array of the next key to press in order to continue the series
    private int _leapIndex;

    // The time (in seconds) the last correct key has been pressed
    private float lastKeyPressTime;


    private void Start()
    {
        foreach(Leap leap in _leaps)
        {
            _leapKeyOrder.Add(leap.KeyCode);
        }
    }

    public void StartLeapSequence(PlayerMovement movement, GameObject startTrigger)
    {
        _movement = movement;
        if (startTrigger == _leapTriggers[0])
        {
            _leapEnd = _leapTriggers[1];
            _leapIndex = 0;
            _reverseOrder = false;

        } 
        else
        {
            _leapEnd = _leapTriggers[0];
            _leapIndex = _leaps.Count-1;
            _reverseOrder = true;
        }
        

        Debug.Log("Player entered grip trigger area");
 
        _checkSequence = true;
        _firstMove = true;
        lastKeyPressTime = Time.time;

        _leaps[_leapIndex].EnableGlow(true);
        _leapEnd.gameObject.SetActive(false);
        StartCoroutine(LeapSequence());
    }

    IEnumerator LeapSequence()
    {
        while (_checkSequence)
        {
            // Make sure some keys have been specified in the inspector 
            if (_leapKeyOrder.Count == 0)
            {
                Debug.Log("No grips in container!");
                _checkSequence = false;
                yield return null;
            }
            // Check if the user pressed the key before the end of the timer
            if (Time.time - lastKeyPressTime > DelayBeforeFail)
            {
                _checkSequence = false;
            
                   
                Debug.Log("Timer ran out");
            }

            // Correct key pressed!
            if (Input.GetKeyDown(_leapKeyOrder[_leapIndex]))
            {
                _movement.MoveToPosition(_leaps[_leapIndex].gameObject.transform.position);
                
                lastKeyPressTime = Time.time;

                _leaps[_leapIndex].EnableGlow(false);

                _firstMove = false;

                if (!_reverseOrder)
                    _leapIndex++;
                else 
                    _leapIndex--;

               

                // Series completed!
                if (!_reverseOrder && _leapIndex >= _leapKeyOrder.Count || _reverseOrder && _leapIndex <= -1)
                {
                    yield return new WaitForSeconds(_movement.GripSpeed);
                    _movement.MoveToPosition(_leapEnd.transform.position);

                    _checkSequence = false;

                   

                   // yield return new WaitForSeconds(2);
                    _movement.IsLeaping = false;
                  
                    yield return new WaitForSeconds(5);

                    _leapEnd.gameObject.SetActive(true);
                   

                }
                else
                {
                    _leaps[_leapIndex].EnableGlow(true);
                   
                }
            }
            // Wrong key pressed!
            else if (Input.anyKeyDown)
            {
                if(_firstMove && Input.GetKeyDown(KeyCode.A) || _firstMove && Input.GetKeyDown(KeyCode.D))
                {
                    //Allow player to do horizontal movement when they havent started leaping but have entered leap start area.
                }
                else
                {
                    KeyCode keyPressed = KeyCode.None;

                    if (Input.GetKeyDown(KeyCode.Q)) keyPressed = KeyCode.Q;
                    else if (Input.GetKeyDown(KeyCode.W)) keyPressed = KeyCode.W;
                    else if (Input.GetKeyDown(KeyCode.E)) keyPressed = KeyCode.E;


                    if (keyPressed != KeyCode.None)
                    {
                        for (int i = _leapIndex; i < _leaps.Count; i++)
                        {
                            if (_leaps[i].KeyCode == keyPressed)
                            {
                                _movement.MoveToPosition(_leaps[i].gameObject.transform.position, true);
                                break;
                            }
                        }
                    }


                    _checkSequence = false;


                }
            }
            yield return null;
        }
    }
}
