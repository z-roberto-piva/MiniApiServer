# Piano di implementazione

## Obiettivo

Realizzare una soluzione dimostrativa per sperimentare Hangfire con PostgreSQL, separando Web API e Worker, nel rispetto dei vincoli DDD e Clean Architecture descritti in `agents.md`.

## Assunzioni iniziali

- Mantengo `.NET 8` come target framework, coerente con il progetto attuale.
- Propongo di rinominare la soluzione logica in modo più esplicito, mantenendo però il repository corrente.
- Userò un database `PostgreSQL` per i dati applicativi e un database `PostgreSQL` separato per lo storage di Hangfire.
- Considero `data_a`, `data_b` e `result` come numeri interi, salvo diversa indicazione.
- Il campo `date` di `stats` rappresenterà il giorno di competenza del riepilogo.

## Struttura proposta della solution

```text
MiniApiServer.slnx
src/
  MiniApiServer.Domain/
  MiniApiServer.Application/
  MiniApiServer.Infrastructure/
  MiniApiServer.Api/
  MiniApiServer.Worker/
tests/
  MiniApiServer.Domain.Tests/
  MiniApiServer.Application.Tests/
  MiniApiServer.Infrastructure.Tests/
```

## Fase 1: bootstrap architetturale

### Attività

- Creare i nuovi progetti per `Domain`, `Application`, `Infrastructure`, `Api` e `Worker`.
- Spostare l'attuale progetto web dentro `src/MiniApiServer.Api` oppure sostituirlo con un nuovo progetto API pulito.
- Aggiornare la solution per includere tutti i progetti applicativi e di test.
- Definire i riferimenti tra layer nel rispetto delle dipendenze consentite.

### Deliverable

- Solution organizzata in layer.
- Dipendenze tra progetti coerenti con Clean Architecture.

## Fase 1.5: aggiunta della libreria Serilog per la gestione dell'output dei log

### Attività

- Aggiungere la libreria srilog e la relativa configurazione all'appsettings.json

### Deliverable

- Le applicazioni della soluzione devono loggare su standard output con il formato di default di Serilog

## Fase 2: modellazione domain

### Attività

- Introdurre enum `OperationStatus` con valori `TODO`, `DOING`, `DONE`.
- Modellare le entità:
  - `DataIn`
  - `DataSumm`
  - `DataSubtraction`
  - `Stat`
- Definire regole di dominio minime:
  - creazione consistente di `DataIn`;
  - transizioni di stato controllate;
  - protezione da stati invalidi.
- Definire le interfacce repository nel layer `Domain` o `Application`, in base al confine scelto.

### Deliverable

- Modello di dominio indipendente da EF Core e Hangfire.

## Fase 3: casi d'uso applicativi

### Attività

- Implementare i use case:
  - `CreateInputData`
  - `ProcessSum`
  - `ProcessSubtraction`
  - `GenerateDailyStats`
- Definire DTO e command request/response minimi.
- Introdurre un'astrazione per l'enqueue dei job, così l'Application non dipende direttamente da Hangfire.
- Stabilire il flusso applicativo:
  - creazione record su `data_in`;
  - enqueue dei due job;
  - aggiornamento stato durante l'elaborazione;
  - persistenza dei risultati in `data_summs` e `data_subtractions`;
  - generazione del riepilogo giornaliero su `stats`.

### Deliverable

- Servizi applicativi testabili e indipendenti dal transport layer.

## Fase 4: infrastructure con PostgreSQL e Hangfire

### Attività

- Configurare `DbContext` EF Core per PostgreSQL.
- Configurare due connection string distinte:
  - una per il database applicativo;
  - una per il database dedicato a Hangfire.
- Mappare le tabelle richieste mantenendo i nomi esatti:
  - `data_in`
  - `data_summs`
  - `data_subtractions`
  - `stats`
- Configurare la persistenza dell'enum `OperationStatus`.
- Implementare i repository concreti.
- Aggiungere migration iniziale.
- Configurare Hangfire con storage PostgreSQL sul database dedicato.
- Implementare i job wrapper Hangfire che delegano ai use case applicativi.
- Configurare il recurring job giornaliero per le statistiche.

### Deliverable

- Persistenza funzionante su PostgreSQL.
- Hangfire configurato e pronto a schedulare/eseguire job.

## Fase 5: API REST

### Attività

- Esporre endpoint `POST /api/data-in`.
- Definire payload minimo con:
  - `description`
  - `dataA`
  - `dataB`
- Aggiungere validazioni superficiali lato API.
- Collegare il controller al use case `CreateInputData`.
- Restituire una risposta HTTP chiara con identificativo del record creato.

### Deliverable

- Endpoint API che salva l'input e crea i job Hangfire.

## Fase 6: Worker dedicato

### Attività

- Creare host separato `MiniApiServer.Worker`.
- Configurare DI, Infrastructure e Hangfire Server nel worker.
- Registrare i processor/job necessari all'esecuzione dei task enqueueati dalla API.
- Assicurare che nel worker non ci sia logica di business oltre al bootstrap.

### Deliverable

- Applicazione worker separata che processa i job Hangfire.

## Fase 7: test

### Attività

- Aggiungere unit test per:
  - entità e transizioni di stato;
  - `CreateInputData`;
  - `ProcessSum`;
  - `ProcessSubtraction`;
  - `GenerateDailyStats`.
- Aggiungere test di integrazione mirati per:
  - mapping EF Core;
  - persistenza principale;
  - endpoint `POST /api/data-in`.
- Verificare che i job deleghino correttamente ai casi d'uso.

### Deliverable

- Copertura minima coerente con la `Definition of Done` di `agents.md`.

## Fase 8: configurazione ed esecuzione locale

### Attività

- Introdurre configurazione connection string PostgreSQL per API e Worker.
- Distinguere esplicitamente la connection string del database applicativo da quella del database Hangfire.
- Preparare `appsettings` distinti per i due host.
- Documentare i passaggi minimi per:
  - avviare PostgreSQL;
  - applicare le migration;
  - avviare API;
  - avviare Worker;
  - verificare il job giornaliero.

### Deliverable

- Soluzione eseguibile in locale con setup chiaro.

## Sequenza di implementazione consigliata

1. Riorganizzazione solution e progetti.
2. Modello di dominio.
3. Use case applicativi.
4. Infrastructure EF Core + PostgreSQL.
5. Configurazione Hangfire.
6. Endpoint API.
7. Worker dedicato.
8. Test unitari e di integrazione.
9. Verifica end-to-end locale.

## Dettagli implementativi che intendo seguire

- Userò interfacce applicative per isolare Hangfire dal core applicativo.
- Userò connection string separate per isolare i dati di dominio dalle tabelle tecniche di Hangfire.
- I job Hangfire conterranno solo orchestrazione tecnica e delega ai use case.
- Lo stato di `data_in` verrà aggiornato in modo esplicito durante l'esecuzione dei job, evitando logica implicita nei controller.
- Il job giornaliero leggerà i risultati prodotti nella giornata e scriverà un record di riepilogo in `stats`.
- I nomi tabellari richiesti verranno preservati anche se non idiomatici.

## Rischi o decisioni da confermare durante l'implementazione

- Se `n_of_operations` debba rappresentare il numero di record in `data_in` creati nel giorno o il numero totale di job eseguiti.
- Se `total_of_sums` e `total_of_subtractions` debbano essere somme aritmetiche dei risultati o conteggi delle operazioni concluse.
- Se serva aggiungere una chiave primaria esplicita a tutte le tabelle, anche se nelle specifiche non è stata elencata.
- Se il riepilogo giornaliero debba usare il fuso orario locale o UTC per determinare il perimetro della giornata.
- Se il database Hangfire debba stare sullo stesso server PostgreSQL del database applicativo o su un server separato.

## Output atteso a fine implementazione

- Una Web API che inserisce dati in `data_in` e accoda due job Hangfire.
- Un Worker separato che esegue somma e sottrazione e salva i risultati nelle tabelle dedicate.
- Un job schedulato giornaliero che popola `stats`.
- Test eseguiti con esito positivo.
