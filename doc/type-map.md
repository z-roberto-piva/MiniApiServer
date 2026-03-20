# Mappa dei tipi pubblici

## Domain

- `DataIn`: input applicativo da elaborare e relativo stato.
- `DataSumm`, `DataSubtraction`, `DataMultiplication`, `DataDivision`: risultati persistiti delle elaborazioni.
- `Stat`: snapshot aggregata delle operazioni giornaliere.
- `OperationStatus`: stato di avanzamento dell'input.
- `IData*Repository`, `IStatRepository`: contratti di persistenza usati dal layer applicativo.
- `DomainException`: eccezione per violazioni delle invarianti di dominio.

## Application

- `CreateInputDataUseCase`: crea l'input e pubblica i job.
- `ProcessSumUseCase`, `ProcessSubtractionUseCase`, `ProcessMultiplicationUseCase`, `ProcessDivisionUseCase`: use case di calcolo.
- `GenerateDailyStatsUseCase`: produce le statistiche giornaliere.
- `CreateInputDataCommand`, `Process*Command`, `GenerateDailyStatsCommand`: input dei casi d'uso.
- `CreateInputDataResult`, `Process*Result`, `GenerateDailyStatsResult`, `DailyOperationsSummary`: output applicativi.
- `IBackgroundJobScheduler`: astrazione per l'enqueue dei job.
- `IBackgroundJobCategoryResolver`: mappa un tipo di job nella categoria applicativa.
- `IDailyOperationsSummaryReader`: estrae il riepilogo aggregato da persistenza.
- `IDataInStatusCoordinator`: centralizza le transizioni di stato del record `DataIn`.

## Infrastructure

- `ServiceCollectionExtensions`: punto di registrazione DI dell'intera infrastruttura.
- `MiniApiServerDbContext`, `MiniApiServerDbContextFactory`: accesso EF Core e supporto design-time.
- `Data*Configuration`, `StatConfiguration`: mapping tabelle e colonne.
- `Data*Repository`, `StatRepository`: implementazioni dei repository di dominio.
- `DailyOperationsSummaryReader`: query specializzata per l'aggregazione giornaliera.
- `DataInStatusCoordinator`: applica le transizioni di stato nel database.
- `HangfireBackgroundJobScheduler`: adapter Hangfire per la pubblicazione dei job.
- `Process*Job`, `GenerateDailyStatsRecurringJob`: wrapper eseguibili da Hangfire.
- `ConfiguredBackgroundJobCategoryResolver`, `BackgroundJobQueueResolver`: risoluzione di categoria e coda.
- `HangfireJobCategoryOptions`, `HangfireQueueOptions`, `JobExecutionDelayOptions`: opzioni di configurazione.
- `JobExecutionDelaySimulator`, `RandomDelaySelector`, `TaskDelayAwaiter`: componenti di supporto per la simulazione del ritardo.

## Api

- `DataInController`: endpoint REST per creare un input.
- `CreateDataInRequest`: payload HTTP in ingresso.
- `CreateDataInResponse`: payload HTTP in uscita.

## Worker

- `WorkerBootstrapService`: registra il recurring job giornaliero e avvia il worker Hangfire.
