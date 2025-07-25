﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_bonito
{
	public class FooterPageEventHelper : PdfPageEventHelper
	{
		private string _subtotal;
		private string _impuesto;
		private string _total;
		private string _cajero;
		private string _vendedor;
		private string _letras;

		public FooterPageEventHelper(string subtotal, string impuesto, string total,string vendedor, string letras, string cajero)
		{
			_subtotal = subtotal;
			_impuesto = impuesto;
			_total = total;
			_vendedor = vendedor;
			_letras = letras;
			_cajero = cajero;
		}

		public override void OnEndPage(PdfWriter writer, Document document)
		{
			base.OnEndPage(writer, document);
			PdfPTable footer = new PdfPTable(1);
			footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
			footer.DefaultCell.Border = Rectangle.NO_BORDER;

			iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

			// Crear celdas y añadir el contenido con la alineación correcta
			PdfPCell cell = new PdfPCell(new Paragraph($"Subtotal: ${decimal.Parse(_subtotal):N2}", titleFont)) { Border = Rectangle.TOP_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 5f };
			footer.AddCell(cell);

			cell = new PdfPCell(new Paragraph($"IVA: ${decimal.Parse(_impuesto):N2}", titleFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 5f };
			footer.AddCell(cell);

			cell = new PdfPCell(new Paragraph($"Total: ${decimal.Parse(_total):N2}", titleFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 5f };
			footer.AddCell(cell);

            cell = new PdfPCell(new Paragraph($"Nota realizada por: {_cajero}", titleFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 5f };
            footer.AddCell(cell);

            cell = new PdfPCell(new Paragraph($"Chofer: {_vendedor}", titleFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 5f };
			footer.AddCell(cell);

			cell = new PdfPCell(new Paragraph($"\n{_letras}", titleFont)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 5f };
			footer.AddCell(cell);

			footer.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + 100, writer.DirectContent);
		}
	}
}
