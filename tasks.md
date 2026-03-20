# Piano di implementazione

## Obiettivo

Realizzare una soluzione dimostrativa per sperimentare Hangfire con PostgreSQL, separando Web API e Worker, nel rispetto dei vincoli DDD e Clean Architecture descritti in `agents.md`.

## Assunzioni iniziali

- Mantengo `.NET 8` come target framework, coerente con il progetto attuale.
- Propongo di rinominare la soluzione logica in modo più esplicito, mantenendo però il repository corrente.
- Userò un database `PostgreSQL` per i dati applicativi e un database `PostgreSQL` separato per lo storage di Hangfire.
- Considero `data_a`, `data_b` e `result` come numeri interi, salvo diversa indicazione.
- Il campo `date` di `stats` rappresenterà il giorno di competenza del riepilogo.
- `n_of_operations` rappresenterà il numero di record presenti in `data_in` per la giornata di competenza.
- I totali statistici rappresenteranno il numero di operazioni concluse per tipo, non la somma aritmetica dei risultati.
- Il riepilogo giornaliero ragionerà in `UTC`.
- Il database Hangfire resterà separato da quello applicativo, come già implementato.

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

## Fase 6.5: rafforzamento del coordinatore di stato

### Attività

- Migliorare il coordinatore che aggiorna `data_in` per gestire correttamente concorrenza e retry.
- Rendere l'aggiornamento dello stato idempotente rispetto all'esecuzione ripetuta dei job.
- Evitare race condition tra job di somma e job di sottrazione eseguiti da worker diversi.
- Valutare protezioni a livello database:
  - lock espliciti;
  - vincoli di unicità sui risultati;
  - controlli ottimistici o pessimisti sulla riga sorgente.
- Fare in modo che lo stato finale venga determinato in base ai risultati realmente persistiti.

### Deliverable

- Coordinatore di stato robusto rispetto a più worker Hangfire attivi in parallelo.

## Fase 7: nuovi job di multiplication e division

### Attività

- Estendere il flusso di input per creare anche due nuovi job Hangfire:
  - `multiplication`
  - `division`
- Introdurre i casi d'uso applicativi e i job wrapper dedicati alle due nuove operazioni.
- Modellare e persistere i risultati delle due nuove operazioni in strutture dedicate, coerenti con l'architettura esistente.
- Definire la regola di business della divisione:
  - se non si verifica division by zero, eseguire la divisione;
  - se si verifica division by zero, salvare come risultato `0`.
- Aggiornare il coordinatore di stato per considerare correttamente anche i nuovi job nel completamento del record sorgente.

### Deliverable

- Due nuovi job Hangfire, `multiplication` e `division`, integrati nel flusso applicativo e persistiti correttamente.

## Fase 7.5: introduzione di uno sleep random nei job

### Attività

- Introdurre un ritardo casuale controllato durante l'esecuzione dei job Hangfire di:
  - `sum`
  - `subtraction`
  - `multiplication`
  - `division`
- Vincolare il ritardo casuale ai soli valori:
  - `5`
  - `10`
  - `15`
  - `20`
  - `30`
  - `40`
  - `50`
  - `60` secondi
- Rendere il ritardo configurabile e facilmente attivabile o disattivabile per ambiente.
- Usare il ritardo solo come strumento di test e osservabilità del flusso, evitando di legarlo alla logica di business.
- Verificare che con tempi di completamento diversi lo stato di `data_in` transizioni correttamente tra:
  - `TODO`
  - `DOING`
  - `DONE`
- Valutare se aggiungere logging esplicito dell'inizio e della fine di ciascun job per rendere più leggibile la sequenza di esecuzione.

### Deliverable

- Possibilità di simulare latenze variabili nei job per testare in modo più realistico concorrenza e aggiornamento degli stati.

## Fase 7.6: categorizzazione e priorità dei job Hangfire

### Attività

- Introdurre un sistema per categorizzare i job Hangfire in gruppi logici coerenti con il dominio o con esigenze operative.
- Mappare le categorie su meccanismi di esecuzione Hangfire compatibili con la gestione della priorità, ad esempio code dedicate.
- Estendere l'astrazione applicativa di scheduling per permettere l'enqueue dei job con categoria esplicita.
- Configurare il worker per consumare le code in un ordine che rifletta la priorità desiderata.
- Definire una strategia minima di priorità, ad esempio distinguendo tra:
  - job ad alta priorità;
  - job standard;
  - job differibili o a priorità bassa.
- Valutare se associare categorie e priorità in modo statico per tipo di job oppure tramite configurazione.
- Verificare che la categorizzazione non introduca logica di business nei job wrapper e resti confinata a livello infrastrutturale.

### Deliverable

- Sistema di categorizzazione dei job Hangfire con priorità di esecuzione governata dalla categoria.

## Fase 7.7: adeguamento della tabella stats e del riepilogo giornaliero

### Attività

- Estendere la tabella `stats` per rappresentare il conteggio delle operazioni concluse per tutti i tipi supportati:
  - `sum`
  - `subtraction`
  - `multiplication`
  - `division`
- Aggiornare il modello di dominio, i mapping EF Core e le migration per includere i nuovi campi statistici.
- Adeguare il job giornaliero di riepilogo affinché:
  - usi `UTC` per il perimetro della giornata;
  - calcoli `n_of_operations` come numero di record in `data_in`;
  - calcoli i totali come conteggio delle operazioni concluse per categoria.
- Aggiornare query, DTO e test relativi alla generazione delle statistiche giornaliere.
- Verificare che il riepilogo resti coerente con la presenza di più job per singolo record sorgente.

### Deliverable

- Tabella `stats` e job giornaliero coerenti con il nuovo perimetro funzionale dell'applicazione.

## Fase 8: test

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

## Fase 9: configurazione ed esecuzione locale

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

### Nota sulla schedulazione del job `stats`

- La schedulazione del recurring job delle statistiche è controllata da `Hangfire:RecurringJobs:DailyStatsCron` nel file [appsettings.json](/Users/pivrob/Sviluppo/esperimenti/MiniApiServer/src/MiniApiServer.Worker/appsettings.json).
- La cron expression viene interpretata in `UTC`.
- Esempi utili:
  - `59 23 * * *` per eseguire ogni giorno alle `23:59 UTC`
  - `5 0 * * *` per eseguire ogni giorno alle `00:05 UTC`
  - `0 * * * *` per eseguire ogni ora al minuto `0`
  - `*/5 * * * *` per eseguire ogni `5` minuti durante i test
- Dopo la modifica della cron expression è necessario riavviare il worker per aggiornare la registrazione del recurring job.

## Sequenza di implementazione consigliata

1. Riorganizzazione solution e progetti.
2. Modello di dominio.
3. Use case applicativi.
4. Infrastructure EF Core + PostgreSQL.
5. Configurazione Hangfire.
6. Endpoint API.
7. Worker dedicato.
8. Rafforzamento del coordinatore di stato.
9. Nuovi job di multiplication e division.
10. Introduzione di uno sleep random controllato nei job.
11. Categorizzazione dei job Hangfire e gestione della priorità.
12. Adeguamento della tabella `stats` e del riepilogo giornaliero.
13. Test unitari e di integrazione.
14. Verifica end-to-end locale.

## Dettagli implementativi che intendo seguire

- Userò interfacce applicative per isolare Hangfire dal core applicativo.
- Userò connection string separate per isolare i dati di dominio dalle tabelle tecniche di Hangfire.
- I job Hangfire conterranno solo orchestrazione tecnica e delega ai use case.
- Lo stato di `data_in` verrà aggiornato in modo esplicito durante l'esecuzione dei job, evitando logica implicita nei controller.
- Il coordinatore di stato verrà irrobustito per reggere esecuzioni concorrenti di più worker.
- L'aggiunta di nuovi job dovrà mantenere coerenza architetturale ed estendere il coordinamento dello stato senza duplicare logica.
- L'introduzione dello sleep random dovrà restare confinata a un meccanismo tecnico configurabile, così da non contaminare la logica di business.
- La categorizzazione e la priorità dei job dovranno sfruttare le primitive native di Hangfire senza introdurre accoppiamento diretto del dominio alle code tecniche.
- Il job giornaliero leggerà i risultati prodotti nella giornata e scriverà un record di riepilogo in `stats`.
- Il modello di `stats` andrà esteso per includere anche il conteggio di `multiplication` e `division`, coerentemente con l'evoluzione del flusso.
- I nomi tabellari richiesti verranno preservati anche se non idiomatici.

## Rischi o decisioni da confermare durante l'implementazione

- Se serva aggiungere una chiave primaria esplicita a tutte le tabelle, anche se nelle specifiche non è stata elencata.
- Confermare l'estensione della tabella `stats` con i campi per numero di `multiplication` e numero di `division`.
- Se i nuovi risultati di `multiplication` e `division` debbano avere tabelle dedicate analoghe a somma e sottrazione oppure una modellazione più generica.
- Definire una tassonomia di categorie e priorità semplice e didattica, senza vincoli di riuso diretto verso ZMenuNext.

## Output atteso a fine implementazione

- Una Web API che inserisce dati in `data_in` e accoda due job Hangfire.
- Un Worker separato che esegue somma, sottrazione, multiplication e division e salva i risultati nelle tabelle dedicate.
- Un job schedulato giornaliero che popola `stats`.
- Test eseguiti con esito positivo.
