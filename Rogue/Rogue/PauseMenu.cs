using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    class PauseMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public event EventHandler OptionsButtonPressedEvent;
        public event EventHandler MainMenuButtonPressedEvent;
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

            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "MainMenu") == 1)
            {
                MainMenuButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Options") == 1)
            {
                OptionsButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }
            button_y += button_height * 2;
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Back") == 1)
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
            }

            Raylib.EndDrawing();
        }
    }
}
