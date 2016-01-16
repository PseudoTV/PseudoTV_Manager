using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace PseudoTV_Manager
{
	public sealed class Database
	{
		private static Logger _logger = LogManager.GetCurrentClassLogger();

		private static readonly Database instance = new Database();

		private Database() { }

		public static Database Instance
		{
			get 
			{
				return instance; 
			}
		}

		public void Connect()
		{
			// Code runs.
			Console.WriteLine("Test");
			_logger.Debug("worked");
		}
	}
}
