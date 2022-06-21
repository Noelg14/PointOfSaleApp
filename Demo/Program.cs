namespace Demo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                if (args.Length != 0)
                {
                    switch (args[0]) {
                        case ("test"):
                            Utils.log("Running test param");
                            Application.Run(new frmManage());
                            break;
                        case ("qr"):
                            Utils.log("Running param"+args[0]);
                            Application.Run(new qr(Utils.genQR("https://noelgriffin.ie")));
                            break;
                        default:
                            Utils.log("Running param "+args[0]);
                            Application.Run(new Form1());
                            break;
                    }


                }
                else
                {
                    Utils.log("Running, no Params");
                    Application.Run(new Form1());
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}