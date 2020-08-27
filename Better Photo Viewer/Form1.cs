using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Better_Photo_Viewer
{

    public partial class frmBPV : Form
    {

        public static string testingString = "FUcking shit";

        public frmBPV()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Sets Environment Working Directory
            var t = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = t.DirectoryName;

            //Gets files
            string[] args = Environment.GetCommandLineArgs();

            //Checks if file exists and if file is present
            if (args.Length < 2)
            {
                MessageBox.Show("Please select a valid image.", "Better Photo Viewer");
                Environment.Exit(-1);
            }
            if (!File.Exists(args[1]))
            {
                MessageBox.Show("Could not find a valid image.", "Better Photo Viewer");
                Environment.Exit(-1);
            }

            //Sets display
            picDisplay.Image = Image.FromFile(args[1]);

            //Sets display size
            picDisplay.Size = new Size(picDisplay.Image.Width, picDisplay.Image.Height);
            Size = new Size(picDisplay.Image.Width + 16, picDisplay.Image.Height + 39);

            //Loads plugins
            if (!Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");

            foreach (string f in Directory.GetFiles("Plugins"))
            {

                //Checks for dll file
                if (Path.GetExtension(f) != ".dll")
                {
                    Console.WriteLine("Skipped: " + Path.GetExtension(f));
                    continue;
                }
                
                //Loads plugin
                LoadPlugin(Environment.CurrentDirectory + Path.DirectorySeparatorChar + f);

            }

        }

        private void frmBPV_Resize(Object sender, EventArgs e)
        {

            picDisplay.Size = new Size(Size.Width - 16, Size.Height - 39);

        }

        private void LoadPlugin(string f)
        {

            //Loads plugin and checks for valid Type
            Assembly pluginFile = Assembly.LoadFile(f);

            foreach (Type type in pluginFile.GetTypes())
            {

                if (type.Name != "BPVPlugin")
                {
                    continue;
                }

                //Passes variables to constructor and invokes plugin
                Activator.CreateInstance(type, this, picDisplay);

            }

            //Type type = ass.GetType("BPV_Plugins.BPVPlugin");

            //if (type == null)
            //{
            //    Console.WriteLine("Failed to load plugin: " + f);
            //}

            ////Passes variables to constructor and invokes
            //Activator.CreateInstance(type, this, picDisplay);

        }

    }

}
