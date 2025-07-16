using TMPro;
using UnityEngine;

public class TextTyperLoop3D : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private string fullText = "Hello, looping world!";
    [SerializeField] private float letterDelay = 0.1f;
    [SerializeField] private float pauseDuration = 1.5f;
    [SerializeField] private float randomStartDelayMin = 0f;
    [SerializeField] private float randomStartDelayMax = 1f;

    private float _timer;
    private int _charIndex;
    private float _startDelay;
    private bool _typingStarted;
    private bool _finishedTyping;
    private float _pauseTimer;

    private void Awake()
    {
        if (!textMesh)
            textMesh = GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        ResetTyping();
    }

    private void Update()
    {
        if (!_typingStarted)
        {
            _startDelay -= Time.deltaTime;
            if (_startDelay <= 0f)
            {
                _typingStarted = true;
                _timer = 0f;
            }
            return;
        }

        if (_finishedTyping)
        {
            _pauseTimer += Time.deltaTime;
            if (_pauseTimer >= pauseDuration)
            {
                ResetTyping();
            }
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= letterDelay)
        {
            _timer = 0f;
            _charIndex++;

            if (_charIndex > fullText.Length)
            {
                _charIndex = fullText.Length;
                _finishedTyping = true;
                _pauseTimer = 0f;
            }

            textMesh.text = fullText.Substring(0, _charIndex);
        }
    }

    private void ResetTyping()
    {
        _charIndex = 0;
        _typingStarted = false;
        _finishedTyping = false;
        textMesh.text = "";
        _startDelay = Random.Range(randomStartDelayMin, randomStartDelayMax);
    }
}
