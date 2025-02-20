using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Paper.Models;

namespace Paper.Services
{
    public class GeminiService
    {
        private readonly string apiKey;
        private readonly HttpClient client;

        public GeminiService()
        {
            apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
            client = new HttpClient();
        }

        public async Task<string> GetSummaryFromGemini(string pdfText)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = $"Summarize the following text in a clear and concise way:\n{pdfText}"
                                }
                            }
                        }
                    }
                };

                string jsonBody = JsonSerializer.Serialize(requestBody);
                var response = await client.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}",
                    new StringContent(jsonBody, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                JsonDocument document = JsonDocument.Parse(responseContent);
                var root = document.RootElement;

                // Extract the text from the response
                var text = root
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "No summary generated.";
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API request failed: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse API response: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        public async Task<string> ChatWithGemini(string message)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = message }
                            }
                        }
                    }
                };

                string jsonBody = JsonSerializer.Serialize(requestBody);
                var response = await client.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}",
                    new StringContent(jsonBody, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                JsonDocument document = JsonDocument.Parse(responseContent);
                var root = document.RootElement;

                // Extract the text from the response
                var text = root
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "No response generated.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get response from Gemini: {ex.Message}");
            }
        }

        public async Task<List<Flashcard>> GenerateFlashcardsFromText(string pdfText)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = $@"Generate 8 flashcards from this text. Format each flashcard as 'Q: question | A: answer'.
                                    Make the questions test key concepts and understanding.
                                    Text: {pdfText}"
                                }
                            }
                        }
                    }
                };

                string jsonBody = JsonSerializer.Serialize(requestBody);
                var response = await client.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}",
                    new StringContent(jsonBody, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                JsonDocument document = JsonDocument.Parse(responseContent);
                var text = document.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // Parse the flashcards from the text
                return ParseFlashcards(text ?? "");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate flashcards: {ex.Message}");
            }
        }

        private List<Flashcard> ParseFlashcards(string text)
        {
            var flashcards = new List<Flashcard>();
            var lines = text.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.Contains("Q:") && line.Contains("|") && line.Contains("A:"))
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        var question = parts[0].Replace("Q:", "").Trim();
                        var answer = parts[1].Replace("A:", "").Trim();
                        flashcards.Add(new Flashcard { Question = question, Answer = answer });
                    }
                }
            }

            return flashcards;
        }
    }
}
