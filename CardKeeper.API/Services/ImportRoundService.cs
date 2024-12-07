using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardKeeper.API.Models;
using CardKeeper.API.Services.Interfaces;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Files;

namespace CardKeeper.API.Services
{
    public class ImportRoundService : IImportRoundService
    {
        private readonly OpenAIClient _openAIClient;
        private readonly OpenAIFileClient _fileClient;
        #pragma warning disable OPENAI001
        private readonly AssistantClient _assistantClient;
        const string API_KEY = "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";
        public ImportRoundService()
        {
            _openAIClient = new OpenAIClient(API_KEY);
            _fileClient = _openAIClient.GetOpenAIFileClient();
            _assistantClient = _openAIClient.GetAssistantClient();
        }

        public async Task<Scorecard> ImportRound(string imagePath, string playerName, string teeColor)
        {
            OpenAIFile imageFile = _fileClient.UploadFile(imagePath, FileUploadPurpose.Vision);

            Assistant assistant = _assistantClient.CreateAssistant(
                "gpt-4o",
                new AssistantCreationOptions()
                {
                    Instructions = "You are an assistant used to extract data from an image of a golf scorecard. The scorecard includes information about the golf course name, the date the round was played, and the tee used (e.g., blue, white, red). All you will do is return JSON data. You will always return the data in the following JSON format: {\"courseName\": \"<course name>\", \"date\": \"<date>\", \"holes\": {\"hole_1\": {\"yardage\": \"<yardage>\", \"par\": \"<par>\", \"handicap\": \"<handicap>\", \"playerScore\": \"<player's score>\"}, \"hole_2\": {\"yardage\": \"<yardage>\", \"par\": \"<par>\", \"handicap\": \"<handicap>\", \"playerScore\": \"<player's score>\"}, ...}, \"frontNineScore\": \"<total score for front nine>\", \"backNineScore\": \"<total score for back nine>\", \"totalScore\": \"<total score for the round>\"}"
                }
            );

            AssistantThread thread = _assistantClient.CreateThread(new ThreadCreationOptions()
            {
                InitialMessages =
                    {
                        new ThreadInitializationMessage(
                            MessageRole.User,
                            [
                                $"Hello, assistant! Analyze the attached image of a golf course scorecard. Extract only the data for the {playerName} row, and assume the player played from the {teeColor} tees. For each hole, extract the yardage, par, handicap, and the player's score. At the end, calculate and provide the total scores for the front nine, back nine, and the entire round. Ensure the JSON keys are accurately populated with their respective values from the image.",
                                MessageContent.FromImageFileId(imageFile.Id),
                            ]),
                    }
            });

            CollectionResult<StreamingUpdate> streamingUpdates = _assistantClient.CreateRunStreaming(
                thread.Id,
                assistant.Id
            );

            string rawReply = "";

            foreach (StreamingUpdate streamingUpdate in streamingUpdates)
            {
                if (streamingUpdate is MessageContentUpdate contentUpdate)
                {
                    rawReply += contentUpdate.Text;
                }
            }

            string strippedReply = rawReply.TrimStart('`').TrimStart("json".ToCharArray()).TrimStart('\n').TrimEnd('`').Trim();

            Scorecard scorecard = JsonConvert.DeserializeObject<Scorecard>(strippedReply);

            return scorecard;
        }
    }
}
                