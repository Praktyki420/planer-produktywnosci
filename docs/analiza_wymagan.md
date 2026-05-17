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
