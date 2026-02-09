using UnityEngine;

namespace _Project.Scripts.Audio
{
	public class AudioController : MonoBehaviour
	{
		private static AudioController _instance;
		public static AudioController Instance => _instance;
		
		[SerializeField] private AudioConfig _audioConfig;
		[SerializeField] private AudioSource _backgroundAudioSource;
		[SerializeField] private AudioSource _soundEffectAudioSource;
		
		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}

			_instance = this;
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			SetupBackgroundAudioSource();
		}

		public void PlaySoundEffect(AudioType type)
		{
			var clip = _audioConfig.GetAudioClipByType(type);

			_soundEffectAudioSource.clip = clip;
			_soundEffectAudioSource.loop = false;
			_soundEffectAudioSource.Play();
			
		}

		private void SetupBackgroundAudioSource()
		{
			_backgroundAudioSource.clip = _audioConfig.GetAudioClipByType(AudioType.Background);
			_backgroundAudioSource.loop = true;
			_backgroundAudioSource.Play();
		}
		
	}

	
}