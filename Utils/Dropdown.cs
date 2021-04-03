using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMenu.Utils
{
    class Dropdown
    {
        private Vector2 scrollViewVector = Vector2.zero;
        private Rect dropdownLocation { get; set; }
        private string dropdownTitle { get; set; }
        private string[] options { get; set; }

        private bool expanded = false;
        public int currentlySelectedIndex = 0;

        public Dropdown(Rect position, string[] options)
        {
            this.dropdownLocation = position;
            this.options = options;
            this.dropdownTitle = "Currently Selected";
        }

        public Dropdown(Rect position, string[] options, string title)
        {
            this.dropdownLocation = position;
            this.options = options;
            this.dropdownTitle = title;
        }

        public void Draw()
        {
            if (GUI.Button(new Rect(dropdownLocation.x, dropdownLocation.y, dropdownLocation.width, 25), $"{dropdownTitle}: {options[currentlySelectedIndex]}")) 
                expanded = !expanded;

            if (expanded)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(dropdownLocation.x, (dropdownLocation.y + 25), dropdownLocation.width, dropdownLocation.height), scrollViewVector, new Rect(0, 0, dropdownLocation.width, Mathf.Max(dropdownLocation.height, (options.Length * 25))));
                for (int index = 0; index < options.Length; index++)
                {

                    if (GUI.Button(new Rect(0, (index * 25), dropdownLocation.width, 25), options[index]))
                    {
                        expanded = false;
                        currentlySelectedIndex = index;
                    }

                }

                GUI.EndScrollView();
            }

        }
    }
}
