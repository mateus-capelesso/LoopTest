using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Audio
{
	[CreateAssetMenu(fileName = "AudioConfig", menuName = "InfinityLoop/Audio", order = 0)]
	public class AudioConfig : ScriptableObject
	{
		[SerializeField] private List<AudioItem> _audioItems;
		
		public AudioClip GetAudioClipByType(AudioType type)
		{
			return _audioItems.FirstOrDefault(audioItem => audioItem.type == type).clip;
		}
	}
	
	[Serializable]
	public struct AudioItem
	{
		public AudioType type;
		public AudioClip clip;
	}
}