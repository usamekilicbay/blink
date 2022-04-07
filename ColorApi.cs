using System.Net.Http.Json;

namespace blink
{
    public class ColorApi
    {
        private const string _apiUrl = "https://www.colr.org/json/scheme/random";
        private static readonly string[] _defaultColors = { "0a402e", "606fdb", "2173a3", "09ad6c", "1f3442" };

        public static async Task<string[]> GetRandomColors()
        {
            string[] colors = new string[2];

            using (HttpResponseMessage responseMessage = await ApiHelper.ApiClient.GetAsync($"{_apiUrl}"))
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    ColorModel colorModel = await responseMessage.Content.ReadFromJsonAsync<ColorModel>();
                    colors[0] = colorModel.Schemes[0].Colors[0];
                }
                else
                {
                    int randomIndex = new Random().Next(0, _defaultColors.Length);
                    colors[0] = _defaultColors[randomIndex];
                }
            }

            colors[1] = await GetColorByIncomingColorAsync(colors[0]);

            return colors;
        }

        public static async Task<string> GetColorByIncomingColorAsync(string color)
        {
            return await Task.Run(() =>
            {
                Color col = ColorTranslator.FromHtml($"#{color}");

                string returningColor;

                returningColor = (col.R * 0.2126 + col.G * 0.7152 + col.B * 0.0722) < (255 / 2)
                    // If the color is dark set light color
                    ? "D7E0DD"
                    // If the color is light set dark color
                    : "665564";

                return returningColor;
            });
        }
    }
}
