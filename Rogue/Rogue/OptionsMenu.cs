using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    class OptionsMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public void DrawMenu()
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
            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Back") == 1)
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }

            Raylib.EndDrawing();
        }
    }
}
