# AGENT.md

## Scopo del progetto

Questa applicazione serve per sperimentare **Hangfire** prima dell’integrazione della libreria nel progetto **ZMenuNext**.

Il sistema deve:
- usare **PostgreSQL** come persistenza applicativa;
- usare un database **PostgreSQL dedicato** per lo storage di **Hangfire**, separato dal database applicativo;
- esporre una **Web API** per ricevere richieste di input;
- salvare i dati ricevuti nella tabella `data_in`;
- generare due job distinti, uno per la **somma** e uno per la **sottrazione**;
- avere una **Worker App** dedicata all’esecuzione dei job;
- eseguire **una volta al giorno** un job di riepilogo che salva le statistiche nella tabella `stats`.

---

## Vincoli architetturali obbligatori

### 1. Architettura
Il progetto deve rispettare i principi di:
- **Domain Driven Design (DDD)**
- **Clean Architecture**

La soluzione deve essere organizzata in layer chiaramente separati, ad esempio:
- **Domain**
  - entità
  - value object
  - enum
  - interfacce repository
  - regole di business
- **Application**
  - use case / application services
  - command / query
  - dto
  - orchestrazione dei job
  - validazioni applicative
- **Infrastructure**
  - persistenza EF Core / PostgreSQL
  - configurazione Hangfire
  - implementazioni repository
  - scheduler
- **Presentation**
  - API REST
- **Worker**
  - host separato per l’esecuzione dei job Hangfire

### 2. Separazione delle responsabilità
- Il **Domain** non deve dipendere da Infrastructure, API o Hangfire.
- La logica di business deve stare nel **Domain** e/o nei **Use Case** applicativi, non nei controller.
- I controller devono limitarsi a:
  - validare il payload a livello superficiale;
  - invocare il caso d’uso applicativo;
  - restituire la risposta HTTP.
- La Worker App deve contenere solo bootstrap, configurazione e avvio dei processor Hangfire.

### 3. Persistenza
Usare **PostgreSQL**.

Il progetto deve distinguere chiaramente:
- **database applicativo** per le tabelle di dominio;
- **database Hangfire** dedicato esclusivamente allo storage dei job e delle tabelle interne di Hangfire.

Le due persistenze devono essere configurate con **connection string separate**.

Tabelle richieste:

#### `data_in`
Campi:
- `description`: string
- `data_a`: number
- `data_b`: number
- `status`: enum con valori:
  - `TODO`
  - `DOING`
  - `DONE`

#### `data_summs`
Campi:
- `description`: string
- `result`: number

#### `data_subtractions`
Campi:
- `description`: string
- `result`: number

#### `stats`
Campi:
- `date`: datetime
- `n_of_operations`: int
- `total_of_sums`: int
- `total_of_subtractions`: int

> Nota: mantenere i nomi richiesti, anche se `data_summs` contiene una grafia non standard.

---

## Comportamento richiesto

### 1. Inserimento dati
Deve esistere un endpoint **POST** API che:
1. riceve un payload con i dati di input;
2. salva il record in `data_in`;
3. crea due job Hangfire:
   - job di **somma**;
   - job di **sottrazione**.

### 2. Elaborazione job
La **Worker App** deve processare i job Hangfire.

Regole minime attese:
- il job di somma legge i dati di input e salva il risultato in `data_summs`;
- il job di sottrazione legge i dati di input e salva il risultato in `data_subtractions`;
- lo stato del record su `data_in` deve essere gestito in modo coerente con il flusso elaborativo.

### 3. Job giornaliero di statistiche
Una volta al giorno deve essere eseguito un job schedulato che:
- riassume le operazioni della giornata;
- salva i risultati nella tabella `stats`.

Il riepilogo deve includere almeno:
- numero totale di operazioni;
- totale delle somme;
- totale delle sottrazioni.

---

## Linee guida implementative

### Modellazione domain
Creare almeno:
- enum `OperationStatus` con valori `TODO`, `DOING`, `DONE`;
- entità coerenti con le tabelle richieste;
- metodi di dominio per proteggere consistenza e transizioni di stato.

### Use case applicativi minimi
Definire casi d’uso espliciti, ad esempio:
- `CreateInputData`
- `ProcessSum`
- `ProcessSubtraction`
- `GenerateDailyStats`

Ogni use case deve avere una responsabilità chiara e testabile.

### Hangfire
- Configurare Hangfire su PostgreSQL usando un database dedicato, separato da quello applicativo.
- Distinguere chiaramente:
  - **job enqueue** lato API / Application;
  - **job execution** lato Worker.
- Evitare logica di business dentro i job wrapper: i job devono delegare ai casi d’uso applicativi.

### API
Esporre endpoint minimali, chiari e versionabili.

Esempio minimo:
- `POST /api/data-in`

### Worker
La worker deve essere un progetto separato, dedicato all’esecuzione dei background jobs.

---

## Qualità del codice

### Regole obbligatorie
- Scrivere codice leggibile, semplice e coerente.
- Favorire nomi espliciti e intenzionali.
- Evitare logica duplicata.
- Evitare dipendenze inutili tra layer.
- Applicare dependency injection ovunque appropriato.
- Ogni nuova funzionalità deve essere accompagnata da test.

### Test
Creare **unit test** per:
- use case applicativi;
- logica di dominio;
- eventuali servizi puri.

Quando utile, aggiungere anche test di integrazione per:
- persistenza;
- configurazione Hangfire;
- endpoint principali.

---

## Regole operative per l’agente

### 1. Eseguire solo task di sviluppo quando richiesto
L’agente deve eseguire **solo attività di sviluppo richieste esplicitamente**.

Non deve:
- introdurre funzionalità non richieste;
- fare refactor estesi non necessari;
- modificare il perimetro architetturale senza motivo.

Può proporre miglioramenti, ma deve mantenere il focus sul task richiesto.

### 2. Definition of Done obbligatoria
Un task può essere dichiarato **completo** solo se:
1. il codice richiesto è stato implementato;
2. i test pertinenti sono stati creati o aggiornati;
3. i test sono stati **eseguiti e validati con esito positivo**.

Se i test non sono stati eseguiti oppure non passano, il task **non può essere dichiarato concluso**.

### 3. Obbligo di trasparenza
Quando conclude un task, l’agente deve riportare chiaramente:
- cosa è stato implementato;
- quali test sono stati aggiunti o aggiornati;
- quali test sono stati eseguiti;
- esito dei test;
- eventuali limiti o punti aperti.

---

## Convenzioni suggerite per la soluzione

Possibile struttura della solution:

```text
HangfirePlayground.sln
src/
  HangfirePlayground.Domain/
  HangfirePlayground.Application/
  HangfirePlayground.Infrastructure/
  HangfirePlayground.Api/
  HangfirePlayground.Worker/
tests/
  HangfirePlayground.Domain.Tests/
  HangfirePlayground.Application.Tests/
  HangfirePlayground.Infrastructure.Tests/
```

---

## Criteri decisionali

In caso di dubbio, preferire sempre:
1. chiarezza architetturale;
2. isolamento della logica di business;
3. testabilità;
4. semplicità di evoluzione futura verso ZMenuNext.

---

## Obiettivo finale

Realizzare una soluzione dimostrativa ma solida, utile come base di apprendimento e come possibile riferimento per l’integrazione futura di Hangfire nel progetto **ZMenuNext**, mantenendo rigore architetturale, separazione delle responsabilità e disciplina sui test.

