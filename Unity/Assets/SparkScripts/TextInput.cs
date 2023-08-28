using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;

public class TextInput : MonoBehaviour
{
    [SerializeField] private Translate translate;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text outputText;
    private string apiKey = "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";

    public class Request
    {
        [JsonProperty("model")]
        public string Model { get; set; } = "gpt-3.5-turbo";
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; } = 500;
        [JsonProperty("messages")]
        public RequestMessage[] Messages { get; set; }
    }

    public class RequestMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Root
    {
        [JsonProperty("role")]
        public string ID { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        public int created { get; set; }
        public List<Choice> choices { get; set; }
        public Usage usage { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public Message message { get; set; }
        public string finish_reason { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }    

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }


    private void Start()
    {
        //inputField.onEndEdit.AddListener(SubmitInput);
    }
    public async void OnClickButton()
    {
        //outputText.text = "You entered: " + inputField.text;
        //AskQuestion(inputField.text);
        //inputField.text = "";
        inputField.ActivateInputField();
        outputText.text = await AskGpt3Question(inputField.text);
        inputField.text = "";
    }

    public  async void AskQuestion(string question)
    {
        // Code to ask the question to GPT and get an answer
        // Replace this with your own implementation
        //string answer = "This is the answer to your question.";
        //outputText.text += "\nGPT Answer: " + answer;
        outputText.text = await AskGpt3Question(question);
        translate.SpeakGPT(outputText.text);
    }


    private async Task<string> AskGpt3Question(string question)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";

        using (HttpClient client = new HttpClient())
        {
            // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            //var requestBody = new
            //{
            //    prompt = $"Q: {question}\nA:",
            //    max_tokens = 50 // Adjust as needed
            //};

            RequestMessage[] requestMessages = new RequestMessage[1] ;
            RequestMessage QuesttionBody = new RequestMessage
            {
                Role = "user",
                Content = question
            };

            Request requestbody = new Request();
            
            if(requestMessages[0] == null)
            {
                requestMessages[0] = QuesttionBody;
            }

            requestbody.Messages = requestMessages;




            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestbody), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {

                string responseBody    = await response.Content.ReadAsStringAsync();

                Root jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(responseBody);
                return jsonResponse.choices[0].message.content;
            }
            else
            {
                return "Error: Unable to get response from GPT-3.";
            }
        }
    }

}