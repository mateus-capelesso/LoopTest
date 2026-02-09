using UnityEngine;
using Newtonsoft.Json;

namespace _Project.Scripts.Game
{
	public class GamePersistentData
	{
		public GameModel RestoreGameModel(string userKey)
		{
			if (PlayerPrefs.HasKey(userKey))
			{
				var json = PlayerPrefs.GetString(userKey);
				var dto =  JsonConvert.DeserializeObject<GameDTO>(json);
				return new GameModel(dto.score, dto.level);
			}
			else
			{
				return new GameModel();
			}
		}

		public void SaveGameModel(string userKey, GameModel model)
		{
			var dto = new GameDTO
			{
				score = model.Score,
				level = model.MaxLevel
			};
			PlayerPrefs.SetString(userKey, JsonConvert.SerializeObject(dto));
		}
	}
}