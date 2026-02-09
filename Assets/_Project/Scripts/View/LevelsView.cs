using System.Collections.Generic;
using _Project.Scripts.Level;
using _Project.Scripts.View.Components;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View
{
	public class LevelsView: BaseView
	{
		[SerializeField] private GridLayoutGroup _gridLayoutGroup;
		[SerializeField] private LevelButton _levelButtonPrefab;
		[SerializeField] private Button _backButton;

		
		private List<LevelButton> _instantiatedButtons;
		
		public override void EnterView()
		{
			SetupLevelGrid();
			InstantiateLevelButtons();
			
			_backButton.onClick.AddListener(() => _stateMachine.ChangeState(ViewState.MainMenu));
			
			base.EnterView();
		}

		public override void ExitView()
		{
			_backButton.onClick.RemoveAllListeners();
			
			ClearLevelButtons();
			
			base.ExitView();
		}

		public void LevelSelectedHandler(int level)
		{
			_stateMachine.LevelSelectedHandler(level);
		}

		private void SetupLevelGrid()
		{
			var gridRect = _gridLayoutGroup.GetComponent<RectTransform>();
			float totalWidth = gridRect.rect.width;

			int columns = 3;
			float horizontalSpacing = 25f;
			float verticalSpacing = 25f;

			float availableWidth = totalWidth - (horizontalSpacing * (columns - 1));
			float cellSize = availableWidth / columns;

			_gridLayoutGroup.spacing = new Vector2(horizontalSpacing, verticalSpacing);
			_gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
			_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			_gridLayoutGroup.constraintCount = columns;
		}

		private void InstantiateLevelButtons()
		{
			_instantiatedButtons = new List<LevelButton>();
			var availableLevels = _stateMachine.AvailableLevels;
			var maxLevel = _stateMachine.MaxLevel;

			for (int i = 0; i < availableLevels.Length; i++)
			{
				var level = availableLevels[i];
				var button = InstantiateLevelButton(i, level);
				button.SetLock(i > maxLevel);
				
				_instantiatedButtons.Add(button);
			}
		}
		
		private LevelButton InstantiateLevelButton(int index, LevelData level)
		{
			var button = Instantiate(_levelButtonPrefab, _gridLayoutGroup.transform);
			button.SetupButton(this, index, level.LevelName);

			return button;
		}

		private void ClearLevelButtons()
		{
			foreach (var button in _instantiatedButtons)
			{
				Destroy(button.gameObject);
			}
			
			_instantiatedButtons.Clear();
		}
	}
}