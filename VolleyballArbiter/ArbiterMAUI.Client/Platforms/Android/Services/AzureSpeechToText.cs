using ArbiterMAUI.Client.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Globalization;

namespace ArbiterMAUI.Client.Platforms.Services
{
    public class AzureSpeechToText : ISpeechToTextService
    {
        public async Task<string> Listen(CultureInfo culture, IProgress<string> recognitionResult, CancellationToken cancellationToken)
        {
            var speechConfig = SpeechConfig.FromSubscription("", "");
            speechConfig.SpeechRecognitionLanguage = culture.Name;

            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var stopRecognitionTask = new TaskCompletionSource<int>();

            var recognizedText = "";

            recognizer.Recognizing += (sender, e) =>
            {
            };

            recognizer.Recognized += (sender, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedText += e.Result.Text + " ";
                    recognitionResult.Report(e.Result.Text);
                }
            };

            recognizer.Canceled += (sender, e) =>
            {
                if (e.Reason == CancellationReason.Error)
                {
                    throw new Exception("Exception in Azure speech to text service");
                }

                stopRecognitionTask.TrySetResult(0);
            };

            recognizer.SessionStarted += (sender, e) =>
            {
            };

            recognizer.SessionStopped += (sender, e) =>
            {
                stopRecognitionTask.TrySetResult(0);
            };

            await recognizer.StartContinuousRecognitionAsync();

            await Task.WhenAny(stopRecognitionTask.Task, Task.Delay(Timeout.Infinite, cancellationToken));
            await recognizer.StopContinuousRecognitionAsync();

            return recognizedText.Trim();
        }

        public async Task<bool> RequestPermissions()
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            return status == PermissionStatus.Granted;
        }
    }
}
