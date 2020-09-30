using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TutteeFrame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< Updated upstream
            Application.Run(new frmMain());
=======
            Application.Run(new frmAccountManagement());

>>>>>>> Stashed changes
        }
    }
}
