using System.Numerics;
using ZeroElectric.Vinculum;
using TurboMapReader;
using RayGuiCreator;
using System.Collections.Generic;
using System;



namespace Rogue
{
    public class Game
    {
        int screen_width = 1280;
        int screen_height = 720;

        PlayerCharacter player;
        Map level01;
        TiledMap level;
        public static readonly int tileSize = 16;
        int game_width;
        int game_height;
        RenderTexture game_screen;
        public static Sound EnemySound;
        public static Sound ItemSound;
        public static Sound WallCollide;
        public static Texture atlasImage;
        OptionsMenu myOptionsMenu;
        PauseMenu myPauseMenu;
        Stack<GameState> stateStack = new Stack<GameState>();
        public enum GameState
        {
            MainMenu,
            CharacterCreation,
            GameLoop,
            PauseMenu,
            OptionsMenu
        }

        public GameState currentGameState;
        TextBoxEntry playerNameEntry;
        bool IsNameOk;
        public MultipleChoiceEntry classChoices = new MultipleChoiceEntry(new string[] {Class.Rogue.ToString(), Class.Warrior.ToString(), Class.Mage.ToString()});
        public MultipleChoiceEntry raceChoices = new MultipleChoiceEntry(new string[] { Race.Human.ToString(),Race.Elf.ToString(), Race.Orc.ToString()});
        public void Run()
        {
            Init();
            GameLoop();
            Raylib.UnloadRenderTexture(game_screen);
            Raylib.CloseWindow();
        }

        private void Init()
        {
            ChangeState(GameState.MainMenu);
            player = new PlayerCharacter();
            playerNameEntry = new TextBoxEntry(12);
            myOptionsMenu = new OptionsMenu();
            myPauseMenu = new PauseMenu();
            player.position = new Point2D(2, 2);

            myOptionsMenu.BackButtonPressedEvent += this.OnOptionsBackButtonPressed;
            myPauseMenu.BackButtonPressedEvent += this.OnPauseBackButtonPressed;
            myPauseMenu.OptionsButtonPressedEvent += this.OnPauseOptionsButtonPressed;
            myPauseMenu.MainMenuButtonPressedEvent += this.OnPauseMainMenuPressed;


            Raylib.InitAudioDevice();
            Raylib.InitWindow(screen_width, screen_height, "Rogue");
            Raylib.SetTargetFPS(30);
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);

            EnemySound = Raylib.LoadSound("Sounds/Enemy.mp3");
            ItemSound = Raylib.LoadSound("Sounds/Item.mp3");
            WallCollide = Raylib.LoadSound("Sounds/WallCollide.mp3");



            MapLoader Reader = new MapLoader();
            level01 = Reader.LoadLayeredMap("Maps/mapfile.json");
            level = MapReader.LoadMapFromFile("Maps/mapfile.json");

            Reader.ToMap(level01, level);

            atlasImage = Raylib.LoadTexture("Images/MAP.png");

            player.SetImageAndIndex(atlasImage, 12, 96);

            game_width = 480;
            game_height = 360;

            player.MapWidth = level01.mapWidth;
            player.MapTiles = level01.layers[0].mapTiles;

            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            Raylib.SetWindowMinSize(game_width, game_height);
        }

        private void DrawGame()
        {
            level01.DrawMap();
            player.Draw();



        }

        private void DrawGameToTexture()
        {
            Raylib.BeginTextureMode(game_screen);
            DrawGame();
            Raylib.EndTextureMode();
            DrawGameScaled();
        }

        private void UpdateGame()
        {
            Console.CursorVisible = false;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                player.Move(0, -1);
                level01.GetEnemyAt(player.position);
                level01.GetItemAt(player.position);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                player.Move(0, 1);
                level01.GetEnemyAt(player.position);
                level01.GetItemAt(player.position);
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                player.Move(1, 0);
                level01.GetEnemyAt(player.position);
                level01.GetItemAt(player.position);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                player.Move(-1, 0);
                level01.GetEnemyAt(player.position);
                level01.GetItemAt(player.position);

            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_TAB))
            {
                ChangeState(GameState.PauseMenu);

            }
        }

        private void GameLoop()
        {
            while (Raylib.WindowShouldClose() == false)
            {
                switch (stateStack.Peek())
                {
                    case GameState.MainMenu:
                        DrawMainMenu();
                        break;
                    case GameState.CharacterCreation:
                        DrawCharacterMenu();
                        break;

                    case GameState.GameLoop:
                        UpdateGame();
                        DrawGameToTexture();
                        break;
                    case GameState.PauseMenu:
                        myPauseMenu.DrawMenu();
                        break;
                    case GameState.OptionsMenu:
                        myOptionsMenu.DrawMenu();
                        break;
                }

            }
        }

        private void DrawGameScaled()
        {

            // Tässä piirretään tekstuuri ruudulle
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);


            Rectangle source = new Rectangle(0.0f, 0.0f,
                game_screen.texture.width,
                game_screen.texture.height * -1.0f);

            Rectangle destination = new Rectangle(
                (draw_width - (float)game_width * scale) * 0.5f,
                (draw_height - (float)game_height * scale) * 0.5f,
                game_width * scale,
                game_height * scale);

            Raylib.DrawTexturePro(game_screen.texture,
                source, destination,
                new Vector2(0, 0), 0.0f, Raylib.WHITE);

            Raylib.EndDrawing();
        }

        public void DrawMainMenu()
        {
            // Tyhjennä ruutu ja aloita piirtäminen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            // Laske ylimmän napin paikka ruudulla.
            int button_width = 100;
            int button_height = 20;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            // Piirrä pelin nimi nappien yläpuolelle
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 4, button_width, button_height), "Rogue");
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 3, button_width, button_height), "Move: arrow keys");
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Pause with TAB");
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Start Game") == 1)
            {
                ChangeState(GameState.GameLoop);
            }

            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Create Character") == 1)
            {
                ChangeState(GameState.CharacterCreation);
            }
            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Options") == 1)
            {
                ChangeState(GameState.OptionsMenu);
            }

            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Quit") == 1)
            {
                Raylib.CloseWindow();
            }

            Raylib.EndDrawing();
        }

        public void DrawCharacterMenu()
        {
            
            // Tyhjennä ruutu ja aloita piirtäminen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int x = 0;
            int y = 40;
            int width = 140;
            MenuCreator c = new MenuCreator(Raylib.GetScreenWidth() / 2 - width / 2, y, width);
            string playername;
            c.Label("Character creation");
            c.Label("");
            c.Label("Character Name:");
            c.TextBox(playerNameEntry);
            c.Label("");
            c.Label("Character Race:");
            c.ToggleGroup(raceChoices);
            c.Label("");
            c.Label("Character Class");
            c.ToggleGroup(classChoices);
            c.Label("");
            c.Label("");
            if(c.LabelButton("Create Character"))
            {
                playername = playerNameEntry.ToString();

                CheckIfNameOK(playername);
                if (IsNameOk == true) 
                {
                    player.name = playername;
                    switch (raceChoices.GetSelected())
                    {
                        case "Human":
                            player.race = Race.Human; 
                            break;
                        case "Elf":
                            player.race = Race.Elf;
                            break;
                        case "Orc":
                            player.race = Race.Orc;
                            break;

                    }
                    switch (classChoices.GetSelected())
                    {
                        case "Rogue":
                            player.Role = Class.Rogue;
                            break;
                        case "Warrior":
                            player.Role = Class.Warrior;
                            break;
                        case "Mage":
                            player.Role = Class.Mage;
                            break;

                    }
                    ChangeState(GameState.GameLoop);

                }
                
            }
            c.EndMenu();

            

            Raylib.EndDrawing();
        }

        public string CheckIfNameOK(string name)
        {   

            IsNameOk = true;
            if (Char.IsLetter(name[0]) == false)
            {
                IsNameOk = false;
            }   
            for (int i = 0; i < name.Length - 1; i++)
            {
                char kirjain = name[i];
                if (Char.IsLetter(kirjain) != true && Char.IsControl(kirjain) != true) 
                {
                    IsNameOk = false;
                }
 
            }

            if (IsNameOk == false)
            {
                Console.WriteLine("Name can only contain letters!");
            }
            return name;

        }

        void OnOptionsBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
        }
        void OnPauseMainMenuPressed(object sender, EventArgs args)
        {
            ChangeState(GameState.MainMenu);
        }
        void OnPauseBackButtonPressed(object sender, EventArgs args)
        {
            stateStack.Pop();
        }
        void OnPauseOptionsButtonPressed(object sender, EventArgs args)
        {
            ChangeState(GameState.OptionsMenu);
        }

        void ChangeState(GameState gameState)
        {
            currentGameState = gameState;
            if (gameState == GameState.MainMenu)
            {
                stateStack.Clear();
            }
                
            stateStack.Push(currentGameState);
        }

    }

}
