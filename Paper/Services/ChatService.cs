using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Paper.Services
{
    partial class ChatService
    {
        private readonly string apiKey;
        private readonly RestClient client;
        private readonly List<Message> history;

        public ChatService(string apiKey)
        {
            this.apiKey = apiKey;
            this.client = new RestClient("https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent");
            this.history = new List<Message>();
        }

        public async Task<string> SendMessageAsync(string message)
        {
            try
            {
                // Add user message to history
                history.Add(new Message { Role = "user", Parts = new[] { new Part { Text = message } } });

                var request = new RestRequest("", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("x-goog-api-key", apiKey);

                var payload = new
                {
                    contents = history.ToArray(),
                    generationConfig = new
                    {
                        temperature = 0.7,
                        topK = 40,
                        topP = 0.95,
                        maxOutputTokens = 1024,
                    }
                };

                request.AddJsonBody(JsonConvert.SerializeObject(payload));

                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<GeminiResponse>(response.Content);
                    var aiResponse = result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "Sorry, I couldn't generate a response.";

                    // Add AI response to history
                    history.Add(new Message { Role = "model", Parts = new[] { new Part { Text = aiResponse } } });

                    return aiResponse;
                }
                else
                {
                    return $"Error: {response.ErrorMessage}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private class Message
        {
            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("parts")]
            public Part[] Parts { get; set; }
        }

        private class Part
        {
            [JsonProperty("text")]
            public string Text { get; set; }
        }

        private class GeminiResponse
        {
            [JsonProperty("candidates")]
            public Candidate[] Candidates { get; set; }
        }

        private class Candidate
        {
            [JsonProperty("content")]
            public Content Content { get; set; }
        }

        private class Content
        {
            [JsonProperty("parts")]
            public Part[] Parts { get; set; }
        }
    }
}
