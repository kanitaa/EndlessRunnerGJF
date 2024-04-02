using System.Collections;
using UnityEngine;


public class Monster : MonoBehaviour
{
    private enum MonsterState
    {
        Wait,
        Chase,
        ResetChase
    }
    private MonsterState _myState;
    private bool _isAwakened = false;

    private Animator _anim;

    [SerializeField] private Transform _startPoint, _endPoint;

    [SerializeField] private float _movementSpeed;

    [SerializeField] private Transform _player;

    [SerializeField] private GameObject _endTrigger;

    [SerializeField] private Transform _bounceEnd;
    public Transform BounceEnd { get => _bounceEnd; }


    private void Start()
    {
        _anim = GetComponent<Animator>();
        _myState = MonsterState.Wait;
        StartCoroutine(CheckMonsterState());
    }

    private IEnumerator CheckMonsterState()
    {
        while (true)
        {
            switch (_myState)
            {
                case MonsterState.Wait:
                    WaitForPlayer();
                    break;
                case MonsterState.Chase:
                    ChasePlayer();
                    break;
                case MonsterState.ResetChase:
                    yield return StartCoroutine(ResetChase());
                    break;
                default:
                    break;
            }
            yield return null;
        }
    }

    private void WaitForPlayer()
    {
        //Awaken the monster when player is nearby and play the animation.
        if (!_isAwakened && Vector3.Distance(transform.position, _player.position) < 28f)
        {
            _anim.SetTrigger("Buff");
            _endTrigger.SetActive(true);
            _isAwakened = true;
            
        }
        //Enable chase.
        if (Vector3.Distance(transform.position, _player.position) < 20f)
        {
            _myState = MonsterState.Chase;
            _player.GetComponent<PlayerMovement>().MonsterChase(true);
            _anim.SetBool("Run Forward", true);
        }
            
    }

    private void ChasePlayer()
    {
        float step = _movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _endPoint.position, step);

        if (Vector3.Distance(transform.position, _endPoint.position) < 0.001f)
        {
            _anim.SetBool("Run Forward", false);
            _anim.SetBool("Stunned Loop", true);
            _myState = MonsterState.ResetChase;
        }
           
    }

    private IEnumerator ResetChase()
    {
        yield return new WaitForSeconds(0.5f);
        _player.GetComponent<PlayerMovement>().MonsterChase(false);

        //Reset monsters location back to start.
        yield return new WaitForSeconds(25f);
        _myState = MonsterState.Wait;
        transform.position = _startPoint.position;
        _endTrigger.SetActive(false);
        _anim.SetBool("Stunned Loop", false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _myState == MonsterState.Chase)
        {
            _myState = MonsterState.ResetChase;
            _anim.SetBool("Run Forward", false);
            _anim.SetBool("Stunned Loop", true);
        }
    }
}

