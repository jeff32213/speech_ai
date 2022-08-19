using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.PronunciationAssessment;

namespace speechai
{
    class Program
    {
        static async Task Main()
        {
            while(true){
                await RecognitionWithLanguageAndDetailedOutputAsync();
            }
        }
       
        public static async Task RecognitionWithLanguageAndDetailedOutputAsync()
        {
            var config = SpeechConfig.FromSubscription("4326e1af28f84740bf600bb159f8ed19", "westus");

            var language = "zh-TW";

            config.OutputFormat = OutputFormat.Detailed;

            config.SpeechSynthesisLanguage = language;

            config.RequestWordLevelTimestamps();

            using (var recognizer = new SpeechRecognizer(config, language))
            {
                Console.WriteLine($"用中文說點東西...");

                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                var synthesizer = new SpeechSynthesizer(config);

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine($"RECOGNIZED: {result.Text}");
                    
                    var detailedResults = result.Best();
                    foreach (var item in detailedResults)
                    {
                        if(item.Text.Equals("你好。"))
                        {
                            await synthesizer.SpeakTextAsync("你好");
                            break;
                        }

                        if (item.Text.Equals("今天天氣如何？"))
                        {
                            await synthesizer.SpeakTextAsync("非常好");
                            break;
                        }

                        if (item.Text.Equals("再見。"))
                        {
                            await synthesizer.SpeakTextAsync("掰掰~");
                            break;
                        }
                    }
                }

            }
        }
    }
}
