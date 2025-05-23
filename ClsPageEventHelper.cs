﻿using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Ticket_bonito
{
	//Esta clase es para poder insertar imagenes en el pdf
	public class ClsPageEventHelper : PdfPageEventHelper
	{
		private Image _headerImage;

		public ClsPageEventHelper(string imagePath)
		{
			_headerImage = Image.GetInstance(imagePath);
			_headerImage.ScaleToFit(160f, 110f); // Ajusta el tamaño de la imagen si es necesario
			_headerImage.SetAbsolutePosition(10, (PageSize.A4.Height - _headerImage.ScaledHeight - 30));
		}

		public ClsPageEventHelper(string imagePath, float witdh, float height)
		{
			_headerImage = Image.GetInstance(imagePath);
			_headerImage.ScaleToFit(witdh, height); // Ajusta el tamaño de la imagen si es necesario
			_headerImage.SetAbsolutePosition(10, PageSize.A4.Height - _headerImage.ScaledHeight - 10);
		}

		public override void OnEndPage(PdfWriter writer, Document document)
		{
			PdfContentByte cb = writer.DirectContent;
			cb.AddImage(_headerImage);
		}
	}
}
