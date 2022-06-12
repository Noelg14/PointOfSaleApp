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
                    if (args[0].Equals("test"))
                    {
                        Utils.log("Running test param");
                        Application.Run(new frmManage());

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