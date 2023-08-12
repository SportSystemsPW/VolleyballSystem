using ArbiterMAUI.Client.Services.Interfaces;
using System.Globalization;

namespace ArbiterMAUI.Client.Platforms.Services
{
    public class AzureSpeechToText : ISpeechToTextService
    {
        public Task<string> Listen(CultureInfo culture, IProgress<string> recognitionResult, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestPermissions()
        {
            throw new NotImplementedException();
        }
    }
}
