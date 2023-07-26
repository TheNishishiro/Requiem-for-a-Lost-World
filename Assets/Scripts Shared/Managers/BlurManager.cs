using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Managers
{
	public class BlurManager : MonoBehaviour
	{
		public void Blur()
		{
			var volume = FindObjectOfType<Volume>();
			volume.profile.TryGet(out DepthOfField _dof);
			_dof.active = true;
		}
		
		public void DeBlur()
		{
			var volume = FindObjectOfType<Volume>();
			volume.profile.TryGet(out DepthOfField _dof);
			_dof.active = false;
		}
	}
}