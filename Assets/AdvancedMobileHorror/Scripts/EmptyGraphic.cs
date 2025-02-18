using UnityEngine.UI;

namespace AdvancedHorrorFPS
{
	public class EmptyGraphic : Graphic
	{
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
