using System;

namespace Objects.Characters
{
	public class CharacterAttribute : Attribute
	{
		public string Name { get; protected set; }
		
		public CharacterAttribute(string name)
		{
			Name = name;
		}
	}
	
	public static class CharacterValueExtension
	{
		public static string GetName(this CharactersEnum charactersEnum)
		{
			var fieldInfo = charactersEnum.GetType().GetField(charactersEnum.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(CharacterAttribute), false) as CharacterAttribute[];
			return attributes?.Length > 0 ? attributes[0].Name : null;
		}
	}
}