using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Ticket_bonito
{
	internal class ClsConnection
	{
		MySqlConnection con;
		MySqlDataAdapter ad;
		MySqlCommand cmd;

		public string GetRowQuery(string query)
		{
			try
			{
				con.Open();

				MySqlCommand cmd = new MySqlCommand(query, con);
				var rd = cmd.ExecuteScalar();

				if (rd != null)
				{
					con.Close();
					return rd.ToString();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				con.Close();
			}
			return "";
		}

		public ClsConnection(string cadena)
		{
			con = new MySqlConnection(cadena);
			cmd = con.CreateCommand();
			ad = new MySqlDataAdapter(cmd);
		}

		public async Task<DataTable> GetTicketsHeader(string command)
		{
			DataTable tickets = new DataTable();
			try
			{
				await con.OpenAsync();
				cmd.CommandText = command;
				cmd.ExecuteNonQuery();
				ad.Fill(tickets);
			}
			catch (MySqlException e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				await con.CloseAsync();
			}
			return tickets;
		}

		public string GetValueFromDataBase(string query)
		{
			try
			{
				con.Open();
				cmd.CommandText = query;

				object result = cmd.ExecuteScalar();

				con.Close();

				return result.ToString();
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.Message);
				con.Close();
			}

			return "";
		}

		public async Task<bool> UpdateChofer(string chofer, string folio)
		{
			try
			{
				await con.OpenAsync();
				cmd = con.CreateCommand();
				cmd.CommandText = $"update tblrenventas set cod_ven='{chofer}' where ref_doc='{folio}';";
				await cmd.ExecuteNonQueryAsync();
				await con.CloseAsync();
				return true;
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.Message);
				await con.CloseAsync();
				return false;
			}
		}

		public async Task<bool> UpdateNota(string nota, string folio)
		{
			try
			{
				await con.OpenAsync();
				cmd = con.CreateCommand();
				cmd.CommandText = $"update tblgralventas set nota='{nota}' where ref_doc='{folio}'";
				await cmd.ExecuteNonQueryAsync();
				await con.CloseAsync();
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				await con.CloseAsync();
				return false;
			}
		}

	}
}
