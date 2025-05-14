using System;
using System.Collections.Generic;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Rogue
{
    public enum Race
    {
        Human,
        Elf,
        Orc
    }
    public enum Class
    {
        Rogue,
        Warrior,
        Mage
    }
    public class PlayerCharacter
    {
        public string name;
        public Race race;
        public Class Role;
        public int HP = 50;
        public int MaxHP = 50;

        public Point2D position;
        public Point2D LastPosition;
        public int NextPosition;
        public int[] MapTiles;
        public int Numero;
        public int MapWidth;

        public Texture image;
        float imagePixelX;
        float imagePixelY;
        public Rectangle imageRect;

        public void Move(int x_move, int y_move)
        {
            position.x += x_move;
            position.y += y_move;
            NextPosition = position.x + position.y * MapWidth;
            Numero = MapTiles[NextPosition];
            int tileId = Numero;
            List<int> FloorTileNumbers = new List<int> { 52, 51, 49, 50, 31, 40, 43, 34, 53 };
            if (FloorTileNumbers.Contains(tileId))
            {
                LastPosition.x = position.x - x_move;
                LastPosition.y = position.y - y_move;
                position.x = Math.Clamp(position.x, 0, Console.WindowWidth - 1);
                position.y = Math.Clamp(position.y, 0, Console.WindowHeight - 1);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Raylib.PlaySound(Game.WallCollide);
                position.x -= x_move;
                position.y -= y_move;
            }
        }

        public void Draw()
        {
            Vector2 vector = new Vector2(position.x, position.y);
            Raylib.DrawTextureRec(image, imageRect, vector * Game.tileSize, Raylib.WHITE);
        }

        public void SetImageAndIndex(Texture atlasImage, int imagesPerRow, int index)
        {
            float tileSize = 16f;
            image = atlasImage;
            imagePixelX = (index % imagesPerRow) * tileSize;
            imagePixelY = (int)(index / imagesPerRow) * tileSize;

            imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
        }

        public int Attack()
        {
            Random rand = new Random();
            return rand.Next(5, 11);
        }
    }
}