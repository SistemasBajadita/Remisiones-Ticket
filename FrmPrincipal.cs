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
            conn = new ClsConnection(ConfigurationManager.ConnectionStrings["servidor"].ToString());
            string filter = TxtFiltro.Text;
            string dateA = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dateB = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string query = $@"select distinct ven.fec_doc as Fecha,ven.ref_doc as Folio,cli.NOM_CLI as Nombre, ven.hora_reg as Hora, concat('$', round(ven.tot_doc - coalesce(sum(dev.tot_dev), 0),2)) as Total, frm.des_frp as Pago,CASE 
								   WHEN dev.TOT_DEV=ven.tot_doc AND dev.cod_sts = 5 THEN 'Cancelado'
									when dev.tot_dev<ven.tot_doc and dev.cod_sts = 5 then 'Devolucion'
									WHEN ven.sts_doc=3 then 'Facturado'
								   ELSE 'Aplicado'
							   END AS Estado
							  from tblgralventas ven
							  inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI
							  inner join tblauxcaja aux on aux.REF_DOC=ven.REF_DOC
						      inner join tblformaspago frm on frm.COD_FRP=aux.COD_FRP
							  left join tblencdevolucion dev on dev.REF_DOC=ven.ref_doc
							  where CAJA_DOC=9 and (ven.fec_doc like '%{filter}%' or ven.ref_doc like '%{filter}%' or nom_cli like '%{filter}%' or ven.hora_reg like '%{filter}%' or TOT_DOC like '%{filter}%') 
							  and (ven.fec_doc between '{dateA}' and '{dateB}')
							group by ven.ref_doc;";

            DataTable temp = await conn.GetTicketsHeader(query);

            reporte.DataSource = temp;
            CountTickets(temp.Rows.Count);
        }

        private async void GeneratePdf()
        {
            try
            {
                // Recorre todas las filas seleccionadas
                foreach (DataGridViewRow row in reporte.SelectedRows)
                {
                    string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fuentes", "Montserrat-Bold.ttf");

                    // Registrar y crear la fuente
                    BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    Font customFont = new Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

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
                            "                         R.F.C CURC890920PW1", customFont)
                        { Alignment = Element.ALIGN_LEFT });
                        Paragraph title = new Paragraph("                         LA BAJADITA - VENTA DE FRUTAS Y VERDURAS", customFont)
                        {
                            Alignment = Element.ALIGN_LEFT
                        };
                        doc.Add(title);
                        doc.Add(new Paragraph("                         CALLE AVENIDA DE LOS MAESTROS #42 LOCAL 10", customFont) { Alignment = Element.ALIGN_LEFT });
                        doc.Add(new Paragraph("                         COL. JARDINES DEL BOSQUE", customFont) { Alignment = Element.ALIGN_LEFT });
                        doc.Add(new Paragraph("                         C.P. 84063 H. NOGALES, SONORA", customFont) { Alignment = Element.ALIGN_LEFT });

                        doc.Add(new Paragraph("\n"));

                        PdfPTable table = new PdfPTable(3)
                        {
                            WidthPercentage = 100
                        };

                        float[] columnWidths = new float[] { 1f, 1f, 1f };
                        table.SetWidths(columnWidths);

                        customFont.Size = 10;

                        iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                        PdfPCell headerCell;

                        PdfPTable table2 = new PdfPTable(4);
                        float[] columnWidths_ = new float[] { 3f, 3f, 3f, 1f };
                        table2.SetWidths(columnWidths_);

                        headerCell = new PdfPCell(new Phrase($"FECHA: {Convert.ToDateTime(row.Cells[0].Value).ToString("dd/MM/yyyy")}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
                        table.AddCell(headerCell);
                        headerCell = new PdfPCell(new Phrase($"FOLIO: {row.Cells[1].Value}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
                        table.AddCell(headerCell);
                        headerCell = new PdfPCell(new Phrase($"CLIENTE: {row.Cells[2].Value}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
                        table.AddCell(headerCell);
                        doc.Add(table);

                        doc.AddTitle($"{row.Cells[2].Value} - {row.Cells[1].Value}");

                        string query = $"select cod_cli from tblgralventas where ref_doc='{row.Cells[1].Value}'";
                        string comodin = "";
                        await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
                        headerCell = new PdfPCell(new Phrase($"NO. CLIENTE: {comodin}.", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
                        table2.AddCell(headerCell);

                        query = $"select call_num from tblcatclientes c inner join tblgralventas v on v.COD_CLI=c.cod_cli where v.ref_doc='{row.Cells[1].Value}';";
                        comodin = "";
                        await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
                        headerCell = new PdfPCell(new Phrase($"DIRECCION: {comodin}.", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
                        table2.AddCell(headerCell);

                        query = $"select col_cli from tblcatclientes c inner join tblgralventas v on v.COD_CLI=c.cod_cli where v.ref_doc='{row.Cells[1].Value}';";
                        comodin = "";
                        await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
                        headerCell = new PdfPCell(new Phrase($"COLONIA: {comodin}.", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
                        table2.AddCell(headerCell);

                        query = $"select cop_cli from tblcatclientes c inner join tblgralventas v on v.COD_CLI=c.cod_cli where v.ref_doc='{row.Cells[1].Value}';";
                        comodin = "";
                        await Task.Run(() => comodin = conn.GetValueFromDataBase(query));
                        headerCell = new PdfPCell(new Phrase($"CP: {comodin}.", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.NO_BORDER, PaddingBottom = 10f };
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

                        query = $@"select ren.cod1_art, round(sum(ren.can_art) - coalesce(sum(devr.can_dev), 0), 2),  cat.DES1_ART, concat('$',round(pcio_ven,2)), concat('$', round((sum(can_art)*pcio_ven) - coalesce(sum(devr.can_dev*devr.pcio_art),0),2)), ren.cod_und
									from tblrenventas ren
									inner join tblcatarticulos cat on cat.cod1_art=ren.cod1_art
									left join tblencdevolucion dev on dev.REF_DOC=ren.REF_DOC
									left join tblrendevolucion devr on devr.FOL_DEV=dev.FOL_DEV and ren.COD1_ART=devr.cod1_Art
									where ren.ref_doc='{row.Cells[1].Value}'
									group by ren.cod1_art, ren.pcio_ven;";

                        DataTable ticket = new DataTable();

                        await Task.Run(async () => ticket = await conn.GetTicketsHeader(query));

                        PdfPCell dataCell;
                        iTextSharp.text.Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);



                        PdfPTable ticket_pdf = new PdfPTable(5);
                        ticket_pdf.WidthPercentage = 100;

                        float[] columnWidths_ticket = new float[] { 1f, 1f, 3f, 1f, 1f }; // Ajusta estos valores según sea necesario
                        ticket_pdf.SetWidths(columnWidths_ticket);

                        headerCell = new PdfPCell(new Phrase("CODIGO", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                        ticket_pdf.AddCell(headerCell);
                        headerCell = new PdfPCell(new Phrase("CANTIDAD", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                        ticket_pdf.AddCell(headerCell);
                        headerCell = new PdfPCell(new Phrase("DESCRIPCION", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                        ticket_pdf.AddCell(headerCell);
                        headerCell = new PdfPCell(new Phrase("PRECIO", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                        ticket_pdf.AddCell(headerCell);
                        headerCell = new PdfPCell(new Phrase("IMPORTE", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                        ticket_pdf.AddCell(headerCell);

                        fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fuentes", "Montserrat-Regular.ttf");
                        bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        customFont = new Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                        foreach (DataRow r in ticket.Rows)
                        {
                            if (double.Parse(r[1].ToString()) == 0)
                            {
                                continue;
                            }

                            dataCell = new PdfPCell(new Phrase($"{r[0]}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                            ticket_pdf.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[1]}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                            ticket_pdf.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[2]}  ({r[5]})", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                            ticket_pdf.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[3]}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                            ticket_pdf.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"${decimal.Parse(r[4].ToString().Substring(1)):N2}", customFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 10f };
                            ticket_pdf.AddCell(dataCell);
                        }

                        doc.Add(ticket_pdf);

                        query = $"select frp.DES_FRP, IMP_MBA from tblauxcaja aux inner join tblformaspago frp on frp.cod_frp=aux.cod_frp where ref_doc='{row.Cells[1].Value}'";
                        DataTable formasDePago = await conn.GetTicketsHeader(query);

                        if (formasDePago.Rows.Count == 1)
                            doc.Add(new Paragraph($"Forma de pago: {formasDePago.Rows[0][0]}", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        else
                        {
                            doc.Add(new Paragraph($"Formas de pago", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                            foreach (DataRow r in formasDePago.Rows)
                            {
                                doc.Add(new Paragraph($"{r[0]}: {r[1]}", FontFactory.GetFont(FontFactory.HELVETICA, 8)));
                            }
                        }


                        query = $"select group_concat(fol_dev Separator ' - ') from tblencdevolucion where ref_doc='{row.Cells[1].Value}'";
                        string devoluciones = conn.GetValueFromDataBase(query);

                        if (devoluciones != "")
                            doc.Add(new Paragraph($"Devoluciones relacionadas: {devoluciones}", FontFactory.GetFont(FontFactory.HELVETICA, 8)));


                        query = $"select round(gral.sub_doc - coalesce(sum(dev.sub_dev), 0),2) from tblgralventas gral left join tblencdevolucion dev on dev.REF_DOC=gral.ref_doc where gral.ref_doc='{row.Cells[1].Value}' group by gral.ref_doc";
                        string subtotal = conn.GetValueFromDataBase(query);

                        query = $"select round(gral.iva_doc - coalesce(sum(dev.iva_dev), 0),2) from tblgralventas gral left join tblencdevolucion dev on dev.ref_doc=gral.REF_DOC where gral.ref_doc='{row.Cells[1].Value}' group by gral.ref_doc";
                        string impuesto = conn.GetValueFromDataBase(query);

                        query = $"select round(gral.tot_doc - coalesce(sum(dev.tot_dev), 0),2) from tblgralventas gral left join tblencdevolucion dev on dev.ref_doc=gral.ref_doc where gral.ref_doc='{row.Cells[1].Value}' group by gral.ref_doc";
                        string total = conn.GetValueFromDataBase(query);

                        query = $"select ven.NOM_VEN from tblrenventas ren inner join tblvendedores ven on ven.cod_ven = ren.cod_ven where ref_doc='{row.Cells[1].Value}' limit 1;";
                        string vendedor = conn.GetValueFromDataBase(query);

                        query = $"select usu.NOM_USU from tblgralventas ven inner join tblusuarios usu on usu.COD_USU=ven.cod_usu where ven.REF_DOC='{row.Cells[1].Value}';";
                        string cajero = conn.GetValueFromDataBase(query);

                        ClsLetras letras = new ClsLetras();
                        string l = letras.NumberToWords(Convert.ToDecimal(total));

                        FooterPageEventHelper footerEventHelper = new FooterPageEventHelper(subtotal, impuesto, total, vendedor, l, cajero);
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

        //Este evento esta asignado a ambos dateTimePickers
        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            conn = new ClsConnection(ConfigurationManager.ConnectionStrings["servidor"].ToString());
            DataTable rep = await conn.GetTicketsHeader($"select distinct ven.fec_doc as Fecha,ven.ref_doc as Ticket,cli.NOM_CLI as Nombre, ven.hora_reg as Hora, concat('$', round(ven.tot_doc - coalesce(sum(dev.tot_dev), 0),2)) as Total, frm.des_frp as Pago, CASE WHEN dev.TOT_DEV=ven.tot_doc AND dev.cod_sts = 5 THEN 'Cancelado' when dev.tot_dev<ven.tot_doc and dev.cod_sts = 5 then 'Devolucion' WHEN ven.sts_doc=3 then 'Facturado' ELSE 'Aplicado' END AS Estado" +
                $" from tblgralventas ven " +
                $"inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI " +
                $" inner join tblauxcaja aux on aux.REF_DOC=ven.REF_DOC " +
                $"inner join tblformaspago frm on frm.COD_FRP=aux.COD_FRP " +
                $" left join tblencdevolucion dev on dev.REF_DOC=ven.ref_doc " +
                $"where CAJA_DOC=9 and ven.fec_doc between '{dateTimePicker1.Value:yyyy-MM-dd}' and '{dateTimePicker2.Value:yyyy-MM-dd}' " +
                $"group by ven.ref_doc");

            reporte.DataSource = rep;
            CountTickets(rep.Rows.Count);

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            BtnUpdateTickets.Enabled = false;
            reporte.DataSource = null;
            label1.Visible = true;
            DataTable result = await conn.GetTicketsHeader($@"select distinct ven.fec_doc as Fecha,ven.ref_doc as Ticket,cli.NOM_CLI as Nombre, ven.hora_reg as Hora, concat('$', round(ven.tot_doc - coalesce(sum(dev.tot_dev), 0),2)) as Total, frm.des_frp as Pago, CASE 
																							   WHEN dev.TOT_DEV=ven.tot_doc AND dev.cod_sts = 5 THEN 'Cancelado'
																								when dev.tot_dev<ven.tot_doc and dev.cod_sts = 5 then 'Devolucion'
																								WHEN ven.sts_doc=3 then 'Facturado'
																							   ELSE 'Aplicado'
																						   END AS Estado
																						from tblgralventas ven 
																						inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI 
																						inner join tblauxcaja aux on aux.REF_DOC=ven.REF_DOC
																						inner join tblformaspago frm on frm.COD_FRP=aux.COD_FRP
																						left join tblencdevolucion dev on dev.REF_DOC=ven.ref_doc
																						where CAJA_DOC=9 and ven.fec_doc between '{dateTimePicker1.Value:yyyy-MM-dd}' and '{dateTimePicker2.Value:yyyy-MM-dd}'
																						group by ven.ref_doc;");
            reporte.DataSource = result;

            CountTickets(result.Rows.Count);

            label1.Visible = false;

            cmbVendedor.Enabled = false;

            DataTable vendedores = await conn.GetTicketsHeader("select cod_ven, nom_ven from tblvendedores where cod_sts=1;");

            cmbVendedor.DataSource = vendedores;
            cmbVendedor.DisplayMember = "nom_ven";
            cmbVendedor.ValueMember = "cod_ven";

            cmbVendedorChange.DataSource = vendedores;
            cmbVendedorChange.DisplayMember = "nom_ven";
            cmbVendedorChange.ValueMember = "cod_ven";

            cmbVendedor.Enabled = true;

            BtnUpdateTickets.Enabled = true;
        }

        private void CountTickets(int num)
        {
            lblTIcketCount.Text = $"Numero de tickets: {num}";
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
            string cod = cmbVendedor.SelectedValue.ToString();
            string fechaA = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            string fechaB = dateTimePicker4.Value.ToString("yyyy-MM-dd");

            DataTable tickets = await conn.GetTicketsHeader($@"select DISTINCT ven.fec_doc, ven.ref_doc, cli.NOM_CLI, hora_reg, round(tot_doc,2), ven.COD_USU, ren.cod_ven
												                    from tblgralventas ven
												                    inner join tblcatclientes cli on cli.COD_Cli=ven.COD_CLI
												                    inner join tblrenventas ren on ren.REF_DOC=ven.REF_DOC
												                    where CAJA_DOC=9 
												                    and (ven.FEC_DOC between '{fechaA}' and '{fechaB}')
												                    and ren.cod_ven='{cod}';");

            try
            {
                Document doc = new Document();
                string pdfPath = "reporte de venta.pdf";

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
                        query = $@" select round( SUM(tot_dev),2)
									from tblencdevolucion dev
									inner join tblgralventas ven on dev.REF_DOC=ven.REF_DOC
									where dev.REF_DOC='{r[1]}' and cod_sts=5;";

                        string ro = conn.GetRowQuery(query);

                        bool creditFlag = false;

                        if (conn.GetValueFromDataBase($"select cod_frp from tblauxcaja where ref_doc = '{r[1]}'") == "5")
                        {
                            creditFlag = true;
                        }


                        if (ro == "")
                        {
                            dataCell = new PdfPCell(new Phrase($"{Convert.ToDateTime(r[0]):dd/MM/yyyy}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f, BackgroundColor = creditFlag ? new BaseColor(255, 255, 0) : new BaseColor(255, 255, 255) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[1]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = creditFlag ? new BaseColor(255, 255, 0) : new BaseColor(255, 255, 255) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[2]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = creditFlag ? new BaseColor(255, 255, 0) : new BaseColor(255, 255, 255) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[3]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = creditFlag ? new BaseColor(255, 255, 0) : new BaseColor(255, 255, 255) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"${Convert.ToDouble(r[4]):N2}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = creditFlag ? new BaseColor(255, 255, 0) : new BaseColor(255, 255, 255) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"${0}.00", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = creditFlag ? new BaseColor(255, 255, 0) : new BaseColor(255, 255, 255) };
                            table.AddCell(dataCell);
                        }
                        else
                        {
                            devTotal += Convert.ToDouble(Convert.ToDouble(ro));

                            dataCell = new PdfPCell(new Phrase($"{Convert.ToDateTime(r[0]):dd/MM/yyyy}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 5f, BackgroundColor = new BaseColor(230, 133, 138) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[1]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[2]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"{r[3]}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"${Convert.ToDouble(r[4]):N2}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
                            table.AddCell(dataCell);
                            dataCell = new PdfPCell(new Phrase($"${Convert.ToDouble(ro):N2}", dataFont)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = PdfPCell.BOTTOM_BORDER, PaddingBottom = 2f, BackgroundColor = new BaseColor(230, 133, 138) };
                            table.AddCell(dataCell);
                        }
                    }

                    double subtotal = 0;
                    double credito = 0;

                    doc.Add(table);

                    foreach (DataRow r in tickets.Rows)
                    {
                        query = $"select cod_frp from tblauxcaja where ref_doc = '{r[1]}'";
                        if (conn.GetValueFromDataBase(query) == "5")
                        {
                            query = $@" select round( SUM(tot_dev),2)
									from tblencdevolucion dev
									inner join tblgralventas ven on dev.REF_DOC=ven.REF_DOC
									where dev.REF_DOC='{r[1]}' and cod_sts=5;";
                            if (conn.GetRowQuery(query) == "")
                                credito += double.Parse(r[4].ToString());
                            else
                                credito += double.Parse(r[4].ToString()) - double.Parse(conn.GetRowQuery(query));
                        }
                        subtotal += double.Parse(r[4].ToString());
                    }

                    doc.Add(new Paragraph(Environment.NewLine));

                    PdfPTable colores = new PdfPTable(1)
                    {
                        WidthPercentage = 20,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    colores.AddCell(new PdfPCell(new Phrase("Devolucion")) { BackgroundColor = new BaseColor(230, 133, 138) });
                    colores.AddCell(new PdfPCell(new Phrase("Credito")) { BackgroundColor = new BaseColor(255, 255, 0) });
                    doc.Add(colores);

                    doc.Add(new Paragraph($"Subtotal: ${Convert.ToDouble(subtotal):N2}", titleFont) { Alignment = Element.ALIGN_RIGHT });
                    doc.Add(new Paragraph($"Credito: ${credito:N2}", titleFont) { Alignment = Element.ALIGN_RIGHT });
                    doc.Add(new Paragraph($"Devoluciones: ${Convert.ToDouble(devTotal):N2}", titleFont) { Alignment = Element.ALIGN_RIGHT });
                    doc.Add(new Paragraph($"Total a entregar: ${Convert.ToDouble(subtotal - devTotal - credito):N2}\n\n", titleFont) { Alignment = Element.ALIGN_RIGHT });



                    doc.Close();
                    writer.Close();
                    Process.Start(pdfPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "La Bajadita - Venta de Frutas y Verduras");
            }
        }

        private async void reporte_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string folio = reporte.Rows[reporte.SelectedRows[0].Index].Cells[1].Value.ToString();
            Clipboard.SetText(folio);

            label1.Visible = true;
            label1.Text = "Folio copiado al portapapeles";
            await Task.Delay(3000);
            label1.Text = "";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemOpenBrackets)
            {
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode == Keys.Enter && TxtFolioChofer.Text != "")
            {
                var caja = conn.GetValueFromDataBase($"select caja_doc from tblgralventas where ref_doc='{TxtFolioChofer.Text}'");

                if (caja == "9")
                {
                    var vendedor = conn.GetValueFromDataBase($"select cod_ven from tblrenventas where ref_doc='{TxtFolioChofer.Text}'");

                    cmbVendedorChange.SelectedValue = vendedor;
                }
                else
                {
                    MessageBox.Show("No se pueden actualizar tickets de otras cajas, solo de la caja de remisiones", "OJO!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtFolioChofer.Text = "";
                }
            }
        }

        private async void BtnUpdateChofer_Click(object sender, EventArgs e)
        {
            bool result = await conn.UpdateChofer(cmbVendedorChange.SelectedValue.ToString(), TxtFolioChofer.Text);

            if (result)
            {
                lblChoferUpdate.Text = "Ticket actualizado exitosamente";
                lblChoferUpdate.BackColor = System.Drawing.Color.Green;
                lblChoferUpdate.ForeColor = System.Drawing.Color.White;

                await Task.Delay(3000);

                lblChoferUpdate.Text = "";
                TxtFolioChofer.Text = "";

            }
            else
            {
                lblChoferUpdate.Text = "Ocurrio un error al intentar actualizar el ticket, vuelve a intentarlo";
                lblChoferUpdate.BackColor = System.Drawing.Color.Red;
                lblChoferUpdate.ForeColor = System.Drawing.Color.White;

                await Task.Delay(3000);

                lblChoferUpdate.Text = "";
            }
        }

        private void TxtFolioNota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var caja = conn.GetValueFromDataBase($"select caja_doc from tblgralventas where ref_doc='{TxtFolioNota.Text}'");

                if (caja == "9")
                {
                    var nota = conn.GetValueFromDataBase($"select nota from tblgralventas where ref_doc='{TxtFolioNota.Text}'");

                    TxtNota.Text = nota;
                }
                else
                {
                    MessageBox.Show("No se pueden actualizar tickets de otras cajas, solo de la caja de remisiones", "OJO!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtFolioChofer.Text = "";
                }
            }
        }

        private async void BtnUpdateNota_Click(object sender, EventArgs e)
        {
            bool result = await conn.UpdateNota(TxtNota.Text, TxtFolioNota.Text);

            if (result)
            {
                lblNota.Text = "Nota actualizada exitosamente";
                lblNota.BackColor = System.Drawing.Color.Green;
                lblNota.ForeColor = System.Drawing.Color.White;

                await Task.Delay(3000);

                lblNota.Text = "";
                TxtFolioNota.Text = "";
                TxtNota.Text = "";
            }
            else
            {
                lblNota.Text = "Ocurrio un error al actualizar la nota, intente de nuevo";
                lblNota.BackColor = System.Drawing.Color.Red;
                lblNota.ForeColor = System.Drawing.Color.White;

                await Task.Delay(3000);

                lblNota.Text = "";
            }
        }

        private void BtnUpdateTickets_Click(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }

        private void TxtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemOpenBrackets)
            {
                e.SuppressKeyPress = true;
                return;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}