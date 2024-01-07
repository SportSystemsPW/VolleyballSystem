using ArbiterMAUI.Client.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbiterMAUI.Client.Services
{
    //UNUSED
    public class SignalrTextAnalyzerService : ISignalrTextAnalyzerService
    {
        private readonly HubConnection _hubConnection;

        public SignalrTextAnalyzerService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://192.168.100.16:44396/hub/match-report-text-anaylzer", 
                (opts) =>
                {
                    opts.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            clientHandler.ServerCertificateCustomValidationCallback =
                                (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        return message;
                    };
                    opts.WebSocketConfiguration = wsc => wsc.RemoteCertificateValidationCallback = (sender, certificate, chain, policyErrors) => true;
                })
                .WithAutomaticReconnect()
                .Build();
             
            _hubConnection.On<string, int>("ReceiveTextSentenceToAnalyze", (textSentence, matchId) =>
            {

            });
        }

        public async Task StartConnection()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task StopConnection()
        {
            try
            {
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    await _hubConnection.StopAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task SendTextSentence(string text, int matchId)
        {
            try
            {
                await _hubConnection.InvokeAsync("SendTextSentenceToAnalyze", text, matchId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
