using System.Collections;
using UnityEditor.TextCore.Text;
using UnityEngine;


public class Monster : MonoBehaviour
{
    [SerializeField] private Transform _startPoint, _endPoint;

    [SerializeField] private float _movementSpeed;

    [SerializeField] private Transform _player;

    [SerializeField] private GameObject _endTrigger;

    [SerializeField] private Transform _bounceEnd;
    public Transform BounceEnd { get => _bounceEnd; }

    private MonsterState _myState;

    private Animator _anim;
    private bool _isAwakened = false;

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
        if (!_isAwakened && Vector3.Distance(transform.position, _player.position) < 28f)
        {
            _anim.SetTrigger("Buff");
            _endTrigger.SetActive(true);
            _isAwakened = true;
            
        }
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
            _myState = MonsterState.ResetChase;
            _anim.SetBool("Run Forward", false);
            _anim.SetBool("Stunned Loop", true);
        }
           
    }

    private IEnumerator ResetChase()
    {
        yield return new WaitForSeconds(0.5f);
        _player.GetComponent<PlayerMovement>().MonsterChase(false);

        yield return new WaitForSeconds(30f);
        _myState = MonsterState.Wait;
        transform.position = _startPoint.position;
        _endTrigger.SetActive(false);
        _anim.SetBool("Stunned Loop", false);

    }
}

enum MonsterState
{
    Wait,
    Chase,
    ResetChase
}
