//Programado por Bryan Allan Valdez Muñoz
//alan272_@hotmail.com
//6312988689

using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticket_bonito
{
	public partial class FrmPrincipal : Form
	{
		ClsConnection conn;

		public FrmPrincipal()
		{
			InitializeComponent();
			conn = new ClsConnection(ConfigurationManager.ConnectionStrings["servidor"].ToString());
			Icon = new System.Drawing.Icon("Imagenes/LOGO_EMPRESA-removebg-preview.ico");

			dateTimePicker1.MaxDate = DateTime.Now;
			dateTimePicker2.MaxDate = DateTime.Now;
			dateTimePicker3.MaxDate = DateTime.Now;
			dateTimePicker4.MaxDate = DateTime.Now;
		}

		private async void TxtFiltro_TextChanged(object sender, EventArgs e)
		{
			string filter = TxtFiltro.Text;
			string dateA = dateTimePicker1.Value.ToString("yyyy-MM-dd");
			string dateB = dateTimePicker2.Value.ToString("yyyy-MM-dd");
			string query = $@"select ven.fec_doc as Fecha,ven.ref_doc as Folio,cli.NOM_CLI as Nombre, ven.hora_reg as Hora, concat('$', round(ven.tot_doc,2)) as Total, frm.des_frp as Pago,CASE 
								   WHEN dev.ref_doc IS NOT NULL AND dev.cod_sts = 5 THEN 'Cancelado'
								   ELSE 'Aplicado'
							   END AS Estado
							  from tblgralventas ven
							  inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI
							  inner join tblauxcaja aux on aux.REF_DOC=ven.REF_DOC
						      inner join tblformaspago frm on frm.COD_FRP=aux.COD_FRP
							  left join tblencdevolucion dev on dev.REF_DOC=ven.ref_doc
							  where CAJA_DOC=9 and (ven.fec_doc like '%{filter}%' or ven.ref_doc like '%{filter}%' or nom_cli like '%{filter}%' or ven.hora_reg like '%{filter}%' or TOT_DOC like '%{filter}%') 
							  and (ven.fec_doc between '{dateA}' and '{dateB}');";

			await Task.Run(() =>
			{
				Invoke(new Action(() =>
				{
					reporte.DataSource = conn.GetTicketsHeader(query);
				}));
			});
		}

		private async void GeneratePdf()
		{
			try
			{
				// Recorre todas las filas seleccionadas
				foreach (DataGridViewRow row in reporte.SelectedRows)
				{
					string pdfPath = $"ticket_{row.Index}.pdf"; // Nombre de archivo único para cada PDF

					// Generar el PDF para cada fila seleccionada
					using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
					{
						Document doc = new Document();
						PdfWriter writer = PdfWriter.GetInstance(doc, fs);

						ClsPageEventHelper pageEventHelper = new ClsPageEventHelper("Imagenes/LOGO_EMPRESA-removebg-preview.png");
						writer.PageEvent = pageEventHelper;

						doc.Open();

						// Título del documento
						Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

						doc.Add(new Paragraph("                         CURLANGO RAMOS CHRISTIAN YARELY \n" +
							"                         R.F.C CURC890920PW1", titleFont)
						{ Alignment = Element.ALIGN_LEFT });
						Paragraph title = new Paragraph("                         LA BAJADITA - VENTA DE FRUTAS Y VERDURAS", titleFont);
						title.Alignment = Element.ALIGN_LEFT;
						doc.Add(title);
						doc.Add(new Paragraph("                         CALLE AVENIDA DE LOS MAESTROS #42 LOCAL 10", titleFont) { Alignment = Element.ALIGN_LEFT });
						doc.Add(new Paragraph("                         COL. JARDINES DEL BOSQUE", titleFont) { Alignment = Element.ALIGN_LEFT });
						doc.Add(new Paragraph("                         C.P. 84063 H. NOGALES, SONORA", titleFont) { Alignment = Element.ALIGN_LEFT });

						doc.Add(new Paragraph("\n"));

						PdfPTable table = new PdfPTable(3);
						table.WidthPercentage = 100;

						float[] columnWidths = new float[] { 1f, 1f, 1f };
						table.SetWidths(columnWidths);

						iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
						PdfPCell headerCell;

						PdfPTable table2 = new PdfPTable(4);
						float[] columnWidths_ = new float[] { 3f, 3f, 3f, 1f };
						table2.SetWidths(columnWidths_);

						headerCell = new PdfPCell(new Phrase($"FECHA: {Convert.ToDateTime(row.Cells[0].Value).ToString("dd/MM/yyyy")}", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table.AddCell(headerCell);
						headerCell = new PdfPCell(new Phrase($"FOLIO: {row.Cells[1].Value}", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table.AddCell(headerCell);
						headerCell = new PdfPCell(new Phrase($"CLIENTE: {row.Cells[2].Value}", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table.AddCell(headerCell);
						doc.Add(table);

						string query = $"select cod_cli from tblgralventas where ref_doc='{row.Cells[1].Value}'";
						string comodin = "";
						await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
						headerCell = new PdfPCell(new Phrase($"NO. CLIENTE: {comodin}.", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table2.AddCell(headerCell);

						query = $"select call_num from tblcatclientes c inner join tblgralventas v on v.COD_CLI=c.cod_cli where v.ref_doc='{row.Cells[1].Value}';";
						comodin = "";
						await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
						headerCell = new PdfPCell(new Phrase($"DIRECCION: {comodin}.", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table2.AddCell(headerCell);

						query = $"select col_cli from tblcatclientes c inner join tblgralventas v on v.COD_CLI=c.cod_cli where v.ref_doc='{row.Cells[1].Value}';";
						comodin = "";
						await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
						headerCell = new PdfPCell(new Phrase($"COLONIA: {comodin}.", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table2.AddCell(headerCell);

						query = $"select cop_cli from tblcatclientes c inner join tblgralventas v on v.COD_CLI=c.cod_cli where v.ref_doc='{row.Cells[1].Value}';";
						comodin = "";
						await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
						headerCell = new PdfPCell(new Phrase($"CP: {comodin}.", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
						table2.AddCell(headerCell);

						doc.Add(table2);

						query = $"select nota from tblgralventas where ref_doc='{row.Cells[1].Value}';";
						string nota = conn.GetValueFromDataBase(query);

						if (nota != "")
						{
							PdfPTable table3 = new PdfPTable(1);


							headerCell = new PdfPCell(new Phrase($"NOTA: {nota}")) { HorizontalAlignment = Element.ALIGN_CENTER, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
							table3.AddCell(headerCell);
							doc.Add(table3);
						}

						doc.Add(new Paragraph("\n"));

						query = $@"
							select ren.cod1_art, round(can_art, 2),  cat.DES1_ART, concat('$',round(pcio_ven,2)), concat('$', round( can_art*pcio_ven,2))
							from tblrenventas ren
							inner join tblcatarticulos cat on cat.cod1_art=ren.cod1_art
							where ref_doc='{row.Cells[1].Value}';";

						DataTable ticket = new DataTable();

						await Task.Run(() => ticket = conn.GetTicketsHeader(query));

						PdfPCell dataCell;
						iTextSharp.text.Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

						PdfPTable ticket_pdf = new PdfPTable(5);
						ticket_pdf.WidthPercentage = 100;

						float[] columnWidths_ticket = new float[] { 1f, 1f, 3f, 1f, 1f }; // Ajusta estos valores según sea necesario
						ticket_pdf.SetWidths(columnWidths_ticket);

						headerCell = new PdfPCell(new Phrase("CODIGO", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
						ticket_pdf.AddCell(headerCell);
						headerCell = new PdfPCell(new Phrase("CANTIDAD", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
						ticket_pdf.AddCell(headerCell);
						headerCell = new PdfPCell(new Phrase("DESCRIPCION", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
						ticket_pdf.AddCell(headerCell);
						headerCell = new PdfPCell(new Phrase("PRECIO", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
						ticket_pdf.AddCell(headerCell);
						headerCell = new PdfPCell(new Phrase("IMPORTE", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
						ticket_pdf.AddCell(headerCell);

						foreach (DataRow r in ticket.Rows)
						{
							dataCell = new PdfPCell(new Phrase($"{r[0]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
							ticket_pdf.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[1]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
							ticket_pdf.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[2]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
							ticket_pdf.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[3]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
							ticket_pdf.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[4]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
							ticket_pdf.AddCell(dataCell);
						}

						doc.Add(ticket_pdf);


						query = $"select round(sub_doc,2) from tblgralventas where ref_doc='{row.Cells[1].Value}'";
						string subtotal = conn.GetValueFromDataBase(query);

						query = $"select round(iva_doc,2) from tblgralventas where ref_doc='{row.Cells[1].Value}'";
						string impuesto = conn.GetValueFromDataBase(query);

						query = $"select round(tot_doc,2) from tblgralventas where ref_doc='{row.Cells[1].Value}'";
						string total = conn.GetValueFromDataBase(query);

						query = $"select ven.NOM_VEN from tblrenventas ren inner join tblvendedores ven on ven.cod_ven = ren.cod_ven where ref_doc='{row.Cells[1].Value}' limit 1;";
						string vendedor = conn.GetValueFromDataBase(query);

						ClsLetras letras = new ClsLetras();
						string l = letras.NumberToWords(Convert.ToDecimal(total));

						FooterPageEventHelper footerEventHelper = new FooterPageEventHelper(subtotal, impuesto, total, vendedor, l);
						writer.PageEvent = footerEventHelper;

						doc.Close();
						writer.Close();

						// Abrir el PDF generado
						Process.Start(pdfPath);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ocurrió un error al generar los PDFs\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void BtnPrintTicket_Click(object sender, EventArgs e)
		{
			GeneratePdf();
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				Invoke(new Action(() => { reporte.DataSource = conn.GetTicketsHeader($"select ven.fec_doc as Fecha,ven.ref_doc as Ticket,cli.NOM_CLI as Nombre, ven.hora_reg as Hora, concat('$', round(ven.tot_doc,2)) as Total, frm.des_frp as Pago, CASE WHEN dev.ref_doc IS NOT NULL AND dev.cod_sts = 5 THEN 'Cancelado' ELSE 'Aplicado' END AS Estado" +
					$" from tblgralventas ven " +
					$"inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI " +
					$" inner join tblauxcaja aux on aux.REF_DOC=ven.REF_DOC " +
					$"inner join tblformaspago frm on frm.COD_FRP=aux.COD_FRP " +
					$" left join tblencdevolucion dev on dev.REF_DOC=ven.ref_doc " +
					$"where CAJA_DOC=9 and ven.fec_doc between '{dateTimePicker1.Value:yyyy-MM-dd}' and '{dateTimePicker2.Value:yyyy-MM-dd}' "); }));
			});
		}

		private async void Form1_Load(object sender, EventArgs e)
		{
			label1.Visible = true;
			await Task.Run(() =>
			{
				Invoke(new Action(() => { reporte.DataSource = conn.GetTicketsHeader($@"select ven.fec_doc as Fecha,ven.ref_doc as Ticket,cli.NOM_CLI as Nombre, ven.hora_reg as Hora, concat('$', round(ven.tot_doc,2)) as Total, frm.des_frp as Pago, CASE 
																							   WHEN dev.ref_doc IS NOT NULL AND dev.cod_sts = 5 THEN 'Cancelado'
																							   ELSE 'Aplicado'
																						   END AS Estado
																						from tblgralventas ven 
																						inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI 
																						inner join tblauxcaja aux on aux.REF_DOC=ven.REF_DOC
																						inner join tblformaspago frm on frm.COD_FRP=aux.COD_FRP
																						left join tblencdevolucion dev on dev.REF_DOC=ven.ref_doc
																						where CAJA_DOC=9 and ven.fec_doc='{DateTime.Now:yyyy-MM-dd}'"); }));
			});
			label1.Visible = false;

			cmbVendedor.Enabled = false;

			DataTable vendedores = new DataTable();

			await Task.Run(() => vendedores = conn.GetTicketsHeader("select cod_ven, nom_ven from tblvendedores;"));

			cmbVendedor.DataSource = vendedores;
			cmbVendedor.DisplayMember = "nom_ven";
			cmbVendedor.ValueMember = "cod_ven";


			cmbVendedor.Enabled = true;
		}

		private string GetSelectedTextFromCombo()
		{
			if (cmbVendedor.SelectedItem != null)
			{
				DataRowView selectedRow = cmbVendedor.SelectedItem as DataRowView;

				if (selectedRow != null)
				{
					string selectedText = selectedRow[cmbVendedor.DisplayMember].ToString();
					return selectedText;
				}
			}
			return null;
		}

		private async void BtnPrintReport_Click(object sender, EventArgs e)
		{
			DataTable tickets = new DataTable();
			string cod = cmbVendedor.SelectedValue.ToString();
			string fechaA = dateTimePicker3.Value.ToString("yyyy-MM-dd");
			string fechaB = dateTimePicker4.Value.ToString("yyyy-MM-dd");

			await Task.Run(() => tickets = conn.GetTicketsHeader($@"select DISTINCT ven.fec_doc, ven.ref_doc, cli.NOM_CLI, hora_reg, concat('$',round(tot_doc,2)), ven.COD_USU, ren.cod_ven
												from tblgralventas ven
												inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI
												inner join tblrenventas ren on ren.REF_DOC=ven.REF_DOC
												where CAJA_DOC=9 
												and (ven.FEC_DOC between '{fechaA}' and '{fechaB}')
												and ren.cod_ven='{cod}';"));

			try
			{
				Document doc = new Document();
				string pdfPath = "reporte.pdf";

				using (FileStream fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					PdfWriter writer = PdfWriter.GetInstance(doc, fs);

					ClsPageEventHelper pageEventHelper = new ClsPageEventHelper("Imagenes/LOGO_EMPRESA-removebg-preview.png");
					writer.PageEvent = pageEventHelper;

					doc.Open();

					// Título del documento
					Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

					doc.Add(new Paragraph("                         CURLANGO RAMOS CHRISTIAN YARELY \n" +
						"                         R.F.C CURC890920PW1", titleFont)
					{ Alignment = Element.ALIGN_LEFT });
					//Paragraph title = new Paragraph("                         LA BAJADITA - VENTA DE FRUTAS Y VERDURAS", titleFont);
					//title.Alignment = Element.ALIGN_LEFT;
					//doc.Add(title);
					doc.Add(new Paragraph("                         CALLE AVENIDA DE LOS MAESTROS #42 LOCAL 10", titleFont) { Alignment = Element.ALIGN_LEFT });
					doc.Add(new Paragraph("                         COL. JARDINES DEL BOSQUE", titleFont) { Alignment = Element.ALIGN_LEFT });
					doc.Add(new Paragraph("                         C.P. 84063 H. NOGALES, SONORA", titleFont) { Alignment = Element.ALIGN_LEFT });

					doc.Add(new Paragraph("\n"));
					doc.Add(new Paragraph($"REPORTE DE TICKETS POR CHOFER\nCHOFER: {GetSelectedTextFromCombo()}\nPERIODO: {dateTimePicker3.Value.ToString("dd/MM/yyyy")} a {dateTimePicker4.Value.ToString("dd/MM/yyyy")}"));
					doc.Add(new Paragraph("\n"));

					PdfPTable table = new PdfPTable(6);
					table.WidthPercentage = 100;

					float[] columnWidths = new float[] { 1f, 2f, 2f, 1f, 1f, 1f };
					table.SetWidths(columnWidths);

					Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
					PdfPCell headerCell;

					headerCell = new PdfPCell(new Phrase("FECHA", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
					table.AddCell(headerCell);
					headerCell = new PdfPCell(new Phrase("FOLIO", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
					table.AddCell(headerCell);
					headerCell = new PdfPCell(new Phrase("CLIENTE", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
					table.AddCell(headerCell);
					headerCell = new PdfPCell(new Phrase("HORA", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
					table.AddCell(headerCell);
					headerCell = new PdfPCell(new Phrase("TOTAL", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
					table.AddCell(headerCell);
					headerCell = new PdfPCell(new Phrase("DEV", headerFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
					table.AddCell(headerCell);

					PdfPCell dataCell;
					Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

					string query = "";
					double devTotal = 0;

					foreach (DataRow r in tickets.Rows)
					{
						query = $@" select fol_dev, round( tot_dev,2)
									from tblencdevolucion dev
									inner join tblgralventas ven on dev.REF_DOC=ven.REF_DOC
									where dev.REF_DOC='{r[1]}'";

						string ro = conn.GetRowQuery(query);

						if (ro == "")
						{
							dataCell = new PdfPCell(new Phrase($"{Convert.ToDateTime(r[0]):dd/MM/yyyy}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[1]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[2]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[3]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[4]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"${0}.00", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f };
							table.AddCell(dataCell);
						}
						else
						{
							devTotal += Convert.ToDouble(ro.Split(',')[1]);

							dataCell = new PdfPCell(new Phrase($"{Convert.ToDateTime(r[0]):dd/MM/yyyy}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f, BackgroundColor = new BaseColor(230, 133, 138) };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[1]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[2]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[3]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"{r[4]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
							table.AddCell(dataCell);
							dataCell = new PdfPCell(new Phrase($"${ro.Split(',')[1]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
							table.AddCell(dataCell);
						}


					}

					double subtotal = 0;

					doc.Add(table);

					foreach (DataRow r in tickets.Rows)
					{
						subtotal += double.Parse(r[4].ToString().Substring(1));
					}


					doc.Add(new Paragraph($"Subtotal: ${subtotal}", titleFont) { Alignment = Element.ALIGN_RIGHT });
					doc.Add(new Paragraph($"Devoluciones: ${devTotal}", titleFont) { Alignment = Element.ALIGN_RIGHT });
					doc.Add(new Paragraph($"Total: ${subtotal - devTotal}", titleFont) { Alignment = Element.ALIGN_RIGHT });

					doc.Close();
					writer.Close();
					Process.Start("reporte.pdf");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "La Bajadita - Venta de Frutas y Verduras");
			}
		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}
	}
}