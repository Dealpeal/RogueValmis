using System;
using ZeroElectric.Vinculum;
using RayGuiCreator;
using System.Numerics;

namespace Rogue
{
    public class BattleScreen
    {
        public event EventHandler BattleEndedEvent;
        private Map.Enemy currentEnemy;
        private string lastAction = "Taistelu alkoi!";
        private bool playerTurn = true;

        public void StartBattle(Map.Enemy enemy)
        {
            currentEnemy = enemy;
            lastAction = $"Kohtasit vihollisen: {enemy.name}!";
            playerTurn = true;
        }

        public void DrawBattleScreen(PlayerCharacter player, Map map)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            RayGui.GuiSetStyle(0, (int)GuiDefaultProperty.TEXT_SIZE, 20);

            int width = 300;
            int x = Raylib.GetScreenWidth() / 2 - width / 2;
            int y = 40;
            MenuCreator c = new MenuCreator(x, y, width);

            DrawCharacterSprite(player, new Vector2(250, 100), 16);
            DrawEnemySprite(currentEnemy, new Vector2(Raylib.GetScreenWidth() - 500, 100), 16);

            c.Label($"Pelaaja: {player.HP}/{player.MaxHP} HP");
            c.Label($"{currentEnemy.name}: {currentEnemy.HP}/{currentEnemy.MaxHP} HP");

            if (playerTurn && c.LabelButton("Hyökkää"))
            {
                int damage = player.Attack();
                currentEnemy.HP -= damage;
                lastAction = $"Pelaaja hyökkäsi ja teki {damage} vahinkoa!";
                playerTurn = false;
            }

            c.Label(lastAction);

            if (!playerTurn)
            {
                int damage = currentEnemy.Attack();
                player.HP -= damage;
                lastAction = $"{currentEnemy.name} hyökkäsi ja teki {damage} vahinkoa!";
                playerTurn = true;
            }

            if (currentEnemy.HP <= 0)
            {
                lastAction = $"Voitit vihollisen {currentEnemy.name}!";
                map.RemoveEnemy(currentEnemy);
                BattleEndedEvent?.Invoke(this, EventArgs.Empty);
            }
            else if (player.HP <= 0)
            {
                lastAction = "Hävisit taistelun!";
                BattleEndedEvent?.Invoke(this, EventArgs.Empty);
            }

            c.EndMenu();

            RayGui.GuiSetStyle(0, (int)GuiDefaultProperty.TEXT_SIZE, 10);

            Raylib.EndDrawing();
        }

        private void DrawCharacterSprite(PlayerCharacter player, Vector2 position, float scale)
        {
            Rectangle sourceRect = player.imageRect;
            Rectangle destRect = new Rectangle(
                position.X, position.Y,
                player.imageRect.width * scale, player.imageRect.height * scale);
            Raylib.DrawTexturePro(player.image, sourceRect, destRect, Vector2.Zero, 0f, Raylib.WHITE);
        }

        private void DrawEnemySprite(Map.Enemy enemy, Vector2 position, float scale)
        {
            Rectangle sourceRect = new Rectangle(
                (enemy.AtlasIndex % 12) * Game.tileSize,
                (enemy.AtlasIndex / 12) * Game.tileSize,
                Game.tileSize, Game.tileSize);
            Rectangle destRect = new Rectangle(
                position.X, position.Y,
                Game.tileSize * scale, Game.tileSize * scale);
            Raylib.DrawTexturePro(enemy.EnemySprite, sourceRect, destRect, Vector2.Zero, 0f, Raylib.WHITE);
        }
    }
}