#!/usr/bin/env python3

from __future__ import annotations

import argparse
import csv
import json
import os
import random
import sys
import urllib.error
import urllib.request
from pathlib import Path


def parse_args() -> argparse.Namespace:
    script_dir = Path(__file__).resolve().parent

    parser = argparse.ArgumentParser(
        description="Invia richieste di test al POST /api/data-in del progetto."
    )
    parser.add_argument(
        "--mode",
        choices=("csv", "random"),
        default="csv",
        help="Modalita' di alimentazione dei dati: csv oppure random. Default: %(default)s",
    )
    parser.add_argument(
        "--base-url",
        default=os.environ.get("MINI_API_SERVER_BASE_URL", "http://localhost:5269"),
        help="Base URL dell'API. Default: %(default)s",
    )
    parser.add_argument(
        "--csv",
        type=Path,
        default=script_dir / "test.csv",
        help="Percorso del CSV di input. Default: %(default)s",
    )
    parser.add_argument(
        "--limit",
        type=int,
        default=None,
        help="Numero massimo di record da inviare in modalita' csv.",
    )
    parser.add_argument(
        "--count",
        type=int,
        default=1000,
        help="Numero di record da generare in modalita' random. Default: %(default)s",
    )
    parser.add_argument(
        "--seed",
        type=int,
        default=None,
        help="Seed opzionale per rendere ripetibile la generazione random.",
    )
    parser.add_argument(
        "--min-value",
        type=int,
        default=0,
        help="Valore minimo incluso per dataA e dataB in modalita' random. Default: %(default)s",
    )
    parser.add_argument(
        "--max-value",
        type=int,
        default=999,
        help="Valore massimo incluso per dataA e dataB in modalita' random. Default: %(default)s",
    )
    parser.add_argument(
        "--timeout",
        type=float,
        default=10.0,
        help="Timeout HTTP in secondi. Default: %(default)s",
    )
    parser.add_argument(
        "--progress-every",
        type=int,
        default=100,
        help="Stampa un avanzamento ogni N richieste riuscite. Default: %(default)s",
    )
    return parser.parse_args()


def read_rows(csv_path: Path, limit: int | None) -> list[dict[str, str]]:
    with csv_path.open("r", encoding="utf-8", newline="") as file_handle:
        rows = list(csv.DictReader(file_handle))

    if limit is None:
        return rows

    return rows[:limit]


def generate_rows(
    count: int,
    seed: int | None,
    min_value: int,
    max_value: int,
) -> list[dict[str, int | str]]:
    if count <= 0:
        raise ValueError("--count deve essere maggiore di zero.")

    if min_value > max_value:
        raise ValueError("--min-value non puo' essere maggiore di --max-value.")

    generator = random.Random(seed)

    return [
        {
            "description": f"r{index:04d}",
            "dataA": generator.randint(min_value, max_value),
            "dataB": generator.randint(min_value, max_value),
        }
        for index in range(1, count + 1)
    ]


def load_rows(args: argparse.Namespace) -> list[dict[str, int | str]]:
    if args.mode == "random":
        return generate_rows(args.count, args.seed, args.min_value, args.max_value)

    return read_rows(args.csv, args.limit)


def post_json(base_url: str, payload: dict[str, object], timeout: float) -> tuple[int, str]:
    request = urllib.request.Request(
        url=f"{base_url.rstrip('/')}/api/data-in",
        data=json.dumps(payload).encode("utf-8"),
        headers={
            "Content-Type": "application/json",
            "Accept": "application/json",
        },
        method="POST",
    )

    with urllib.request.urlopen(request, timeout=timeout) as response:
        response_body = response.read().decode("utf-8")
        return response.status, response_body


def main() -> int:
    args = parse_args()

    try:
        rows = load_rows(args)
    except ValueError as exc:
        print(str(exc), file=sys.stderr)
        return 1

    if not rows:
        if args.mode == "csv":
            print(f"Nessun record trovato in {args.csv}.", file=sys.stderr)
        else:
            print("Nessun record generato.", file=sys.stderr)
        return 1

    successes = 0
    failures = 0

    for index, row in enumerate(rows, start=1):
        payload = {
            "description": row["description"],
            "dataA": int(row["dataA"]),
            "dataB": int(row["dataB"]),
        }

        try:
            status_code, _ = post_json(args.base_url, payload, args.timeout)
            if 200 <= status_code < 300:
                successes += 1
                if args.progress_every > 0 and successes % args.progress_every == 0:
                    print(f"Inviate {successes} richieste con successo.")
            else:
                failures += 1
                print(
                    f"[{index}] Richiesta non riuscita con status {status_code}: {payload}",
                    file=sys.stderr,
                )
        except urllib.error.HTTPError as exc:
            failures += 1
            error_body = exc.read().decode("utf-8", errors="replace")
            print(
                f"[{index}] HTTP {exc.code} per payload {payload}: {error_body}",
                file=sys.stderr,
            )
        except urllib.error.URLError as exc:
            failures += 1
            print(
                f"[{index}] Errore di connessione per payload {payload}: {exc.reason}",
                file=sys.stderr,
            )
        except ValueError as exc:
            failures += 1
            print(f"[{index}] Record CSV non valido {row}: {exc}", file=sys.stderr)

    print(
        f"Completato. Totale={len(rows)}, Successi={successes}, Fallimenti={failures}."
    )

    return 0 if failures == 0 else 1


if __name__ == "__main__":
    raise SystemExit(main())
