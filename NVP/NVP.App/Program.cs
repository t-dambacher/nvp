using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NVP.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Prompts the user to select a video, and opens it
            VideoDecoder player = OpenVideo(args);
            if (player == null)
                Application.Exit();

            Application.Run(new VideoPlayer(player));
        }

        /// <summary>
        /// Prompts the user to select a video, and loads a video player capable of reading this file
        /// </summary>
        /// <param name="commandLineArgs">Command line arguments</param>
        /// <returns>A video player capable of reading this file</returns>
        private static VideoDecoder OpenVideo(String[] commandLineArgs)
        {
            String filename = String.Empty;
            if (commandLineArgs != null && commandLineArgs.Length > 0)
                filename = commandLineArgs[0];
            else
                filename = SelectVideoFile();
            if (String.IsNullOrWhiteSpace(filename))
                return null;

            return VideoDecoder.FromFile(filename);
        }

        /// <summary>
        /// Prompts the user to select a video file to open, and returns its path
        /// </summary>
        /// <returns>The paths of the user selected video file</returns>
        private static String SelectVideoFile()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "h264 video files (*.mp4)|*.mp4",
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
                return ofd.FileName;

            return String.Empty;
        }
    }
}
