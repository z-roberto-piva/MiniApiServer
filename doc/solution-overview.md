# Panoramica della solution

## Obiettivo

`MiniApiServer` e una solution dimostrativa per sperimentare Hangfire con:

- Web API dedicata alla raccolta di input applicativi;
- persistenza applicativa PostgreSQL;
- storage Hangfire separato su PostgreSQL;
- worker separato per l'esecuzione dei job;
- use case applicativi espliciti per orchestrare input, elaborazioni e statistiche giornaliere.

## Struttura dei progetti

### `src/MiniApiServer.Domain`

Layer puro di dominio. Contiene:

- entita persistite (`DataIn`, `DataSumm`, `DataSubtraction`, `DataMultiplication`, `DataDivision`, `Stat`);
- enum di dominio (`OperationStatus`);
- contratti repository usati dai layer superiori;
- eccezioni di dominio.

Il progetto non dipende da Hangfire, EF Core o ASP.NET Core.

### `src/MiniApiServer.Application`

Layer applicativo che espone i casi d'uso:

- `CreateInputDataUseCase`: crea l'input e pianifica i job di background;
- `ProcessSumUseCase`, `ProcessSubtractionUseCase`, `ProcessMultiplicationUseCase`, `ProcessDivisionUseCase`: elaborano il singolo input e persistono i risultati;
- `GenerateDailyStatsUseCase`: aggrega i dati giornalieri e produce un record `Stat`.

Contiene anche:

- contratti input/output dei casi d'uso;
- astrazioni per scheduling job, query di riepilogo e coordinamento di stato;
- enum applicativi che descrivono tipo e priorita dei job.

### `src/MiniApiServer.Infrastructure`

Layer di integrazione tecnica. Contiene:

- `MiniApiServerDbContext` e mapping EF Core;
- implementazioni dei repository di dominio;
- query reader per il riepilogo giornaliero;
- integrazione Hangfire per enqueue, wrapper dei job e risoluzione di code/categorie;
- estensioni di dependency injection per registrare database, Hangfire, repository e use case.

Questo progetto e il punto in cui vengono lette le connection string separate per database applicativo e database Hangfire.

### `src/MiniApiServer.Api`

Host HTTP della solution. Responsabilita:

- bootstrap ASP.NET Core;
- configurazione Swagger e logging;
- controller REST `DataInController`;
- request/response contract esposti all'esterno.

Il controller delega il lavoro al layer `Application` e non contiene logica di business.

### `src/MiniApiServer.Worker`

Host separato per l'esecuzione dei background job. Responsabilita:

- bootstrap del processo worker;
- avvio del server Hangfire;
- registrazione del recurring job giornaliero per le statistiche.

La logica applicativa rimane nei use case e nei job wrapper di `Infrastructure`.

### `tests/*`

La solution contiene test mirati per:

- regole di dominio;
- use case applicativi;
- componenti infrastrutturali di Hangfire.

## Flusso applicativo principale

1. `POST /api/data-in` riceve un payload con descrizione e operandi.
2. `DataInController` costruisce `CreateInputDataCommand` e invoca `CreateInputDataUseCase`.
3. Il use case crea l'entita `DataIn`, la salva nel database applicativo e mette in coda i job Hangfire richiesti.
4. Il progetto `MiniApiServer.Worker` esegue i job Hangfire tramite i wrapper `Process*Job`.
5. Ogni job delega al relativo use case applicativo che legge l'input, produce il risultato e salva l'output nella tabella dedicata.
6. Il coordinatore di stato aggiorna `DataIn.Status` in modo coerente con l'avanzamento delle elaborazioni.
7. Il recurring job `GenerateDailyStatsRecurringJob` esegue ogni giorno `GenerateDailyStatsUseCase`, che aggrega i dati di giornata e salva una riga in `stats`.

## Convenzioni utili

- `AssemblyMarker` viene usato come riferimento di assembly per registrazioni o scansioni.
- I nomi delle tabelle e delle entita seguono il perimetro richiesto dal progetto, anche quando non perfettamente idiomatici, come `data_summs`.
- I wrapper Hangfire non contengono business logic: logging, eventuale delay simulato e delega al use case.
