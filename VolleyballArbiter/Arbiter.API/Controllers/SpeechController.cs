using Arbiter.API.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Options;

namespace Arbiter.API.Controllers
{
    [ApiController]
    [Route("speech")]
    public class SpeechController : ControllerBase
    {
        private readonly IOptions<AzureSpeechServiceOptions> _options;

        public SpeechController(IOptions<AzureSpeechServiceOptions> options)
        {
            _options = options;
        }

        [HttpPost]
        [Route("recognise")]
        public async Task<IActionResult> Recognise(IFormFile audio)
        {
            var config = SpeechConfig.FromSubscription(_options.Value.SubscriptionKey, _options.Value.ServiceRegion);
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages([ "pl-PL" ]);

            var reader = new BinaryReader(audio.OpenReadStream());
            var format = AudioStreamFormat.GetWaveFormat(16000, 16, 1, AudioStreamWaveFormat.PCM);
            using var audioConfigStream = AudioInputStream.CreatePushStream(format);

            using var audioConfig = AudioConfig.FromStreamInput(audioConfigStream);
            using var speechRecognizer = new SpeechRecognizer(config, autoDetectSourceLanguageConfig, audioConfig);

            byte[] readBytes;
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioConfigStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);

            var result = await speechRecognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                return Ok(result.Text);
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                return UnprocessableEntity("Speech could not be recognized.");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                return Problem(cancellation.ErrorDetails);
            }

            return NoContent();
        }
    }
}
 