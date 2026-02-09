using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View.Components
{
	public class LevelButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
		[SerializeField] private TMP_Text _levelText;
		[SerializeField] private Image _lockIcon;

		private LevelsView _levelsView;
		private int _levelIndex;
		private bool _isLocked;
		
		public void SetupButton(LevelsView view, int index)
		{
			_levelsView = view;
			_levelIndex = index;
			_levelText.text = index.ToString();
			_button.onClick.AddListener(ButtonClickHandler);
		}
		
		public void SetLock(bool isLocked)
		{
			_isLocked = isLocked;

			if (_isLocked)
			{
				_lockIcon.gameObject.SetActive(true);
				_button.interactable = false;
			}
			else
			{
				_lockIcon.gameObject.SetActive(false);
				_button.interactable = true;
			}
		}
		
		public void ButtonClickHandler()
		{
			_levelsView.LevelSelectedHandler(_levelIndex);
		}
	}
}