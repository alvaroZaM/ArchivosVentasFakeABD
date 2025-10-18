namespace ArchivosVentasFakeABD;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    


        private System.Windows.Forms.Button btnCargarYGuardar;
        private System.Windows.Forms.ListBox lstArchivos;
        private System.Windows.Forms.Label lblTitulo;

        private void InitializeComponent()
        {
             this.btnCargarYGuardar = new System.Windows.Forms.Button();
            this.lstArchivos = new System.Windows.Forms.ListBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
             // Form1
            this.ClientSize = new System.Drawing.Size(584, 321);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lstArchivos);
            this.Controls.Add(this.btnCargarYGuardar);
            this.Name = "Form1";
            this.Text = "Cargar Archivos de Ventas";
            this.ResumeLayout(false);
            this.PerformLayout();
            
            // btnCargarYGuardar 
            this.btnCargarYGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCargarYGuardar.Location = new System.Drawing.Point(30, 250);
            this.btnCargarYGuardar.Name = "btnCargarYGuardar";
            this.btnCargarYGuardar.Size = new System.Drawing.Size(280, 40);
            this.btnCargarYGuardar.TabIndex = 0;
            this.btnCargarYGuardar.Text = "Cargar Archivos y Guardar en BD";
            this.btnCargarYGuardar.UseVisualStyleBackColor = true;
            this.btnCargarYGuardar.Click += new System.EventHandler(this.btnCargarYGuardar_Click);
            
            // lstArchivos
            this.lstArchivos.FormattingEnabled = true;
            this.lstArchivos.ItemHeight = 15;
            this.lstArchivos.Location = new System.Drawing.Point(30, 60);
            this.lstArchivos.Name = "lstArchivos";
            this.lstArchivos.Size = new System.Drawing.Size(500, 169);
            this.lstArchivos.HorizontalScrollbar = true;
            this.lstArchivos.TabIndex = 1;
            
            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(30, 30);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(215, 19);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Text = "Archivos seleccionados a cargar:";
     
           
        }
 
    #endregion
}
