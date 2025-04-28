using System.Numerics;
using System.Transactions;
using ZeroElectric.Vinculum;
namespace raylib_pohja
{
    class WindowTest
    {
        const int screen_width = 1000;
        const int screen_height = 700;
        int Brick_Width = screen_width / 8;
        int Brick_Height = screen_height/8;
        public WindowTest()
        {

        }

        public void Run()
        {
            Raylib.InitWindow(screen_width, screen_height, "Raylib");
            Raylib.SetTargetFPS(60);

            while (Raylib.WindowShouldClose() == false)
            {
                House(screen_width / 2, screen_height / 2, 3, 10);
            }

            Raylib.CloseWindow();
        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.YELLOW);

            Raylib.DrawCircle(screen_width / 2, screen_height / 2, 20, Raylib.MAROON);

            Raylib.EndDrawing();
        }

        private void DrawAll()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            // Draws a maroon circle in the middle
            Raylib.DrawCircle(screen_width / 2, screen_height, 20, Raylib.RED);

            Raylib.DrawRectangle(screen_width / 4, screen_height / 2, 20, 20, Raylib.YELLOW);

            Raylib.DrawTriangle((new Vector2 (screen_width/1.5f, 80 )),
                              (new Vector2(screen_width/1.5f - 20.0f, 115)),
                              (new Vector2(screen_width/1.5f + 20.0f, 115)), Raylib.DARKBLUE);




            Raylib.EndDrawing();
        }
        private void ChessBoard() 
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            for (int i = 0; i < 8; i ++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (j != 8)
                    {
                        if ((i + j) % 2 == 0) Raylib.DrawRectangle(i * Brick_Width, j * Brick_Height, Brick_Width, Brick_Height, Raylib.WHITE);
                        else Raylib.DrawRectangle(i * Brick_Width, j * Brick_Height, Brick_Width, Brick_Height, Raylib.GRAY);
                    }
                }
            }





            Raylib.EndDrawing();
        }
        private void SnowwMan(int position_x, int position_y) 
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            Raylib.DrawCircle(position_x, position_y, 50, Raylib.WHITE);
            Raylib.DrawCircle(position_x, position_y - 50 , 40, Raylib.WHITE);
            Raylib.DrawCircle(position_x, position_y - 95, 30, Raylib.WHITE);

            Raylib.EndDrawing();


        }
        private void Tree(int position_x, int position_y, int Height)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);


            Raylib.DrawTriangle((new Vector2(position_x, position_y)),
                  (new Vector2(position_x - Height, position_y + Height)),
                  (new Vector2(position_x + Height, position_y + Height)), Raylib.GREEN);
            position_y += Height / 2;
            Raylib.DrawTriangle((new Vector2(position_x, position_y)),
                  (new Vector2(position_x - Height, position_y + Height)),
                  (new Vector2(position_x + Height, position_y + Height)), Raylib.GREEN);
            position_y += Height / 2;
            Raylib.DrawTriangle((new Vector2(position_x, position_y)),
                  (new Vector2(position_x - Height, position_y + Height)),
                  (new Vector2(position_x + Height, position_y + Height)), Raylib.GREEN);
            position_y += Height;
            Raylib.DrawRectangle(position_x - Height / 4, position_y, Height / 2, Height, Raylib.BROWN);

            Raylib.EndDrawing();
        }

        private void House(int position_x, int position_y, int windows_per_floor, int floor_amount)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.GRAY);

            Raylib.DrawRectangle(position_x, position_y, windows_per_floor * 100 , floor_amount * 100, Raylib.BLACK);

            int Brick_Width = windows_per_floor * 100 / windows_per_floor;
            int Brick_Height = floor_amount * 100 / floor_amount;

            for (int i = 0; i < floor_amount; i++)
            {
                for (int j = 0; j < windows_per_floor; j++)
                {
                    if (j != floor_amount)
                    {
                        if ((i + j) % 2 == 0) Raylib.DrawRectangle( (position_x + 10) +  (i * Brick_Width), (position_y + 10) + (j * Brick_Height), Brick_Width - 20, Brick_Height - 20, Raylib.WHITE);
                        else Raylib.DrawRectangle((position_x + 10) + (i * Brick_Width), (position_y + 10) + (j * Brick_Height), Brick_Width - 20, Brick_Height - 20, Raylib.YELLOW);
                    }
                }
            }

            Raylib.EndDrawing();

        }
    }
}
