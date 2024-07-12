using Newtonsoft.Json;

namespace AbasteceBarato.Models
{
    public class FuelPrice
    {

        [JsonProperty("dscProduto")]
        public string? DescricaoProduto { get; set; }

        [JsonProperty("valUltimaVenda")]
        public double? ValorUltimaVenda { get; set; }

        [JsonProperty("nomRazaoSocial")]
        public string? NomeRazaoSocial { get; set; }

        [JsonProperty("nomFantasia")]
        public string? NomeFantasia { get; set; }

        [JsonProperty("nomLogradouro")]
        public string? NomeLogradouro { get; set; }

        [JsonProperty("numImovel")]
        public string? NumeroImovel { get; set; }

        [JsonProperty("dthEmissaoUltimaVenda")]
        public string? DataEmissaoUltimaVenda { get; set; }

        [JsonProperty("nomBairro")]
        public string? NomeBairro { get; set; }

        [JsonProperty("numLatitude")]
        public double? NumLatitude { get; set; }

        [JsonProperty("numLongitude")]
        public double? NumLongitude { get; set; }
    }
}