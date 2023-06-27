using System;

namespace Objects.Characters
{
	public class CharacterNameAttribute : Attribute
	{
		public string Name { get; protected set; }
		
		public CharacterNameAttribute(string name)
		{
			Name = name;
		}
	}
	
	public static class AchievementValueExtension
	{
		public static string GetName(this CharactersEnum charactersEnum)
		{
			var fieldInfo = charactersEnum.GetType().GetField(charactersEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(CharacterNameAttribute), false) as CharacterNameAttribute[];
			return attributes?.Length > 0 ? attributes[0].Name : null;
		}
	}
}