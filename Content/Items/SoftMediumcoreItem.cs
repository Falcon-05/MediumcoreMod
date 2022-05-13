using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MediumcoreMod.Content.Items
{
	public class SoftMediumcoreItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soft Mediumcore Enabler");
			Tooltip.SetDefault("Turns on Soft Mediumcore mode for your character, making you drop your inventory and all of your coins on death (NOT anything in your hotbar, equipment slots, or ammo slots!)" +
				"\nTHIS EFFECT IS IRREVERSIBLE WITHOUT CHEATS");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			return !player.GetModPlayer<MediumcorePlayer>().SoftMediumcore;
		}

		public override bool? UseItem(Player player)
		{
			player.GetModPlayer<MediumcorePlayer>().SoftMediumcore = true;
			SoundEngine.PlaySound(SoundID.Roar, player.Center, 0);

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText($"{player.name} is now in Soft Mediumcore mode!", Color.Yellow);
			}
			else if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} is now in Soft Mediumcore mode!"), Color.Yellow);
			}

			return true;
		}
	}
}