#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using NordAPI.Model;

namespace NordAPI { 
	public class NordApi
	{
		private readonly XElement? _apiSettings;
		private readonly string? _host;
		private readonly string? _agent;
		private IEnumerable<Server>? _servers;
		
		public NordApi(string apiFile)
		{
			_apiSettings = XDocument.Load(apiFile).Root;
			_host = _apiSettings?.Attribute("host")?.Value;
			_agent = _apiSettings?.Attribute("agent")?.Value;
		}

		private async Task<string?> HttpGetRequest(string url, string? nToken)
		{
			try
			{
				using var client = new HttpClient();
				

				client.DefaultRequestHeaders.Host = _host;
				client.DefaultRequestHeaders.Connection.Add("Close");

				HttpResponseMessage response = await client.GetAsync(url);
				
				response.EnsureSuccessStatusCode();
				
				return await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				return "unauthorized";
			}
		}


		private Task<string?> GetApiItem(string name, IReadOnlyDictionary<string, string> variables)
		{
			XElement? item = (_apiSettings?.Elements("item") ?? Array.Empty<XElement>())
				.FirstOrDefault(x => x.Attribute("name")?.Value == name);
			if (item == null)
				throw new Exception("Api Method " + name + " is not defined");

			var type = item.Attribute("type")?.Value ?? "get";
			var url = item.Element("url")?.Value;
			
			if (url == null)
				throw new Exception("Api call " + name + " url is not defined");

			using var client = new HttpClient();

			IEnumerable<XElement>? parametersNodes = item.Element("url_parameters")?.Elements("url_parameter");
			var vars = new StringBuilder();
			
			switch (type)
			{
				case "get" :
					if (parametersNodes != null)
						foreach (XElement parametersNode in parametersNodes)
						{
							var isVariable = parametersNode.Attribute("type")?.Value == "variable";
							var parameterName = parametersNode.Attribute("name")?.Value;
							
							if (parameterName == null)
								continue;


							if (isVariable)
							{
								if (!variables.ContainsKey(parameterName)) continue;
								
								if (vars.Length > 0)
									vars.Append('&');

								vars.Append(parameterName);
								vars.Append('=');
								vars.Append(variables[parameterName]);
							}
							else
							{
								var parameterValue = parametersNode.Value;
								if (string.IsNullOrEmpty(parameterValue))
									continue;
								
								vars.Append(parameterName);
								vars.Append('=');
								vars.Append(parameterValue);
							}
						}
					break;
			}
	
			return HttpGetRequest(url + vars, null);
		}
		
		
		public async Task<IEnumerable<Server>?> Servers()
		{
			if (_servers != null && _servers.Any())
				return _servers;
			
			_servers = new List<Server>();
			
			var json = await GetApiItem("Servers", new Dictionary<string, string>());

			try
			{
				if (json == null) throw new Exception("error");
			
				_servers = JsonConvert.DeserializeObject<List<Server>>(json);
				return _servers;
			}
		
			catch (Exception ex)
			{
				throw new Exception("Error : " + ex.Message);
			}
		}

		private async Task<Server?> Server (string serverName)
		{
			try
			{
				IEnumerable<Server>? servers = await Servers();

				Server? server = servers?.FirstOrDefault(x => x.name == serverName);

				return server;

			}
		
			catch (Exception ex)
			{
				throw new Exception("Error:" + ex.Message);
			}
		}
		
		/*
		 * Get the individual server load
		 */
		public async Task<int?> ServerLoad(string serverName)
		{
			try
			{
				Server? server = await Server(serverName);

				return server?.load;
			}
			
			catch (Exception ex)
			{
				return -1;
			}
		}
		
		
		/*
		 * Get the individual server load
		 */
		public async Task<IEnumerable<Server>?> RecommendedServers()
		{
			var json = await GetApiItem("Recommendations", new Dictionary<string, string>());

			try
			{
				if (json == null) throw new Exception("error");
			
				return JsonConvert.DeserializeObject<List<Server>>(json);
			}
		
			catch (Exception ex)
			{
				throw new Exception("Error : " + ex.Message);
			}
		}
		
		
		public async Task<Server?> RecommendedServer()
		{
			IEnumerable<Server>? servers = await RecommendedServers();

			return servers?.FirstOrDefault();
		}
	
	}
}