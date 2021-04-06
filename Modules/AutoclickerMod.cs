using System.Runtime.InteropServices;
using FMenu.Utils;
using UnityEngine;

namespace FMenu.Modules
{
    public class AutoclickerMod
    {
        public bool Active { get; set; }
        public bool LeftActive { get; set; }
        public bool RightActive { get; set; }

        private bool Pressed { get; set; }

        public AutoclickerMod()
        {
            this.Active = false;
            this.Pressed = false;
            this.LeftActive = false;
            this.RightActive = false;
        }

        public void Update()
        {
            if (Active && Input.GetKey(KeyCode.E))
            {
                if (!Pressed && LeftActive) Mouse.MouseEvent(Mouse.MouseEventFlags.LeftDown);
                if (!Pressed && RightActive) Mouse.MouseEvent(Mouse.MouseEventFlags.RightDown);
                if (Pressed && LeftActive) Mouse.MouseEvent(Mouse.MouseEventFlags.LeftUp);
                if (Pressed && RightActive) Mouse.MouseEvent(Mouse.MouseEventFlags.RightUp);
                this.Pressed = !Pressed;
            }
        }
    }
}