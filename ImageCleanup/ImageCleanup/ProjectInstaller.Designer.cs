namespace ImageCleanup
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ImageCleanupServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ImageCleanupInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ImageCleanupServiceProcessInstaller
            // 
            this.ImageCleanupServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ImageCleanupServiceProcessInstaller.Password = null;
            this.ImageCleanupServiceProcessInstaller.Username = null;
            // 
            // ImageCleanupInstaller
            // 
            this.ImageCleanupInstaller.Description = "Solink\'s Image cleanup task removes old images.";
            this.ImageCleanupInstaller.DisplayName = "Solink ImageCleanup";
            this.ImageCleanupInstaller.ServiceName = "ImageCleanup";
            this.ImageCleanupInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ImageCleanupServiceProcessInstaller,
            this.ImageCleanupInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ImageCleanupServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ImageCleanupInstaller;
    }
}