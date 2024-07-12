using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using AbasteceBarato.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AbasteceBarato.Pages
{
    public class ConsultaSefazModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public string TipoCombustivel { get; set; }
        [BindProperty]
        public string RaioDistancia { get; set; }
        public List<FuelPrice> PrecoCombustiveis { get; set; } = new List<FuelPrice>();

        public const string ApiUrl = "http://api.sefaz.al.gov.br/sfz_nfce_api/api/public/consultarPrecosCombustivel";

        public ConsultaSefazModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public List<(string Text, string Value)> TiposCombustivel { get; set; } = new List<(string, string)>
        {
            ("Gasolina Comum", "1"),
            ("Gasolina Aditivada", "2"),
            ("Álcool", "3"),
            ("Diesel Comum", "4"),
            ("Diesel Aditivado", "5"),
            ("Gás Natural (GNV)", "6")
        };


        public async Task<IActionResult> OnPostAsync()
        {
            string? apiToken = _configuration["AppSettings:ApiToken"];
            if (string.IsNullOrEmpty(TipoCombustivel))
            {
                ModelState.AddModelError(string.Empty, "Por favor, selecione um tipo de combustível.");
                return Page();
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("AppToken", apiToken);
                    var parameters = new
                    {
                        codTipoCombustivel = int.Parse(TipoCombustivel),
                        dias = 1,
                        latitude = -9.5714535,
                        longitude = -35.7734391,
                        raio = int.Parse(RaioDistancia)
                    };


                    string jsonParameters = JsonConvert.SerializeObject(parameters);
                    var content = new StringContent(jsonParameters, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(ApiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        PrecoCombustiveis = JsonConvert.DeserializeObject<List<FuelPrice>>(responseBody);

                        if (PrecoCombustiveis == null || PrecoCombustiveis.Count == 0)
                        {
                            ModelState.AddModelError(string.Empty, "Nenhum dado de preço retornado.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Falha ao consultar preços. Código de status: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocorreu um erro: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }

            return Page();
        }
    }
}
