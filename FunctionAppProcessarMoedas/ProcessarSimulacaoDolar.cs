using FunctionAppProcessarMoedas.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FunctionAppProcessarMoedas;

public static class ProcessarSimulacaoDolar
{
    [Function(nameof(ProcessarSimulacaoDolar))]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var logger = context.CreateReplaySafeLogger(nameof(ProcessarSimulacaoDolar));
        logger.LogInformation(
            $"{nameof(ProcessarSimulacaoDolar)} - Iniciando a execucao do metodo {nameof(RunOrchestrator)}...");

        var parametrosExecucao = context.GetInput<ParametrosExecucao>();

        var dadosCotacao = await context.CallActivityAsync<DadosCotacao>(
            nameof(ActivitySimularCotacaoDolar), parametrosExecucao);
        await context.CallActivityAsync<DadosCotacao>(
            nameof(ActivityNotificarAzureQueueStorage), dadosCotacao);

        logger.LogInformation(
            $"{nameof(ProcessarSimulacaoDolar)} - Concluindo a execucao do metodo {nameof(RunOrchestrator)}.");
    }
}