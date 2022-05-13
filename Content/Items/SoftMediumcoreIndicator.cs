using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace MediumcoreMod.Content.Items
{
	public class SoftMediumcoreIndicator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soft Mediumcore Indicator");
			Tooltip.SetDefault("Using this tells you whether Soft Mediumcore mode is on for your character" +
				"\nUseful for if you're returning to this character after a long break and may have forgotten about this mod");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item4;
			Item.rare = ItemRarityID.Blue;
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				string text = player.GetModPlayer<MediumcorePlayer>().SoftMediumcore ? "on." : "off.";
				Main.NewText("Soft Mediumcore is currently " + text, Color.Yellow);
			}
			return true;
		}
	}
}