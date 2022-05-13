using Terraria.ModLoader;

namespace MediumcoreMod
{
	public class MediumcoreMod : Mod
	{
		internal static MediumcoreMod Instance;

		public override void Load()
		{
			Instance = this;
		}

		public override void Unload()
		{
			Instance = null;
		}
	}
}