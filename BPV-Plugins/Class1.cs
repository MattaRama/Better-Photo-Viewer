using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DirectoryNavigation
{
    public class BPVPlugin
    {

        int activeFile = 0;

        Form f;
        PictureBox p;
        List<string> pdir;

        public BPVPlugin(Form f, PictureBox p)
        {

            //Stores arguments
            this.f = f;
            this.p = p;

            //Sets focus to picturebox
            p.Focus();

            //Gets Photo directory
            var photo = new FileInfo(Environment.GetCommandLineArgs()[1]).Directory.FullName;
            var dir = new DirectoryInfo(photo);

            //Gets all pictures in directory
            pdir = new List<string>();

            foreach (FileInfo fi in dir.GetFiles("*"))
            {

                var nSP = fi.Name.Split('.');
                if ("png jpg gif jpeg wmf bmp".Contains(nSP[nSP.Length - 1]))
                {

                    Console.WriteLine("DIRFILE: " + fi.DirectoryName + "\\" + fi.Name);
                    pdir.Add(fi.DirectoryName + "\\" + fi.Name);

                }

            }

            //Finds active file
            for (int i = 0; i < pdir.Count; i++)
            {

                if (pdir[i] == photo)
                {
                    activeFile = i;
                }

            }

            //Adds methods for keyboard handling
            f.KeyDown += OnKeyDown;

        }

        /// <summary>
        /// Handles Keypresses
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            
            //Handles moving left/right
            if (e.KeyCode == Keys.Left)
            {

                //Sets active file
                activeFile--;

                if (activeFile == -1)
                {
                    activeFile = pdir.Count - 1;
                }

                //Sets active image
                SetActiveImage();

            } else if (e.KeyCode == Keys.Right)
            {

                //Sets active file
                activeFile++;

                if (activeFile == pdir.Count)
                {
                    activeFile = 0;
                }

                //Sets active image
                SetActiveImage();

            }

        }

        private void SetActiveImage()
        {

            //Loads image into memory
            Image i;

            try
            {

                i = Image.FromFile(pdir[activeFile]);

            } catch
            {
                return;
            }

            //Sets display
            var toD = p.Image;
            p.Image = i;
            toD.Dispose();

        }

    }
}

