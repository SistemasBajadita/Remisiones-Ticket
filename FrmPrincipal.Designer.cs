using System.Windows.Forms;

namespace Ticket_bonito
{
	partial class FrmPrincipal
	{
		/// <summary>
		/// Variable del diseñador necesaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén usando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.TabPages = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.TxtFiltro = new System.Windows.Forms.TextBox();
			this.reporte = new System.Windows.Forms.DataGridView();
			this.BtnPrintTicket = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.BtnPrintReport = new System.Windows.Forms.Button();
			this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
			this.label4 = new System.Windows.Forms.Label();
			this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbVendedor = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.TabPages.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.reporte)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// TabPages
			// 
			this.TabPages.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.TabPages.Controls.Add(this.tabPage1);
			this.TabPages.Controls.Add(this.tabPage2);
			this.TabPages.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TabPages.HotTrack = true;
			this.TabPages.Location = new System.Drawing.Point(12, 12);
			this.TabPages.Name = "TabPages";
			this.TabPages.SelectedIndex = 0;
			this.TabPages.Size = new System.Drawing.Size(1212, 482);
			this.TabPages.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Linen;
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.dateTimePicker2);
			this.tabPage1.Controls.Add(this.dateTimePicker1);
			this.tabPage1.Controls.Add(this.TxtFiltro);
			this.tabPage1.Controls.Add(this.reporte);
			this.tabPage1.Controls.Add(this.BtnPrintTicket);
			this.tabPage1.Location = new System.Drawing.Point(4, 35);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1204, 443);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Imprimir ticket";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(73, 373);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(145, 21);
			this.label1.TabIndex = 21;
			this.label1.Text = "Cargando tickets";
			this.label1.Visible = false;
			// 
			// dateTimePicker2
			// 
			this.dateTimePicker2.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimePicker2.Location = new System.Drawing.Point(205, 20);
			this.dateTimePicker2.Name = "dateTimePicker2";
			this.dateTimePicker2.Size = new System.Drawing.Size(120, 28);
			this.dateTimePicker2.TabIndex = 19;
			this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimePicker1.Location = new System.Drawing.Point(79, 20);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(120, 28);
			this.dateTimePicker1.TabIndex = 18;
			this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
			// 
			// TxtFiltro
			// 
			this.TxtFiltro.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtFiltro.Location = new System.Drawing.Point(331, 20);
			this.TxtFiltro.Name = "TxtFiltro";
			this.TxtFiltro.Size = new System.Drawing.Size(730, 28);
			this.TxtFiltro.TabIndex = 17;
			this.TxtFiltro.TextChanged += new System.EventHandler(this.TxtFiltro_TextChanged);
			// 
			// reporte
			// 
			this.reporte.AllowUserToAddRows = false;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(245)))), ((int)(((byte)(196)))));
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Lucida Fax", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Navy;
			this.reporte.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
			this.reporte.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.reporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.reporte.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.reporte.BackgroundColor = System.Drawing.Color.Linen;
			this.reporte.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(78)))), ((int)(((byte)(80)))));
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.reporte.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.reporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.reporte.EnableHeadersVisualStyles = false;
			this.reporte.Location = new System.Drawing.Point(23, 67);
			this.reporte.Name = "reporte";
			this.reporte.ReadOnly = true;
			this.reporte.RowHeadersVisible = false;
			this.reporte.RowHeadersWidth = 51;
			dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(229)))), ((int)(((byte)(116)))));
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Lucida Fax", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Navy;
			this.reporte.RowsDefaultCellStyle = dataGridViewCellStyle6;
			this.reporte.RowTemplate.Height = 24;
			this.reporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.reporte.Size = new System.Drawing.Size(1154, 271);
			this.reporte.TabIndex = 20;
			// 
			// BtnPrintTicket
			// 
			this.BtnPrintTicket.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnPrintTicket.Location = new System.Drawing.Point(628, 361);
			this.BtnPrintTicket.Name = "BtnPrintTicket";
			this.BtnPrintTicket.Size = new System.Drawing.Size(342, 42);
			this.BtnPrintTicket.TabIndex = 16;
			this.BtnPrintTicket.Text = "Imprimir ticket";
			this.BtnPrintTicket.UseVisualStyleBackColor = true;
			this.BtnPrintTicket.Click += new System.EventHandler(this.BtnPrintTicket_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.Linen;
			this.tabPage2.Controls.Add(this.BtnPrintReport);
			this.tabPage2.Controls.Add(this.dateTimePicker4);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.dateTimePicker3);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.cmbVendedor);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Location = new System.Drawing.Point(4, 35);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1204, 443);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Reporte por vendedor";
			// 
			// BtnPrintReport
			// 
			this.BtnPrintReport.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnPrintReport.Location = new System.Drawing.Point(395, 251);
			this.BtnPrintReport.Name = "BtnPrintReport";
			this.BtnPrintReport.Size = new System.Drawing.Size(262, 71);
			this.BtnPrintReport.TabIndex = 6;
			this.BtnPrintReport.Text = "Imprimir";
			this.BtnPrintReport.UseVisualStyleBackColor = true;
			this.BtnPrintReport.Click += new System.EventHandler(this.BtnPrintReport_Click);
			// 
			// dateTimePicker4
			// 
			this.dateTimePicker4.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimePicker4.Location = new System.Drawing.Point(701, 164);
			this.dateTimePicker4.Name = "dateTimePicker4";
			this.dateTimePicker4.Size = new System.Drawing.Size(123, 28);
			this.dateTimePicker4.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(674, 165);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(21, 23);
			this.label4.TabIndex = 4;
			this.label4.Text = "a";
			// 
			// dateTimePicker3
			// 
			this.dateTimePicker3.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dateTimePicker3.Location = new System.Drawing.Point(545, 164);
			this.dateTimePicker3.Name = "dateTimePicker3";
			this.dateTimePicker3.Size = new System.Drawing.Size(123, 28);
			this.dateTimePicker3.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(474, 165);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Fecha";
			// 
			// cmbVendedor
			// 
			this.cmbVendedor.Font = new System.Drawing.Font("Century", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbVendedor.FormattingEnabled = true;
			this.cmbVendedor.Location = new System.Drawing.Point(206, 164);
			this.cmbVendedor.Name = "cmbVendedor";
			this.cmbVendedor.Size = new System.Drawing.Size(251, 29);
			this.cmbVendedor.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Century", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(103, 165);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Vendedor";
			// 
			// FrmPrincipal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(145)))), ((int)(((byte)(58)))));
			this.ClientSize = new System.Drawing.Size(1236, 506);
			this.Controls.Add(this.TabPages);
			this.Name = "FrmPrincipal";
			this.Text = "Imprimir tickets";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.TabPages.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.reporte)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private TabControl TabPages;
		private TabPage tabPage1;
		private Label label1;
		private DateTimePicker dateTimePicker2;
		private DateTimePicker dateTimePicker1;
		private TextBox TxtFiltro;
		private DataGridView reporte;
		private Button BtnPrintTicket;
		private TabPage tabPage2;
		private Label label2;
		private ComboBox cmbVendedor;
		private Label label3;
		private Label label4;
		private DateTimePicker dateTimePicker3;
		private Button BtnPrintReport;
		private DateTimePicker dateTimePicker4;
	}
}

