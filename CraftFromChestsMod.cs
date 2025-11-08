using Allumeria;
using Allumeria.Blocks.BlockEntities;
using Allumeria.Blocks.Blocks;
using Allumeria.ChunkManagement;
using Allumeria.EntitySystem.Entities;
using Allumeria.UI.Containers;
using HarmonyLib;
using Ignitron.Loader;
using OpenTK.Mathematics;

namespace CraftFromChests
{
    public class CraftFromChestsMod : IModEntrypoint
    {
        public void Main(ModBox box)
        {
            Logger.Info("Installed CFC");
            Harmony harmony = new("Trigonaut.CFC");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CraftingContainer))]
    [HarmonyPatch("SetToPlayerInventory")]
    public class CraftingContainer_SetToPlayerInventory_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(CraftingContainer __instance)
        {
            Logger.Info("Adding nearby chests to crafting inventory");
            World world = Game.worldManager.world;
            PlayerEntity player = World.player;
            int num = 10;
            Vector3i vector3i = new Vector3i((int)MathF.Floor(player.position.X), (int)MathF.Floor(player.position.Y), (int)MathF.Floor(player.position.Z));
            for (int i = vector3i.X - num; i < vector3i.X + num; i++)
            {
                for (int j = vector3i.Y - num; j < vector3i.Y + num; j++)
                {
                    for (int k = vector3i.Z - num; k < vector3i.Z + num; k++)
                    {
                        if (world.chunkManager.GetBlock(i, j, k) is BlockChest && world.chunkManager.GetBlockEntityAt(i, j, k, out BlockEntity entity) && entity is BlockEntityChest blockEntityChest)
                        {
                            __instance.AddInventory(blockEntityChest.inventory);
                        }
                    }
                }
            }
            __instance.Refresh();
        }
    }
}
