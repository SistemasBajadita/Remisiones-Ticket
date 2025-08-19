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
			string[] units = { "", "UNO", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE", "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISÉIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE" };
			string[] tens = { "", "", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
			string[] hundreds = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

			int integerPart = (int)Math.Floor(number);
			int decimalPart = (int)Math.Round((number - integerPart) * 100);

			string integerPartInWords = ConvertNumberToWords(integerPart, units, tens, hundreds).Trim();
			string decimalPartInWords = decimalPart.ToString("00");

			return $"{integerPartInWords} PESOS CON {decimalPartInWords}/100 M.N.";
		}

		private string ConvertNumberToWords(int number, string[] units, string[] tens, string[] hundreds)
		{
			if (number == 0) return "CERO";

			if (number < 0) return "MENOS " + ConvertNumberToWords(Math.Abs(number), units, tens, hundreds);

			if (number >= 1_000_000)
			{
				if (number / 1_000_000 == 1)
					return "UN MILLÓN " + ConvertNumberToWords(number % 1_000_000, units, tens, hundreds);
				else
					return ConvertNumberToWords(number / 1_000_000, units, tens, hundreds) + " MILLONES " + ConvertNumberToWords(number % 1_000_000, units, tens, hundreds);
			}

			if (number >= 1000)
			{
				if (number / 1000 == 1)
					return "MIL " + ConvertNumberToWords(number % 1000, units, tens, hundreds);
				else
					return ConvertNumberToWords(number / 1000, units, tens, hundreds) + " MIL " + ConvertNumberToWords(number % 1000, units, tens, hundreds);
			}

			if (number >= 100)
			{
				if (number == 100) return "CIEN";
				return hundreds[number / 100] + " " + ConvertNumberToWords(number % 100, units, tens, hundreds);
			}

			if (number >= 30)
			{
				return tens[number / 10] + (number % 10 > 0 ? " Y " + units[number % 10] : "");
			}

			if (number >= 20)
			{
				if (number == 20) return "VEINTE";
				// Manejo especial para 21-29
				string[] veinti = { "VEINTIUNO", "VEINTIDÓS", "VEINTITRÉS", "VEINTICUATRO", "VEINTICINCO",
							"VEINTISÉIS", "VEINTISIETE", "VEINTIOCHO", "VEINTINUEVE" };
				return veinti[number - 21];
			}

			return units[number];
		}

	}
}
