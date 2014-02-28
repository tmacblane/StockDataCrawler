using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investing.DBModel;

namespace DataCrawler
{
	public class StockSymbolImport
	{
		public static void Main()
		{
			ImportStockSymbolData("nyse_companylist.xls");
		}

		public static void ImportStockSymbolData(string fileName)
		{
			string connectionString = string.Empty;

			if(fileName.EndsWith("xls"))
			{
				connectionString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Temp\StockDataCrawler\{0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=2'", fileName);
			}
			else if(fileName.EndsWith("xlsx"))
			{
				connectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Temp\StockDataCrawler\{0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'", fileName);
			}

			using(OleDbConnection connection = new OleDbConnection())
			{
				connection.ConnectionString = connectionString;
				connection.Open();

				DataTable dataTable = new DataTable();

				using(OleDbCommand command = new OleDbCommand())
				{
					command.CommandText = "select * from [NYSE$]";
					command.Connection = connection;

					using(OleDbDataAdapter dataAdapter = new OleDbDataAdapter())
					{
						dataAdapter.SelectCommand = command;
						dataAdapter.Fill(dataTable);
					}
				}

				using(InvestingEntities investingEntities = new InvestingEntities())
				{
					string sectorName = string.Empty;
					string industryName = string.Empty;
					string stockSymbol = string.Empty;
					Guid? sectorId = null;
					Guid? industryId = null;
					Exchange exchangeEntity;
					Sector sectorEntity;
					Industry industryEntity;
					Stock stockEntity;

					foreach(DataRow dataRow in dataTable.Rows)
					{
						exchangeEntity = investingEntities.Exchanges.AsNoTracking().Where(e => e.Name == "NYSE").FirstOrDefault();

						// Check if Exchange Exists
						if(exchangeEntity == null)
						{
							exchangeEntity = new Exchange
							{
								Name = "NYSE"
							};

							investingEntities.Exchanges.Add(exchangeEntity);
							investingEntities.SaveChanges();
						}

						sectorName = dataRow[3].ToString();

						if(sectorName != string.Empty)
						{
							sectorEntity = investingEntities.Sectors.AsNoTracking().Where(s => s.Name == sectorName).FirstOrDefault();

							// Check if Sector Exists
							if(sectorEntity == null)
							{
								sectorEntity = new Sector
								{
									Id = Guid.NewGuid(),
									Name = sectorName
								};

								investingEntities.Sectors.Add(sectorEntity);
								investingEntities.SaveChanges();
							}

							sectorId = sectorEntity.Id;
						}

						industryName = dataRow[4].ToString();

						if(industryName != string.Empty)
						{
							industryEntity = investingEntities.Industries.AsNoTracking().Where(i => i.Name == industryName).FirstOrDefault();

							// Check if Industry Exists
							if(industryEntity == null)
							{
								industryEntity = new Industry
								{
									Id = Guid.NewGuid(),
									Name = industryName
								};

								investingEntities.Industries.Add(industryEntity);
								investingEntities.SaveChanges();
							}

							industryId = industryEntity.Id;
						}

						stockSymbol = dataRow[0].ToString();

						stockEntity = investingEntities.Stocks.AsNoTracking().Where(i => i.Symbol == stockSymbol).FirstOrDefault();

						if(stockEntity == null)
						{
							stockEntity = new Stock
							{
								Id = Guid.NewGuid(),
								Symbol = stockSymbol,
								CompanyName = dataRow[1].ToString(),
								IPOYear = dataRow[2].ToString(),
								ExchangeId = exchangeEntity.Id,
								SectorId = sectorId,
								IndustryId = industryId,
								isActive = true
							};

							investingEntities.Stocks.Add(stockEntity);
							investingEntities.SaveChanges();
						}
					}
				}
			}

			Console.WriteLine("FIN");
			Console.ReadLine();
		}
	}
}