using ArtExplorer.BLL.Dtos;
using ArtExplorer.BLL.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ArtExplorer.BLL.Services;

public class MetMuseumService : IMetMuseumService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://collectionapi.metmuseum.org/public/collection/v1";
    public MetMuseumService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(List<ArtDto> Results, int TotalCount)> SearchArtworksAsync(string? author, string? year, int page, int pageSize)
    {
        var searchTerms = new List<string>();
        if (!string.IsNullOrWhiteSpace(author)) searchTerms.Add(author);
        var query = Uri.EscapeDataString(string.Join(" ", searchTerms));

        var searchUrl = $"{BaseUrl}/search?q={query}&departmentId=11";
        var searchResponse = await _httpClient.GetFromJsonAsync<SearchResponse>(searchUrl);

        if (searchResponse?.ObjectIDs == null || !searchResponse.ObjectIDs.Any())
            return (new List<ArtDto>(), 0);

        var pagedIds = searchResponse.ObjectIDs
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .ToList();

        var tasks = pagedIds.Select(GetArtworkByIdAsync);
        var artworks = await Task.WhenAll(tasks);

        var filtered = artworks
            .Where(a => a != null)
            .Where(a => string.IsNullOrWhiteSpace(year) || a!.Year.Contains(year, StringComparison.OrdinalIgnoreCase))
            .ToList()!;

        return (filtered, searchResponse.ObjectIDs.Count);
    }

    public async Task<ArtDto?> GetArtworkByIdAsync(int objectId)
    {
        var response = await _httpClient.GetFromJsonAsync<Root>($"{BaseUrl}/objects/{objectId}");
        if (response == null) return null;

        return new ArtDto
        {
            ObjectID = response.ObjectID,
            Title = response.Title,
            Author = response.ArtistDisplayName,
            Year = response.ObjectDate,
            Style = response.Classification,
            Description = response.Medium,
            ImageUrl = response.PrimaryImageSmall
        };
    }

    private class SearchResponse
    {
        public int Total { get; set; }
        public List<int>? ObjectIDs { get; set; }
    }

    public class Constituent
    {
        [JsonProperty("constituentID", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("constituentID")]
        public int ConstituentID { get; set; }

        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("constituentULAN_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("constituentULAN_URL")]
        public string ConstituentULANURL { get; set; }

        [JsonProperty("constituentWikidata_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("constituentWikidata_URL")]
        public string ConstituentWikidataURL { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }

    public class ElementMeasurements
    {
        [JsonProperty("Height", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("Height")]
        public double Height { get; set; }

        [JsonProperty("Width", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("Width")]
        public double Width { get; set; }

        [JsonProperty("Depth", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("Depth")]
        public double? Depth { get; set; }
    }

    public class Measurement
    {
        [JsonProperty("elementName", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("elementName")]
        public string ElementName { get; set; }

        [JsonProperty("elementDescription", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("elementDescription")]
        public object ElementDescription { get; set; }

        [JsonProperty("elementMeasurements", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("elementMeasurements")]
        public ElementMeasurements ElementMeasurements { get; set; }
    }

    public class Root
    {
        [JsonProperty("objectID", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectID")]
        public int ObjectID { get; set; }

        [JsonProperty("isHighlight", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("isHighlight")]
        public bool IsHighlight { get; set; }

        [JsonProperty("accessionNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("accessionNumber")]
        public string AccessionNumber { get; set; }

        [JsonProperty("accessionYear", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("accessionYear")]
        public string AccessionYear { get; set; }

        [JsonProperty("isPublicDomain", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("isPublicDomain")]
        public bool IsPublicDomain { get; set; }

        [JsonProperty("primaryImage", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("primaryImage")]
        public string PrimaryImage { get; set; }

        [JsonProperty("primaryImageSmall", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("primaryImageSmall")]
        public string PrimaryImageSmall { get; set; }

        [JsonProperty("additionalImages", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("additionalImages")]
        public List<object> AdditionalImages { get; set; }

        [JsonProperty("constituents", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("constituents")]
        public List<Constituent> Constituents { get; set; }

        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonProperty("objectName", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectName")]
        public string ObjectName { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonProperty("culture", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonProperty("period", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("period")]
        public string Period { get; set; }

        [JsonProperty("dynasty", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("dynasty")]
        public string Dynasty { get; set; }

        [JsonProperty("reign", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("reign")]
        public string Reign { get; set; }

        [JsonProperty("portfolio", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("portfolio")]
        public string Portfolio { get; set; }

        [JsonProperty("artistRole", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistRole")]
        public string ArtistRole { get; set; }

        [JsonProperty("artistPrefix", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistPrefix")]
        public string ArtistPrefix { get; set; }

        [JsonProperty("artistDisplayName", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistDisplayName")]
        public string ArtistDisplayName { get; set; }

        [JsonProperty("artistDisplayBio", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistDisplayBio")]
        public string ArtistDisplayBio { get; set; }

        [JsonProperty("artistSuffix", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistSuffix")]
        public string ArtistSuffix { get; set; }

        [JsonProperty("artistAlphaSort", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistAlphaSort")]
        public string ArtistAlphaSort { get; set; }

        [JsonProperty("artistNationality", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistNationality")]
        public string ArtistNationality { get; set; }

        [JsonProperty("artistBeginDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistBeginDate")]
        public string ArtistBeginDate { get; set; }

        [JsonProperty("artistEndDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistEndDate")]
        public string ArtistEndDate { get; set; }

        [JsonProperty("artistGender", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistGender")]
        public string ArtistGender { get; set; }

        [JsonProperty("artistWikidata_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistWikidata_URL")]
        public string ArtistWikidataURL { get; set; }

        [JsonProperty("artistULAN_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("artistULAN_URL")]
        public string ArtistULANURL { get; set; }

        [JsonProperty("objectDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectDate")]
        public string ObjectDate { get; set; }

        [JsonProperty("objectBeginDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectBeginDate")]
        public int ObjectBeginDate { get; set; }

        [JsonProperty("objectEndDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectEndDate")]
        public int ObjectEndDate { get; set; }

        [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("medium")]
        public string Medium { get; set; }

        [JsonProperty("dimensions", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("dimensions")]
        public string Dimensions { get; set; }

        [JsonProperty("measurements", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("measurements")]
        public List<Measurement> Measurements { get; set; }

        [JsonProperty("creditLine", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("creditLine")]
        public string CreditLine { get; set; }

        [JsonProperty("geographyType", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("geographyType")]
        public string GeographyType { get; set; }

        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonProperty("county", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("county")]
        public string County { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonProperty("region", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonProperty("subregion", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("locale", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonProperty("locus", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("locus")]
        public string Locus { get; set; }

        [JsonProperty("excavation", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("excavation")]
        public string Excavation { get; set; }

        [JsonProperty("river", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("river")]
        public string River { get; set; }

        [JsonProperty("classification", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("classification")]
        public string Classification { get; set; }

        [JsonProperty("rightsAndReproduction", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("rightsAndReproduction")]
        public string RightsAndReproduction { get; set; }

        [JsonProperty("linkResource", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("linkResource")]
        public string LinkResource { get; set; }

        [JsonProperty("metadataDate", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("metadataDate")]
        public DateTime MetadataDate { get; set; }

        [JsonProperty("repository", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("repository")]
        public string Repository { get; set; }

        [JsonProperty("objectURL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectURL")]
        public string ObjectURL { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("objectWikidata_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("objectWikidata_URL")]
        public string ObjectWikidataURL { get; set; }

        [JsonProperty("isTimelineWork", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("isTimelineWork")]
        public bool IsTimelineWork { get; set; }

        [JsonProperty("GalleryNumber", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("GalleryNumber")]
        public string GalleryNumber { get; set; }
    }

    public class Tag
    {
        [JsonProperty("term", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("term")]
        public string Term { get; set; }

        [JsonProperty("AAT_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("AAT_URL")]
        public string AATURL { get; set; }

        [JsonProperty("Wikidata_URL", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("Wikidata_URL")]
        public string WikidataURL { get; set; }
    }
}
