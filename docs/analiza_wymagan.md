# Analiza wymagań

## Cel systemu

System Planer Produktywności umożliwia użytkownikom zarządzanie
zadaniami na wielu platformach z dostępem do wspólnych danych.

## Wymagania funkcjonalne

### Zarządzanie użytkownikami
- System umożliwia rejestrację nowego użytkownika
- System umożliwia logowanie istniejącego użytkownika
- Hasła są przechowywane w formie zahashowanej (SHA256)

### Zarządzanie zadaniami
- Użytkownik może tworzyć zadania z tytułem, opisem,
  kategorią, priorytetem i terminem
- Użytkownik może edytować istniejące zadania
- Użytkownik może usuwać zadania
- Użytkownik może oznaczać status zadania:
  Nowe / W trakcie / Wykonane

### Filtrowanie i wyszukiwanie
- Użytkownik może filtrować zadania po statusie
- Użytkownik może filtrować zadania po kategorii
- Użytkownik może wyszukiwać zadania po tytule

## Wymagania niefunkcjonalne

| Wymaganie | Opis |
|-----------|------|
| Wydajność | API odpowiada w czasie poniżej 1 sekundy |
| Bezpieczeństwo | Hasła hashowane algorytmem SHA256 |
| Responsywność | Aplikacja webowa działa na mobile i desktop |
| Przenośność | Backend działa na Windows, Linux, macOS |

## Diagram przypadków użycia
Użytkownik
├── Rejestracja
├── Logowanie
├── Przeglądanie zadań
│   ├── Filtrowanie po statusie
│   └── Filtrowanie po kategorii
├── Dodawanie zadania
├── Edycja zadania
├── Usuwanie zadania
└── Oznaczanie zadania jako wykonane

## Model danych

### Tabela Users
| Pole | Typ | Opis |
|------|-----|------|
| Id | int | Klucz główny |
| Username | string | Nazwa użytkownika |
| Email | string | Adres email (unikalny) |
| PasswordHash | string | Zahashowane hasło |
| CreatedAt | DateTime | Data rejestracji |

### Tabela Tasks
| Pole | Typ | Opis |
|------|-----|------|
| Id | int | Klucz główny |
| Title | string | Tytuł zadania |
| Description | string | Opis zadania |
| Status | string | Nowe / W trakcie / Wykonane |
| Priority | string | Niski / Średni / Wysoki |
| Category | string | Kategoria zadania |
| DueDate | DateTime? | Termin (opcjonalny) |
| CreatedAt | DateTime | Data utworzenia |
| UserId | int | Klucz obcy do Users |
