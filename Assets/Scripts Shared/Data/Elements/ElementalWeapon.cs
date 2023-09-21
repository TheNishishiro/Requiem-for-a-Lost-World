using Weapons;

namespace Data.Elements
{
	public class ElementalWeapon : WeaponBase
	{
		public ElementalWeapon(Element skillElement)
		{
			element = skillElement;
			isSkill = true;
		}
		
		public override void Attack()
		{
			throw new System.NotImplementedException();
		}
	}
}