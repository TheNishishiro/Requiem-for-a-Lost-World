using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Managers
{
	public class BlurManager : MonoBehaviour
	{
		public void Blur()
		{
			var volume = FindAnyObjectByType<Volume>(FindObjectsInactive.Include);
			volume.profile.TryGet(out DepthOfField dof);
			if (dof == null) return;
			
			dof.active = true;
		}
		
		public void DeBlur()
		{
			var volume = FindAnyObjectByType<Volume>(FindObjectsInactive.Include);
			volume.profile.TryGet(out DepthOfField dof);
			if (dof == null) return;
			
			dof.active = false;
		}
	}
}