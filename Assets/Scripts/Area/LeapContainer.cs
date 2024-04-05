using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapContainer : MonoBehaviour
{
    private PlayerMovement _movement;
    //Leap start and end locations.
    [SerializeField] private GameObject[] _leapTriggers;
    
    private GameObject _leapEnd;
    //Leap objects in the leap sequence.
    [SerializeField] private List<Leap> _leaps = new();
    //Keycode order of leap objects.
    [SerializeField] private List<KeyCode> _leapKeyOrder = new();

    private bool _checkSequence = false;
    //If sequence has started but player still needs to dodge objects with A/D, don't fail sequence with those keybinds.
    private bool _firstMove = true;

    
    //For monster chase player needs to cross the leap container in reverse order.
    private bool _reverseOrder = false;

    //The time player has to press correct key in sequence.
    [SerializeField] private float _delayBeforeFail;

    //The time when last correct key was pressed.
    private float _lastKeyPressTime;

    //Current leap index in the leap sequence.
    private int _leapIndex;

  


    private void Start()
    {
        //Add correct keyorder to keyorder list.
        foreach(Leap leap in _leaps)
        {
            _leapKeyOrder.Add(leap.KeyCode);
        }
    }

    public void StartLeapSequence(PlayerMovement movement, GameObject startTrigger)
    {
        _movement = movement;
        //Check which side player enters the leap container for initializing variables.
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
        _lastKeyPressTime = Time.time;

        _leaps[_leapIndex].EnableGlow(true);
        _leapEnd.gameObject.SetActive(false);

        StartCoroutine(LeapSequence());
    }

    IEnumerator LeapSequence()
    {
        while (_checkSequence)
        {
            //Ensure there are keycodes to compare to.
            if (_leapKeyOrder.Count == 0)
            {
                _checkSequence = false;
                Debug.Log("No leap objects assigned in leap container!");
                yield return null;
            }
            //If player hasn't pressed any key fast enough.
            if (Time.time - _lastKeyPressTime > _delayBeforeFail)
            {
                _checkSequence = false;
                Debug.Log("Leap timer ran out!");
            }

            //Correct key pressed.
            if (Input.GetKeyDown(_leapKeyOrder[_leapIndex]))
            {
                //Move player to leap object's position.
                _movement.MoveToPosition(_leaps[_leapIndex].gameObject.transform.position);
                
                _lastKeyPressTime = Time.time;

                _leaps[_leapIndex].EnableGlow(false);
                _firstMove = false;

                //Leap index depends on which way player is going.
                if (!_reverseOrder)
                {
                    _leapIndex++;
                }  
                else
                {
                    _leapIndex--;
                }

                //Sequence completed.
                if (!_reverseOrder && _leapIndex >= _leapKeyOrder.Count || _reverseOrder && _leapIndex <= -1)
                {
                    //Wait for player to finish moving and then move to leap end position.
                    yield return new WaitForSeconds(_movement.GripSpeed);
                    _movement.MoveToPosition(_leapEnd.transform.position);

                    _checkSequence = false;
                    _movement.IsLeaping = false;
                  
                    yield return new WaitForSeconds(5);
                    //Player isn't near leap end object anymore, enable it so it can be triggered if player comes back.
                    _leapEnd.gameObject.SetActive(true);
                   

                }
                else
                {
                    //Glow up the next leap object for player clue.
                    _leaps[_leapIndex].EnableGlow(true);
                   
                }
            }
            //Wrong key pressed.
            else if (Input.anyKeyDown)
            {
                if(_firstMove && Input.GetKeyDown(KeyCode.A) || _firstMove && Input.GetKeyDown(KeyCode.D))
                {
                    //Allow player to do horizontal movement when they haven't started leaping but have entered leap start area.
                }
                else
                {
                    KeyCode keyPressed = KeyCode.None;

                    if (Input.GetKeyDown(KeyCode.Q)) keyPressed = KeyCode.Q;
                    else if (Input.GetKeyDown(KeyCode.E)) keyPressed = KeyCode.E;

                    //If player pressed wrong keycode from possible keycodes, move player a bit towards the wrong leap object before fail.
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
