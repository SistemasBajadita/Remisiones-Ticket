using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

				if (rd!=null)
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

		public DataTable GetTicketsHeader(string command)
		{
			DataTable tickets = new DataTable();
			try
			{
				con.Open();
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
				con.Close();
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

				return result.ToString();
			}
			catch (MySqlException ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				con.Close();
			}

			return "";
		}

	}
}
