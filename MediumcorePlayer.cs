using MediumcoreMod.Content.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System.Linq;

namespace MediumcoreMod
{
	public class MediumcorePlayer : ModPlayer
	{
		public bool SoftMediumcore = false;

		public override void SaveData(TagCompound tag)
		{
			tag.Add("SoftMediumcore", SoftMediumcore);
		}

		public override void LoadData(TagCompound tag)
		{
			SoftMediumcore = tag.GetBool("SoftMediumcore");
		}

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath && (Player.difficulty == 0 || Player.difficulty == 3))
			{
				return new[]
				{
					new Item(ModContent.ItemType<SoftMediumcoreItem>()),
					new Item(ModContent.ItemType<SoftMediumcoreIndicator>())
				};
			}
			return Enumerable.Empty<Item>();
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if ((Player.difficulty == 0 || Player.difficulty == 3) && !pvp && SoftMediumcore && Player.whoAmI == Main.myPlayer)
			{
				DropAllCoins();
				DropInventory();
			}
		}

		//Copy+pasted from vanilla code, except that you always drop ALL of your coins
		public int DropAllCoins()
		{
			IEntitySource itemSource_Death = Player.GetSource_Death();
			int num = 0;
			for (int i = 0; i < 59; i++)
			{
				if (Player.inventory[i].IsACoin)
				{
					int num2 = Item.NewItem(itemSource_Death, (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type);
					int num3 = 0;
					num3 = Player.inventory[i].stack - num3;
					Player.inventory[i].stack -= num3;
					if (Player.inventory[i].type == ItemID.CopperCoin)
					{
						num += num3;
					}
					if (Player.inventory[i].type == ItemID.SilverCoin)
					{
						num += num3 * 100;
					}
					if (Player.inventory[i].type == ItemID.GoldCoin)
					{
						num += num3 * 10000;
					}
					if (Player.inventory[i].type == ItemID.PlatinumCoin)
					{
						num += num3 * 1000000;
					}
					if (Player.inventory[i].stack <= 0)
					{
						Player.inventory[i] = new Item();
					}
					Main.item[num2].stack = num3;
					Main.item[num2].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
					Main.item[num2].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
					Main.item[num2].noGrabDelay = 100;
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num2);
					}
					if (i == 58)
					{
						Main.mouseItem = Player.inventory[i].Clone();
					}
				}
			}
			if (!Main.masterMode)
			{
				Player.lostCoins += num;
			}
			else
			{
				Player.lostCoins = num;
			}
			Player.lostCoinString = Main.ValueToCoins(Player.lostCoins);
			return num;
		}

		public void DropInventory()
		{
			IEntitySource itemSource_Death = Player.GetSource_Death();
			//Slots 0-9 are the hotbar, so skip them
			//Slots 10-49 are the inventory, and slots 50-53 are the coin slots
			//Slot 58 is the item being held by your cursor
			for (int i = 10; i < 59; i++)
			{
				if (i < 54 || i > 57) //Slots 54-57 are the ammo slots, so skip them
				{
					if (Player.inventory[i].stack > 0)
					{
						int num = Item.NewItem(itemSource_Death, (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type);
						Main.item[num].netDefaults(Player.inventory[i].netID);
						Main.item[num].Prefix(Player.inventory[i].prefix);
						Main.item[num].stack = Player.inventory[i].stack;
						Main.item[num].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
						Main.item[num].velocity.X = (float)Main.rand.Next(-20, 21) * 0.2f;
						Main.item[num].noGrabDelay = 100;
						Main.item[num].newAndShiny = false;
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num);
						}
					}
					Player.inventory[i].TurnToAir();
				}
			}
			Player.inventory[10].SetDefaults(ItemID.CopperShortsword);
			Player.inventory[10].Prefix(-1);
			Player.inventory[11].SetDefaults(ItemID.CopperPickaxe);
			Player.inventory[11].Prefix(-1);
			Player.inventory[12].SetDefaults(ItemID.CopperAxe);
			Player.inventory[12].Prefix(-1);
			Main.mouseItem.TurnToAir();
		}
	}
}