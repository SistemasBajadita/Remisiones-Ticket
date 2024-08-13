using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_bonito
{
	internal class ClsLetras
	{
		public string NumberToWords(decimal number)
		{
			string[] units = { "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE", "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISEIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE" };
			string[] tens = { "", "", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
			string[] hundreds = { "", "CIEN", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

			int integerPart = (int)Math.Floor(number);
			int decimalPart = (int)((number - integerPart) * 100);

			string integerPartInWords = ConvertNumberToWords(integerPart, units, tens, hundreds);
			string decimalPartInWords = decimalPart.ToString("00");

			return $"{integerPartInWords} PESOS CON {decimalPartInWords}/100 M.N.";
		}

		private string ConvertNumberToWords(int number, string[] units, string[] tens, string[] hundreds)
		{
			if (number == 0)
			{
				return "CERO";
			}
			if (number >= 1000)
			{
				return ConvertNumberToWords(number / 1000, units, tens, hundreds) + " MIL " + ConvertNumberToWords(number % 1000, units, tens, hundreds);
			}
			if (number >= 100)
			{
				return hundreds[number / 100] + " " + ConvertNumberToWords(number % 100, units, tens, hundreds);
			}
			if (number >= 20)
			{
				return tens[number / 10] + (number % 10 > 0 ? " Y " + ConvertNumberToWords(number % 10, units, tens, hundreds) : "");
			}
			return units[number];
		}

	}
}
