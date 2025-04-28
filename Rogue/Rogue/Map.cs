using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;
using TurboMapReader;

namespace Rogue
{
    internal class Map
    {
        public int mapWidth;
        public MapLayer[] layers;


        List<Enemy>? enemies;
        List<Item>? items;


        public struct Enemy
        {
            public string name;
            public Point2D position;
            public Texture EnemySprite;
            public Enemy(string pname, Point2D pposition, Texture Sprite)
            {
                this.name = pname;
                this.position = pposition;
                this.EnemySprite = Sprite;
            }

        }
        public struct Item
        {
            public string name;
            public Point2D position;
            public Item(string pname, Point2D pposition, Texture item_image)
            {
                this.name = pname;
                this.position = pposition;
            }

        }

        public Rectangle SetIndex(int imagesPerRow, int index)
        {
            float tileSize = 16f;
            float imagePixelX = index % imagesPerRow * tileSize;
            float imagePixelY = (int)(index / imagesPerRow) * tileSize;

            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            return imageRect;
        }


        public void DrawMap()
        {

            MapLayer groundLayer = layers[0];

            int[] groundTiles = groundLayer.mapTiles;
            Texture Image = Game.atlasImage;
            int mapHeight = groundTiles.Length / mapWidth;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int tileId = groundTiles[x + y * mapWidth];
                    Vector2 pos = new Vector2(x * Game.tileSize, y * Game.tileSize);
                    Rectangle rect = SetIndex(12, tileId - 1);
                    Raylib.DrawTextureRec(Image, rect, pos, Raylib.WHITE);
                }
            }
            LoadEnemiesAndItems();
        }
        public void DrawEnemy(Texture Sprite, Point2D position)
        {
            int imagesPerRow = 12;
            float tileSize = 16;
            
            int atlasIndex = 0 + 10 * imagesPerRow;

            int imageX = atlasIndex % imagesPerRow; 
            int imageY = (int)(atlasIndex / imagesPerRow);
            float imagePixelX = imageX * tileSize;
            float imagePixelY = imageY * tileSize;


            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            // Laske paikka ruudulla
            int pixelPositionX = position.x * Game.tileSize;
            int pixelPositionY = position.y * Game.tileSize;
            Vector2 pixelPosition = new Vector2(pixelPositionX, pixelPositionY);
            Raylib.DrawTextureRec(Sprite, imageRect, pixelPosition, Raylib.WHITE);
        }
        public void DrawItem(Texture Sprite, Point2D position)
        {
            int imagesPerRow = 12;
            float tileSize = 16f;

            int atlasIndex = 1 + 5 * imagesPerRow;

            int imageX = atlasIndex % imagesPerRow;
            int imageY = (int)(atlasIndex / imagesPerRow);
            float imagePixelX = imageX * tileSize;
            float imagePixelY = imageY * tileSize;


            Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);

            // Laske paikka ruudulla
            int pixelPositionX = position.x * Game.tileSize;
            int pixelPositionY = position.y * Game.tileSize;
            Vector2 pixelPosition = new Vector2(pixelPositionX, pixelPositionY);
            Raylib.DrawTextureRec(Sprite, imageRect, pixelPosition, Raylib.WHITE);

        }


        public void LoadEnemiesAndItems()
        {
            enemies = new List<Enemy>();


            MapLayer enemyLayer = layers[2];
            int[] enemyTiles = enemyLayer.mapTiles;
            int mapHeight = enemyTiles.Length / mapWidth;
            Texture enemy_image = Game.atlasImage;
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Point2D position = new Point2D(x, y);
                    Vector2 Enemypos = new Vector2(x * Game.tileSize, y * Game.tileSize);
                    int index = x + y * mapWidth;
                    int EnemytileId = enemyTiles[index];
                    switch (EnemytileId)
                    {
                        case 0:
                            // ei mitään tässä kohtaa
                            break;
                        case 121:
                            enemies.Add(new Enemy("Bat", position, enemy_image));
                            break;
                        case 109:
                            enemies.Add(new Enemy("Ghost", position, enemy_image));
                            break;
                    }
                    if (EnemytileId != 0)
                    {

                        Rectangle Enemyrect = SetIndex(12, EnemytileId - 1);
                        Raylib.DrawTextureRec(enemy_image, Enemyrect, Enemypos, Raylib.WHITE);
                    }
                }
            }


            items = new List<Item>();

            MapLayer itemLayer = layers[1];
            Texture item_image = Game.atlasImage;
            int[] itemTiles = itemLayer.mapTiles;
            int mapHeight1 = itemTiles.Length / mapWidth;
            for (int y = 0; y < mapHeight1; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Point2D position = new Point2D(x, y);
                    Vector2 Itempos = new Vector2(x * Game.tileSize, y * Game.tileSize);
                    int index = x + y * mapWidth;
                    int ItemtileId = itemTiles[index];
                    switch (ItemtileId)
                    {

                        case 0:
                            // ei mitään tässä kohtaa
                            break;
                        case 1:
                            items.Add(new Item("Sword", position, item_image));
                            break;
                        case 2:
                            items.Add(new Item("Spear", position, item_image));
                            break;
                        case 3:
                            items.Add(new Item("Ring", position, item_image));
                            break;
                        case 4:
                            items.Add(new Item("Armor", position, item_image));
                            break;
                        case 5:
                            items.Add(new Item("Shield", position, item_image));
                            break;
                        case 6:
                            items.Add(new Item("Helmet", position, item_image));
                            break;
                        case 62:
                            items.Add(new Item("ItemBox", position, item_image));
                            break;



                    }
                    if (ItemtileId != 0)
                    {

                        Rectangle Enemyrect = SetIndex(12, ItemtileId - 1);
                        Raylib.DrawTextureRec(enemy_image, Enemyrect, Itempos, Raylib.WHITE);
                    }
                }
            }
        }


        public Enemy? GetEnemyAt(Point2D position)
        {
            foreach (var Enemy in enemies)
            {
  
                Vector2 Playerv2 = new Vector2(position.x, position.y);
                Vector2 Enemyv2 = new Vector2(Enemy.position.x, Enemy.position.y);
                if (Playerv2 == Enemyv2)
                {
                    Console.WriteLine("Enemy Found");
                    Raylib.PlaySound(Game.EnemySound);
                    return Enemy;
                }
            }
            return null;
        }
        public Item? GetItemAt(Point2D position) 
        {
            foreach (var Item in items)
            {
                Vector2 Playerv2 = new Vector2(position.x, position.y);
                Vector2 Itemv2 = new Vector2(Item.position.x, Item.position.y);
                if (Playerv2 == Itemv2)
                {
                    Console.WriteLine("Item Found");
                    Raylib.PlaySound(Game.ItemSound);
                    return Item;
                }
            }
            return null;
        }

    }
}
