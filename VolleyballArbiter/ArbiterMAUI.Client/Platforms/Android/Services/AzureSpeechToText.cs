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
            var speechConfig = SpeechConfig.FromSubscription("-", "-");
            speechConfig.SpeechRecognitionLanguage = culture.Name;

            var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var stopRecognitionTask = new TaskCompletionSource<int>();

            var recognizedText = "";

            recognizer.Recognizing += (sender, e) =>
            {
                //recognizedText += e.Result.Text + " ";
                //recognitionResult.Report(e.Result.Text);
            };

            recognizer.Recognized += (sender, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedText += e.Result.Text + " ";
                    recognitionResult.Report(e.Result.Text);
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    // Can't be recognized
                }
            };

            recognizer.Canceled += (sender, e) =>
            {
                if (e.Reason == CancellationReason.Error)
                {
                    // Handle error
                }

                stopRecognitionTask.TrySetResult(0);
            };

            recognizer.SessionStarted += (sender, e) =>
            {
                // Recognition session started
            };

            recognizer.SessionStopped += (sender, e) =>
            {
                // Recognition session stopped
                stopRecognitionTask.TrySetResult(0);
            };

            await recognizer.StartContinuousRecognitionAsync();

            // Wait for recognition to finish or cancellation token to be triggered
            await Task.WhenAny(stopRecognitionTask.Task, Task.Delay(Timeout.Infinite, cancellationToken));

            // Stop recognition
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
